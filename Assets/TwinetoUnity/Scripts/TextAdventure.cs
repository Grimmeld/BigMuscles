using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static SimpleTwineDialogue.TweeParser;

namespace SimpleTwineDialogue
{
    /// <summary>
    /// TextAdventure - A Twine/Twee story player for Unity
    /// 
    /// This script loads and displays interactive fiction stories written in the Twee format.
    /// It handles passage navigation, choice buttons, and image loading from either web or local sources.
    /// 
    /// Key Features:
    /// - Parses Twee files with support for multiple link formats: [[Target]], [[Text|Target]], [[Text->Target]]
    /// - Loads files from web URLs or local StreamingAssets folder
    /// - Displays images referenced in passages
    /// - Creates interactive choice buttons for story navigation
    /// - Tracks user choices
    /// 
    /// Setup Requirements:
    /// 1. Assign UI components in the Inspector (passageText, choiceButtonPrefab, etc.)
    /// 2. Set loadFromWeb to true for web loading or false for local files
    /// 3. For web: Set webFileURL and imageBaseURL
    /// 4. For local: Place Twee file in StreamingAssets folder and set localFileName
    /// 5. Ensure your Twee file has a passage named "Start" (case-sensitive)
    /// </summary>
    public class TextAdventure : MonoBehaviour
    {
        [Header("UI Components")]
        // Text component to display passage content
        public TextMeshProUGUI passageText;

        // Prefab for choice buttons that will be instantiated
        public Button choiceButtonPrefab;

        // Container where choice buttons will be spawned
        public Transform choiceButtonContainer;

        // Container where images will be displayed
        public Transform imageContainer;

        // Prefab for images that will be instantiated
        public Image imagePrefab;

        // Container where character name will be displayed
        [SerializeField] private Transform charnameContainer;
        [SerializeField] private Transform selfnameContainer;

        // Container where images will be displayed
        public Transform imageCharContainer;

        // Container where images will be displayed
        public Transform imageEyeContainer;

        // Counter for tracking how many choices the player has made
        //int myChoices = 0;
        //public TextMeshProUGUI myChoiceCounterUI;

        [Header("File Loading")]
        // Toggle between web and local file loading
        public bool loadFromWeb;

        [Header("Load from Web")]
        // URL to the Twee file hosted on a web server
        public string webFileURL;

        // Base URL where images are hosted (images will be loaded from imageBaseURL/imageName)
        public string imageBaseURL;

        [Header("Load from Local")]
        // Filename of the Twee file in the StreamingAssets folder
        public string localFileName;

        // Parser instance for reading Twee files
        private TweeParser tweeParser;

        // Dictionary storing all passages from the Twee file
        private Dictionary<string, TweeParser.Passage> passages;

        // Title of the currently displayed passage
        private string currentPassageTitle;
        private Passage currentPassage;
        private bool isWriting;

        //Type writer text style
        [Header("Styling text")]
        [Tooltip("The delay every letter will appear, in seconds")] public float delay;
        public TMP_FontAsset _narraFont;
        public TMP_FontAsset _speakFont;
        public Color _narraColor;

        /// <summary>
        /// Initialize the text adventure and start loading the Twee file
        /// </summary>
        void Start()
        {
            tweeParser = new TweeParser();
            if (loadFromWeb)
            {
                StartCoroutine(LoadTweeFile(webFileURL));

            }
            else
            {
                StartCoroutine(LoadTweeFile(Path.Combine(Application.streamingAssetsPath, localFileName)));
            }
        }

        /// <summary>
        /// Called when a choice button is clicked
        /// </summary>
        /// <param name="choiceTitle">The target passage to navigate to</param>
        /// <param name="currentPassageText">The text of the current passage</param>
        void OnChoiceSelected(string choiceTitle, string currentPassageText)
        {
            DisplayPassage(choiceTitle);
            //myChoices += 1;
            //myChoiceCounterUI.text = "Choices made: " + myChoices.ToString();
        }

