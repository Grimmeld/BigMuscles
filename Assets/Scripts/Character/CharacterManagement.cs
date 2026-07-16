using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// In this script, the characters statistics will be managed like the Bro Meter
/// The meter is updated by character
/// 
/// All characters SO including SELF needs to be added to the Parameters
/// 
/// For now the bonus/malus is always the same amount
/// </summary>

public class CharacterManagement : MonoBehaviour
{
    public static CharacterManagement Instance;

    [Header("Parameters")]
    [Tooltip("Lister tous les personnages de la scène")]
    [SerializeField] private CharacterSO[] characters;
    [SerializeField] private Eye[] eyes;
    [HideInInspector] public List<Character> characterList;

    [Header("UI")]
    public Transform meterContainer;
    [SerializeField] float fadeSlider;

    [Header("Debug")]
    public float bonusLevel;

    public Action<float, float> OnValueChanged;

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

        OnValueChanged += UpdateUI;

    }

    private void UpdateUI(float NewMeter, float bonus)
    {
        StopAllCoroutines();
        //StartCoroutine(ChangeMeterContainer(NewMeter));
        StartCoroutine(AnimateSlider(NewMeter, fadeSlider));
    }

    private void AddToList(Character c)
    {
        characterList.Add(c);
    }

    public void GetAndUpdateMeterBYCharacter(string name, float bonus)
    {
        /// Get the right character
        /// Retrieve the meter level
        /// update the meter
        /// Change UI

        Character character = FindCharacterName(name);

        if (character != null)
        {
            character.meter += bonus;
            character.meter = Mathf.Clamp(character.meter, -100, 100);

            
            OnValueChanged.Invoke(character.meter, bonus);

        }


    }


    private IEnumerator ChangeMeterContainer(float meter)
    {
        meterContainer.GetComponentInChildren<TextMeshProUGUI>().text = (meter.ToString() + " %");

        Slider slider = meterContainer.GetComponentInChildren<Slider>();
        if (slider.value < meter) //bonus
        {
            while (slider.value <= meter)
            {

                slider.value += Time.deltaTime * fadeSlider;
                yield return null;
            }

        }
        else if (slider.value > meter) //malus
        {

            while (meter <= slider.value)
            {

                slider.value -= Time.deltaTime * fadeSlider;
                yield return null;
            }
        }


        slider.value = meter;

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



    public void ToggleMeterContainer(bool state)
    {
        meterContainer.gameObject.SetActive(state);

    }
    public Eye FindEyeByName(string nameToCheck)
    {
        foreach (Eye eye in eyes)
        {
            if (eye.keyName == nameToCheck)
            {
                return eye;
            }
        }


        return null;
    }

    private IEnumerator AnimateSlider(float targetValue, float duration)
    {
        meterContainer.GetComponentInChildren<TextMeshProUGUI>().text = (targetValue.ToString() + " %");

        Slider slider = meterContainer.GetComponentInChildren<Slider>();

        float startValue = slider.value;
        float elapsed = 0f;
        while (elapsed<duration)
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
}
