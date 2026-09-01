using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    [System.Serializable]
    public class SummaryCompetence
    {
        public string NameKey;
        public float Value;
        public int GroupNb;
    }

    [SerializeField] private bool notShowingDetail;

    [Header("Valeurs résumé")]
    [SerializeField] private SummaryCompetence[] summaryCompetences;

    [SerializeField] private float sumGroup1;
    [SerializeField] private float sumGroup2;

    [Header("game object")]
    [SerializeField] private CanvasGroup group1;
    [SerializeField] private CanvasGroup group2;
    [SerializeField] private CanvasGroup broFav;
    [SerializeField] private Transform title;
    [SerializeField] private Transform billContainer;
    [SerializeField] private Transform seanContainer;

    [Header("delay animation")]
    [SerializeField] private float delayStart;
    [SerializeField] float fadeSlider;
    [SerializeField] float fadeGroup;
    [SerializeField] float delayStartSlider;

    private void Awake()
    {
        if (group1 != null)
        {
            group1.alpha = 0f;
            group1.transform.localPosition = new Vector2(Screen.width, 79);

        }

        if (group2 != null)
        {
            group2.alpha = 0f;
            group2.transform.localPosition = new Vector2(Screen.width, -70);

        }

        if (broFav != null)
            broFav.transform.localPosition = new Vector2(Screen.width, -338);


    }

    private void OnEnable()
    {
        if (notShowingDetail)
        {
            return;
        }

        //calculate value slide

        group1.alpha = 0f;
        group2.alpha = 0f;

        foreach (SummaryCompetence summaryCompetence in summaryCompetences)
        {
            if (PlayerManager.Instance.isConditionChecked(summaryCompetence.NameKey))
            {
                /// ajouter la valeur dans la bonne variable

                switch (summaryCompetence.GroupNb)
                {
                    case 1:
                        sumGroup1 += summaryCompetence.Value;

                        break;

                    case 2:
                        sumGroup2 += summaryCompetence.Value;

                        break;

                }



            }
        }

        //AddMeterToSum("BILL", sumGroup1);
        //AddMeterToSum("SEAN", sumGroup1);


        // change bro favorite
        string fav = CharacterManagement.Instance.HighestBro();
        if (fav != null)
        {
            switch (fav)
            {
                case "BILL":
                    billContainer.gameObject.SetActive(true);
                    break;

                case "SEAN":
                    seanContainer.gameObject.SetActive(true);
                    break;
            }
        }


        //Start animation
        Invoke("StartAnimation", delayStart);
    }

    private void AddMeterToSum(string charName, float groupValue)
    {
        Character character = CharacterManagement.Instance.FindCharacterName(charName);
        
        if (character != null)
        {
            Debug.Log(character.meter);

            switch (charName)
            {
                case "BILL":
                    groupValue -= (character.meter / 2) ;
                    break;

                case "SEAN":
                    groupValue += (character.meter / 2);
                    break;
            }

        }
        
        
        
    }

    private void StartAnimation()
    {
        title.localPosition = new Vector2(Screen.width, 320);
        title.transform.LeanMoveLocalX(-555, 1f).setEaseInBack();

        group1.LeanAlpha(1, fadeGroup);
        group1.gameObject.LeanMoveLocalX(-283.5f, 1f).delay = 1f;
        Invoke("SliderAnimationGrp1", delayStartSlider);

        group2.LeanAlpha(1, fadeGroup);
        group2.gameObject.LeanMoveLocalX(-283.5f, 1f).delay = 2f; ;
        Invoke("SliderAnimationGrp2", delayStartSlider);

        broFav.LeanAlpha(1, fadeGroup);
        broFav.gameObject.LeanMoveLocalX(-348.75f, 1f).delay = 5f; ;

    }

    private void SliderAnimationGrp1()
    {
        StartCoroutine(AnimateSlider(group1.gameObject, sumGroup1, fadeSlider));

    }

    private void SliderAnimationGrp2()
    {
        StartCoroutine(AnimateSlider(group2.gameObject, sumGroup2, fadeSlider));
    }



    private IEnumerator AnimateSlider(GameObject group, float targetValue, float duration)
    {
        Slider slider = group.GetComponentInChildren<Slider>();

        float startValue = slider.value;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            slider.value = Mathf.Lerp(
                startValue,
                targetValue,
                elapsed / duration
                );

            yield return null;
        }

        slider.value = targetValue;


    }

    private void FadeGroup2()
    {
        
    }
}
