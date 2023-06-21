using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.Bradalli.Mastered.CarConfigurator
{
    [CreateAssetMenu(fileName = "Paint List", menuName = "ScriptableObjects/PaintListObject", order = 1)]
    public class SO_PaintList : ScriptableObject
    {
        //Public list to hold numerous types of paints to be referenced later
        public List<PaintInfo> paintList;
    }

    //Class to hold information of each paint
    [System.Serializable]
    public class PaintInfo
    {
        public string paintName;
        public int modSpeedValue, modAccelValue, modHandValue, modCoolValue;
        public int paintCost;
        public Material paintMaterial;
        public Image paintIcon;
    }
}


