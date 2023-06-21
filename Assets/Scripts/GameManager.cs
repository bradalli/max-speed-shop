using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Bradalli.Mastered.CarConfigurator
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gMang;
        public UIManager uiMang;
        public VehicleViewManager viewMang;
        public SO_VehicleList vehListObj;
        public SO_PaintList paiListObj;
        public SO_WheelsList wheListObj;
        public int selectedVehicleIndex = 0, selectedPaintIndex = 0, selectedWheelIndex = 0;
        public int playerCreditsBalance = 100000;

        private void Awake()
        {
            gMang = this;
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}

