using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.Bradalli.Mastered.CarConfigurator
{
    [CreateAssetMenu(fileName = "Vehicle List", menuName = "ScriptableObjects/VehicleListObject", order = 1)]
    public class SO_VehicleList : ScriptableObject
    {
        //Public list to hold numerous types of vehicles to be referenced later
        public List<VehicleInfo> vehicleList;
    }

    //Class to hold information of each vehicle
    [System.Serializable]
    public class VehicleInfo
    {
        public string vehicleName;
        public int attSpeedValue, attAccelValue, attHandValue, attCoolValue;
        public int vehicleCost;
        public GameObject vehiclePrefab;
        public Image vehicleIcon;
    }
}


