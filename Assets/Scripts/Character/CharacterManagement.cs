using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class CharacterManagement : MonoBehaviour
{
    public Character[] charactersSO;
    public List<Character> characters;
    public TextMeshProUGUI meterTest;
    public float bonusLevel;

    private void Start()
    {
        characters = Resources.LoadAll<Character>("Character/");
        //characters = charactersSO;

        foreach (Character character in characters)
        {
            if (character.name == "BILL")
            {
                UpdateUI(character.meter);
            }
        }
    }


    public void UpdateMeter(string name, float bonus)
    {
        /// Get the right character
        /// Retrieve the meter level
        /// update the meter
        /// Change UI


        foreach(Character character in characters)
        {

            if (character.name == name)
            {
                character.meter = UpdaterMeterChar(character.meter, bonus);

                UpdateUI(character.meter);
            }
        }

    }

    public void UpdateUI(float meter)
    {
        meterTest.text = new string("Meter : " + meter);
    }

    public float UpdaterMeterChar(float currentMeter, float bonus)
    {
        currentMeter += bonus;
        currentMeter = Mathf.Clamp(currentMeter, -100, 100);

        // Mettre un event d'update UI

        // Change in UI if meter is above 100

        return currentMeter;

    }

    public void DebuggerKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UpdateMeter("BILL", bonusLevel);
        }
    }

}
