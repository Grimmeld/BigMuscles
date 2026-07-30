using System.Collections.Generic;
using UnityEngine;
using static SimpleTwineDialogue.TweeParser;

public class PlayerManager : MonoBehaviour
{
    [System.Serializable]
    public class Player_Stat
    {
        public statType type;
        public int hit;
    }

    public enum statType
    {
        candid,
        rebel, 
        selfcentered
    }

    public static PlayerManager Instance;

    [SerializeField] private List<Player_Stat> stats;
    [SerializeField] private List<string> choicesMade;


    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    public void AddKeyChoices(string passageTitle)
    {
        choicesMade.Add(passageTitle);
    }

    public bool isConditionChecked(string passageTitle)
    {
        foreach (string choiceMade in choicesMade)
        {

            if (choiceMade == passageTitle)
            {
                return true;
            }
        }

        return false;
    }
}