        /// <summary>
        /// Load and parse the Twee file from either web or local storage
        /// </summary>
        /// <param name="filePath">Path to the Twee file (URL for web, file path for local)</param>
        IEnumerator LoadTweeFile(string filePath)
        {
            // Check if this uses local files or files loaded from web
            if (loadFromWeb)
            {
                // Load from web using UnityWebRequest
                Debug.Log("Starting twee file download from: " + webFileURL);
                UnityWebRequest request = UnityWebRequest.Get(filePath);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    string text = request.downloadHandler.text;
                    passages = tweeParser.ParseTweeFileFromText(text);

                    CheckForStartPassage();
                }
            }
            else
            {
                // Load from local StreamingAssets folder
                if (File.Exists(filePath))
                {
                    string text = File.ReadAllText(filePath, Encoding.UTF8);
                    passages = tweeParser.ParseTweeFileFromText(text);

                    CheckForStartPassage();

                    yield break; // Exit the coroutine since we're using local file loading
                }
                else
                {
                    Debug.LogError("Twee file not found in StreamingAssets: " + filePath);
                    yield break;
                }

            }

        }

        /// <summary>
        /// Load an image from either web or local storage and display it in the UI
        /// </summary>
        /// <param name="imageFileName">Name of the image file to load</param>
        IEnumerator LoadImage(string imageFileName)
        {
            if (imagePrefab == null || imageContainer == null)
            {
                Debug.LogError("ImagePrefab or ImageContainer is not assigned.");
                yield break;
            }

            string imagePath;

            if (loadFromWeb)
            {
                // Load image from web
                imagePath = Path.Combine(imageBaseURL, imageFileName);
                Debug.Log("Starting texture download from: " + imagePath);
                UnityWebRequest request = UnityWebRequestTexture.GetTexture(imagePath);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Download error: " + request.error);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    if (texture == null)
                    {
                        Debug.LogError("Failed to retrieve texture from web.");
                        yield break;
                    }

                    Debug.Log("Texture downloaded. Width: " + texture.width + ", Height: " + texture.height);

                    // Create sprite from texture and display it
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    Image image = Instantiate(imagePrefab, imageContainer);
                    image.sprite = sprite;
                    image.gameObject.SetActive(true);
                }
            }
            else
            {
                // Load image from local StreamingAssets
                imagePath = Path.Combine(Application.streamingAssetsPath, imageFileName);

                if (File.Exists(imagePath))
                {
                    Debug.Log("Loading texture from local file: " + imagePath);
                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                    Texture2D texture = new Texture2D(2, 2); // Texture size will be updated when the image is loaded
                    texture.LoadImage(imageBytes);

                    if (texture == null)
                    {
                        Debug.LogError("Failed to load texture from StreamingAssets.");
                        yield break;
                    }

                    Debug.Log("Texture loaded from StreamingAssets. Width: " + texture.width + ", Height: " + texture.height);

                    DisplayImage(texture, texture.width, texture.height, imageContainer);

            }
                else
                {
                    Debug.LogError("Image file not found in StreamingAssets: " + imagePath);
                }
            }
        }


        /// <summary>
        /// Display a passage and its contents (text, choices, images) in the UI
        /// Supports all Twine link formats: [[Target]], [[Text|Target]], [[Text->Target]]
        /// </summary>
        /// <param name="passageTitle">The title of the passage to display</param>
        public void DisplayPassage(string passageTitle)
        {
            if (!passages.TryGetValue(passageTitle, out var passage))
            {
                Debug.LogError("Passage not found: " + passageTitle);
                return;
            }

            // Clear previous content
            ClearChoices();
            //ClearImages();
            ClearText();

            // Display background image
            if (passage.Images.Count > 0)
            {
                ClearImagesBack();
                CreateImage(passage);
            }
            

            // Display passage text
            currentPassageTitle = passageTitle;


            /// <summary>
            /// Modifications :
            /// - Text will appear letter by letter
            /// - Choices have to appear only when the full text is displayed
            /// - Player can show full text by clicking on space
            /// </summary>

            currentPassage = passage;
            StopAllCoroutines(); // Clear show text
            StartCoroutine(ShowText(passage));

            foreach (var tags in passage.Tags)
            {

                if (tags.Contains("BONUS"))
                {
                    string[] charTags = tags.Split("-");

                    Character character = CharacterManagement.Instance.FindCharacterName(charTags[1]);
                    if (character != null)
                    {
                        CharacterManagement.Instance.GetAndUpdateMeterBYCharacter(character.keyName, CharacterManagement.Instance.bonusLevel);
                    }
                }

                if (tags.Contains("MALUS"))
                {
                    string[] charTags = tags.Split("-");

                    Character character = CharacterManagement.Instance.FindCharacterName(charTags[1]);
                    if (character != null)
                    {
                        CharacterManagement.Instance.GetAndUpdateMeterBYCharacter(character.keyName, CharacterManagement.Instance.bonusLevel * -1);
                    }
                }


                switch (tags)
                {
                    case "END":

                        Application.Quit();

                    break;
                    

                }

            }

            foreach (var character in passage.CharacterEmotions)
            {
                switch (character.CharacterName)
                {
                    case "REMOVE":
                        ClearImagesChar();
                        ClearImagesEye();
                        break;


                     default:
                        if (character.CharacterName != null)
                        {
                            ClearImagesChar();
                            TagCharacterManager(character.CharacterName);
                        }

                        if (character.CharacterEye != null)
                        {
                            ClearImagesEye();
                            Eye eye = CharacterManagement.Instance.FindEyeByName(character.CharacterEye);
                            if (eye != null)
                            {
                                imageEyeContainer.gameObject.SetActive(true);
                                SetImageSize(imageEyeContainer, eye.eyeWidth, eye.eyeHeight);
                                DisplayImage(eye.eyeImage, eye.eyeImage.width, eye.eyeImage.height, imageEyeContainer);

                            }
                        }

                        break;

                }

                
            }

            foreach (var speaker in passage.Speakers)
            {
                passageText.font = _speakFont;

                switch (speaker)
                {
                    case "REMOVE":
                        selfnameContainer.gameObject.SetActive(false);
                        charnameContainer.gameObject.SetActive(false);
                        break;

                    default:
                        Character character = CharacterManagement.Instance.FindCharacterName(speaker);
                        if (character != null)
                        {
                            passageText.color = character.text_color;

                            switch (character.keyName)
                            {
                                case "SELF":

                                    selfnameContainer.gameObject.SetActive(true);
                                    charnameContainer.gameObject.SetActive(false);

                                    selfnameContainer.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = character.charName;
                                    selfnameContainer.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = character.text_color;


                                    break;

                                default:

                                    selfnameContainer.gameObject.SetActive(false);
                                    charnameContainer.gameObject.SetActive(true);

                                    charnameContainer.GetComponentInChildren<TextMeshProUGUI>().text = character.charName;
                                    charnameContainer.GetComponentInChildren<TextMeshProUGUI>().color = character.text_color;


                                    break;

                            }
                        }
                        break;
                }

                
            }


        }

        private void TagCharacterManager(string name)
        {
            Character character = CharacterManagement.Instance.FindCharacterName(name);
            if (character != null)
            {
                switch (character.keyName)
                {
                    case "SELF":

                        ClearImagesChar();
                        ClearImagesEye();

                        passageText.font = _narraFont;
                        passageText.color = _narraColor;


                        selfnameContainer.gameObject.SetActive(false);
                        charnameContainer.gameObject.SetActive(false);

                        imageCharContainer.gameObject.SetActive(false);
                        imageEyeContainer.gameObject.SetActive(false);
                        CharacterManagement.Instance.ToggleMeterContainer(false);

                        break;

                    default:

                        // Character image
                        SetImageSize(imageCharContainer, character.characterWidth, character.characterHeight);
                        DisplayImage(character.characterImage, character.characterImage.width, character.characterImage.height, imageCharContainer);


                        // Show Bro Meter for current character
                        CharacterManagement.Instance.ToggleMeterContainer(true);
                        CharacterManagement.Instance.GetAndUpdateMeterBYCharacter(character.keyName, 0);

                        break;

                }
            }
        }

        private void SetImageSize(Transform container, float width, float height)
        {
            container.gameObject.SetActive(true);
            container.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            container.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        private void DisplayImage(Texture2D texture, int width, int height, Transform container)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.zero);
            Image image = Instantiate(imagePrefab, container);
            image.sprite = sprite;
            image.gameObject.SetActive(true);
        }

        /// <summary>
        /// Check if a "Start" passage exists and display it, or show helpful error messages
        /// </summary>
        void CheckForStartPassage()
        {
            if (passages.ContainsKey("Start"))
            {
                Debug.Log("Passage 'Start' found.");
                DisplayPassage("Start");  // Display the initial passage
            }
            else
            {
                // Show detailed error message with troubleshooting steps
                Debug.LogError("Passage 'Start' not found.");
                Debug.LogError("");
                Debug.LogError("=== HOW TO FIX ===");
                Debug.LogError("1. Make sure your Twee file has a passage named 'Start' (case-sensitive)");
                Debug.LogError("2. Check the StoryData section - the 'start' field should be set to 'Start'");
                Debug.LogError("3. Verify your Twee file format:");
                Debug.LogError("   :: Start {\"position\":\"400,100\",\"size\":\"100,100\"}");
                Debug.LogError("   Your story text here...");
                Debug.LogError("   [[Choice text|Target passage]]");
                Debug.LogError("");
                Debug.LogError("Available passages found in file:");

                if (passages.Count == 0)
                {
                    Debug.LogError("   (No passages found - check if file was loaded correctly)");
                }
                else
                {
                    // List all available passages to help debugging
                    foreach (var passageTitle in passages.Keys)
                    {
                        Debug.LogError($"   - {passageTitle}");
                    }
                }
            }
        }

        // Clear out the button choices to make room for the new ones.
        void ClearChoices()
        {
            foreach (Transform child in choiceButtonContainer)
            {
                Destroy(child.gameObject);
            }

        }

        void ClearImagesBack()
        {
            foreach (Transform child in imageContainer)
            {
                Destroy(child.gameObject);
            }
        }

        void ClearImagesChar()
        {
            foreach (Transform child in imageCharContainer)
            {
                Destroy(child.gameObject);
            }
        }

        void ClearImagesEye()
        {
            foreach (Transform child in imageEyeContainer)
            {
                Destroy(child.gameObject);
            }
        }

        // Clear out the image to make room for the new one.
        void ClearImages()
        {
            foreach (Transform child in imageContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in imageCharContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in imageEyeContainer)
            {
                Destroy(child.gameObject);
            }
        }

        void ClearText()
        {
            isWriting = true;
            CanceledSelect();
            
        }


        private void CreateChoices(Passage passage)
        {
            //foreach (var choice in passage.ParsedChoices)
            //{
            //    var choiceButton = Instantiate(choiceButtonPrefab, choiceButtonContainer);

            //    // Display the choice text on the button
            //    choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;

            //    // When clicked, navigate to the target passage
            //    string targetPassage = choice.Target; // Capture for lambda
            //    choiceButton.onClick.AddListener(() => OnChoiceSelected(targetPassage, passage.Body));
            //}

            for(int i = 0; i < passage.ParsedChoices.Count; i++) 
            {
                var choiceButton = Instantiate(choiceButtonPrefab, choiceButtonContainer);

                // Display the choice text on the button
                choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = passage.ParsedChoices[i].Text;

                // When clicked, navigate to the target passage
                string targetPassage = passage.ParsedChoices[i].Target; // Capture for lambda
                choiceButton.onClick.AddListener(() => OnChoiceSelected(targetPassage, passage.Body));
                
                // Navigation with keyboard
                // Select the first button to continue
                if (i == 0)
                {
                    EventSystem.current.firstSelectedGameObject = choiceButton.gameObject;
                    choiceButton.Select();
                }
            }
        
        }

        private void CreateImage(Passage passage)
        {
            foreach (var imageFileName in passage.Images)
            {
                StartCoroutine(LoadImage(imageFileName));
            }
        }

        IEnumerator ShowText(Passage passage)
        {
            for (int i = 0; i < passage.Body.Length; i++)
            {
                passageText.text = passage.Body.Substring(0, i);
                yield return new WaitForSeconds(delay);
            }

            isWriting = false;

            // The choices needs to appear when the full text is displayed
            CreateChoices(passage);
            
        }

        private InputAction _actionSelect;

        private void Awake()
        {
            _actionSelect = GameObject.FindWithTag("Player").GetComponent<PlayerInput>().actions.FindActionMap("Player").FindAction("Select");

        }

        /// <summary>
        /// PLAYER INPUT ACTIONS
        /// </summary>

        public void onAdvanceSequence(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ShowAllPassageText();

            }


        }

        public void ShowAllPassageText()
        {
            if (isWriting)
            {
                StopAllCoroutines();
                passageText.text = currentPassage.Body;
                
                // The choices needs to appear when the full text is displayed
                CreateChoices(currentPassage);

                EnableSelect();
                

                isWriting = false;

            }
        }

        public void OnSelectChoice(InputAction.CallbackContext context) 
        { 
            if (!isWriting)
            {
                if (context.canceled)
                {
                     if (EventSystem.current.currentSelectedGameObject != null)
                    {
                            Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                            ExecuteEvents.Execute<IPointerClickHandler>(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                    }

                }
            }
        }

        public void CanceledSelect()
        {
            _actionSelect.canceled -= OnSelectChoice;
            _actionSelect.Disable();
        }
        public void EnableSelect()
        {
            _actionSelect.canceled += OnSelectChoice;
            _actionSelect.Enable();
        }
    }
}