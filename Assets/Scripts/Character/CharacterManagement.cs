using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterManagement : MonoBehaviour
{
    public static CharacterManagement Instance;

    [Header("Parameters")]
    [Tooltip("Lister tous les personnages de la scène")]
    [SerializeField] private CharacterSO[] characters;
    [HideInInspector] public List<Character> characterList;

    [Header("UI")]
    [SerializeField] Slider meterSlider;
    [SerializeField] float fadeSlider;

    [Header("Debug")]
    public float bonusLevel;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;

        foreach(CharacterSO so in characters)
        {
            // Instantiate the Scriptable Object allows to change data and not change the SO
            CharacterSO s = Instantiate(so);
            AddToList(s.character);
    }

        Character character = FindCharacterName(name);

        if (character != null)
        {
            StartCoroutine(UpdateUI(character.meter));
        }

    }

    private void AddToList(Character c)
    {
        characterList.Add(c);
    }

    public void UpdateMeter(string name, float bonus)
    {
        /// Get the right character
        /// Retrieve the meter level
        /// update the meter
        /// Change UI

        Character character = FindCharacterName(name);

        if (character != null)
        {
            character.meter = UpdaterMeterChar(character.meter, bonus);

            StartCoroutine(UpdateUI(character.meter));
        }


    }

    

    private IEnumerator UpdateUI(float meter)
    {

        if (meterSlider.value < meter) //bonus
        {
            while (meterSlider.value <= meter)
            {
                
                meterSlider.value += Time.deltaTime * fadeSlider;
                yield return null;
            }

        }
        else if (meterSlider.value > meter) //malus
        {

            while (meter <= meterSlider.value)
            {
                
                meterSlider.value -= Time.deltaTime * fadeSlider;
                yield return null;
            }
        }


        meterSlider.value = meter;
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

    public Character FindCharacterName(string nameToCheck)
    {
        foreach (Character character in characterList)
        {
            if (character.keyName == nameToCheck)
            {
                return character;            
            }
        }


        return null;
    }

}
