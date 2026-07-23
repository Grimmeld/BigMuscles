using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] private List<Player_Stat> stats;
}
