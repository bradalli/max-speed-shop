using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.Bradalli.Mastered.CarConfigurator
{
    [CreateAssetMenu(fileName = "Wheels List", menuName = "ScriptableObjects/WheelsListObject", order = 1)]
    public class SO_WheelsList : ScriptableObject
    {
        //Public list to hold numerous types of wheels to be referenced later
        public List<WheelsInfo> wheelsList;
    }

    //Class to hold information of each wheel type
    [System.Serializable]
    public class WheelsInfo
    {
        public string wheelName;
        public int modSpeedValue, modAccelValue, modHandValue, modCoolValue;
        public int wheelCost;
        public GameObject wheelPrefab;
        public Image wheelIcon;
    }
}


