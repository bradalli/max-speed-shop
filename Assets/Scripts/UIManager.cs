using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace com.Bradalli.Mastered.CarConfigurator
{
    public class UIManager : MonoBehaviour
    {
        public Transform itemTileContainer;
        public Transform parentSelectionPanel, parentBalanceNCost, parentActiveVehicleInfo, parentCompareVehicleInfo;
        TextMeshProUGUI textCostTotAMOUNT, textCostVehTITLE, textCostVehAMOUNT, textCostPaiTITLE, textCostPaiAMOUNT, textCostWheTITLE, textCostWheAMOUNT;
        public Color defaultCategoryButtonColour;
        public TextMeshProUGUI categoryTitle;
        [System.Serializable]
        public enum itemCategory { VEHICLE, PAINT, WHEELS};
        public itemCategory currentCategory = itemCategory.VEHICLE;
        SO_VehicleList refVehList;
        SO_PaintList refPaiList;
        SO_WheelsList refWheList;

        [Header("Active Vehicle Info Panel")]
        [SerializeField] TextMeshProUGUI textInfoVehName;
        [SerializeField] TextMeshProUGUI textInfoPaiNTyre, textInfoSpeed, textInfoAccel, textInfoHandl, textInfoCool;
        [SerializeField] Image fillSpeed, fillAccel, fillHandl, fillCooln;

        bool cursorHoverOverTile;
        int hoverOverTileIndex;

        [Header("Compare Vehicle Info Panel")]
        [SerializeField] TextMeshProUGUI textCompVehName;
        [SerializeField] TextMeshProUGUI textCompPaiNTyre, textCompSpeed, textCompAccel, textCompHandl, textCompCool;
        [SerializeField] Image fillCompSpeed1, fillCompSpeed2, fillCompAccel1, fillCompAccel2, fillCompHandl1, fillCompHandl2, fillCompCooln1, fillCompCooln2;

        private void Start()
        {
            #region reference text components of costs UI
            Transform parentCosts = parentBalanceNCost.Find("Parent_Costs");
            textCostTotAMOUNT = parentCosts.Find("Text_CostTotalAMOUNT").GetComponent<TextMeshProUGUI>();
            textCostVehTITLE = parentCosts.Find("Text_CostVehicleTITLE").GetComponent<TextMeshProUGUI>();
            textCostVehAMOUNT = parentCosts.Find("Text_CostVehicleAMOUNT").GetComponent<TextMeshProUGUI>();
            textCostPaiTITLE = parentCosts.Find("Text_CostPaintTITLE").GetComponent<TextMeshProUGUI>();
            textCostPaiAMOUNT = parentCosts.Find("Text_CostPaintAMOUNT").GetComponent<TextMeshProUGUI>();
            textCostWheTITLE = parentCosts.Find("Text_CostWheelTITLE").GetComponent<TextMeshProUGUI>();
            textCostWheAMOUNT = parentCosts.Find("Text_CostWheelAMOUNT").GetComponent<TextMeshProUGUI>();
            #endregion

            #region reference scriptable objects from game manager
            refVehList = GameManager.gMang.vehListObj;
            refPaiList = GameManager.gMang.paiListObj;
            refWheList = GameManager.gMang.wheListObj;
            #endregion

            ChangeCategory(0);
            UpdateCost();
            UpdateAttributes();
            parentCompareVehicleInfo.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (cursorHoverOverTile)
            {
                parentCompareVehicleInfo.position = Input.mousePosition;
            }
        }

        void UpdateCost()
        {
            //Vehicle
            textCostVehTITLE.text = refVehList.vehicleList[GameManager.gMang.selectedVehicleIndex].vehicleName;
            int vehCost = refVehList.vehicleList[GameManager.gMang.selectedVehicleIndex].vehicleCost;
            textCostVehAMOUNT.text = "-" + vehCost.ToString("n0") + "cr";
            //Paint
            textCostPaiTITLE.text = refPaiList.paintList[GameManager.gMang.selectedPaintIndex].paintName;
            int paiCost = refPaiList.paintList[GameManager.gMang.selectedPaintIndex].paintCost;
            textCostPaiAMOUNT.text = "-" + paiCost.ToString("n0") + "cr";
            //Wheels
            textCostWheTITLE.text = refWheList.wheelsList[GameManager.gMang.selectedWheelIndex].wheelName;
            int wheCost = refWheList.wheelsList[GameManager.gMang.selectedWheelIndex].wheelCost;
            textCostWheAMOUNT.text = "-" + wheCost.ToString("n0") + "cr";

            //TOTAL
            textCostTotAMOUNT.text = "-" + (vehCost + paiCost + wheCost).ToString("n0") + "cr";


        }

        void UpdateAttributes()
        {
            int vehIndex = GameManager.gMang.selectedVehicleIndex;
            int paiIndex = GameManager.gMang.selectedPaintIndex;
            int wheIndex = GameManager.gMang.selectedWheelIndex;
            textInfoVehName.text = refVehList.vehicleList[vehIndex].vehicleName;
            textInfoPaiNTyre.text = refPaiList.paintList[paiIndex].paintName + " + " + refWheList.wheelsList[wheIndex].wheelName;

            #region add up attribute scores from each selected item
            int speedScore = refVehList.vehicleList[vehIndex].attSpeedValue + refPaiList.paintList[paiIndex].modSpeedValue + refWheList.wheelsList[wheIndex].modSpeedValue;
            int accelScore = refVehList.vehicleList[vehIndex].attAccelValue + refPaiList.paintList[paiIndex].modAccelValue + refWheList.wheelsList[wheIndex].modAccelValue;
            int handlScore = refVehList.vehicleList[vehIndex].attHandValue + refPaiList.paintList[paiIndex].modHandValue + refWheList.wheelsList[wheIndex].modHandValue;
            int coolnScore = refVehList.vehicleList[vehIndex].attCoolValue + refPaiList.paintList[paiIndex].modCoolValue + refWheList.wheelsList[wheIndex].modCoolValue;
            #endregion

            //Speed
            textInfoSpeed.text = "(" + speedScore.ToString() + "/150)";
            fillSpeed.fillAmount = (float)speedScore / 150;

            //Acceleration
            textInfoAccel.text = "(" + accelScore.ToString() + "/150)";
            fillAccel.fillAmount = (float)accelScore / 150;

            //Handling
            textInfoHandl.text = "(" + handlScore.ToString() + "/150)";
            fillHandl.fillAmount = (float)handlScore / 150;

            //Coolness
            textInfoCool.text = "(" + coolnScore.ToString() + "/150)";
            fillCooln.fillAmount = (float)coolnScore / 150;
        }

        public void ChangeActiveItem(int index)
        {
            //Set the item index according to the current category
            switch (currentCategory)
            {
                case itemCategory.VEHICLE:
                    //Disable old selected border
                    SetSELECTEDBorderVisibility(GameManager.gMang.selectedVehicleIndex, false);
                    //Enable new selected border
                    SetSELECTEDBorderVisibility(index, true);
                    //Update selected index in gm
                    GameManager.gMang.selectedVehicleIndex = index;
                    break;

                case itemCategory.PAINT:
                    //Disable old selected border
                    SetSELECTEDBorderVisibility(GameManager.gMang.selectedPaintIndex, false);
                    //Enable new selected border
                    SetSELECTEDBorderVisibility(index, true);
                    //Update selected index in gm
                    GameManager.gMang.selectedPaintIndex = index;
                    break;

                case itemCategory.WHEELS:
                    //Disable old selected border
                    SetSELECTEDBorderVisibility(GameManager.gMang.selectedWheelIndex, false);
                    //Enable new selected border
                    SetSELECTEDBorderVisibility(index, true);
                    //Update selected index in gm
                    GameManager.gMang.selectedWheelIndex = index;
                    break;
            }

            GameManager.gMang.viewMang.RefreshView();
            UpdateCost();
            UpdateAttributes();
            CursorOnItemTile_Exit();
        }

        void SetSELECTEDBorderVisibility(int tileIndex, bool state)
        {
            itemTileContainer.GetChild(tileIndex).Find("Tile_SELECTEDBorder").gameObject.SetActive(state);
        }

        public void ChangeCategory(int newCategoryIndex)
        {
            itemCategory newCategory = (itemCategory)newCategoryIndex;

            //Set old category button to default colours
            Transform button = parentSelectionPanel.Find("Button_Category" + currentCategory.ToString());
            button.GetComponent<Image>().color = defaultCategoryButtonColour;
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

            //Set new category button to SELECT colours
            button = parentSelectionPanel.Find("Button_Category" + newCategory.ToString());
            button.GetComponent<Image>().color = Color.white;
            button.GetComponentInChildren<TextMeshProUGUI>().color = defaultCategoryButtonColour;

            currentCategory = newCategory;
            categoryTitle.text = "SELECT " + newCategory.ToString();

            RefreshTiles();
        }

        //Refresh tiles when the category changes
        public void RefreshTiles()
        {
            int listCount = RetrieveListCount();
            HideTiles();
            for(int i = 0; i < listCount; i++)
            {
                GameObject tile = itemTileContainer.GetChild(i).gameObject;

                //Activate if this tile is the current selected item
                tile.transform.Find("Tile_SELECTEDBorder").gameObject.SetActive(ItemTileSelectStatus(i));

                tile.SetActive(true);
                tile.transform.Find("Tile_NameText").GetComponent<TextMeshProUGUI>().text = RetrieveItemName(i);
                tile.transform.Find("Tile_PriceText").GetComponent<TextMeshProUGUI>().text = RetrieveItemPrice(i);
            }
        }

        //Retrieve number of items in the list of the current category
        int RetrieveListCount()
        {
            int count = 0;

            switch (currentCategory)
            {
                case itemCategory.VEHICLE:
                    count = refVehList.vehicleList.Count;
                    break;

                case itemCategory.PAINT:
                    count = refPaiList.paintList.Count;
                    break;

                case itemCategory.WHEELS:
                    count = refWheList.wheelsList.Count;
                    break;
            }

            return count;
        }

        //Retrieve the item name in a category according to an index
        string RetrieveItemName(int index)
        {
            string retrievedItemName = string.Empty;

            switch (currentCategory)
            {
                case itemCategory.VEHICLE:
                    retrievedItemName = refVehList.vehicleList[index].vehicleName;
                    break;

                case itemCategory.PAINT:
                    retrievedItemName = refPaiList.paintList[index].paintName;
                    break;

                case itemCategory.WHEELS:
                    retrievedItemName = refWheList.wheelsList[index].wheelName;
                    break;
            }

            return retrievedItemName;
        }

        string RetrieveItemPrice(int index)
        {
            string retrievedItemPrice = string.Empty;

            switch (currentCategory)
            {
                case itemCategory.VEHICLE:
                    if (refVehList.vehicleList[index].vehicleCost > 0) 
                    {
                        retrievedItemPrice = refVehList.vehicleList[index].vehicleCost.ToString("n0") + "cr"; 
                    }
                    break;

                case itemCategory.PAINT:
                    if (refPaiList.paintList[index].paintCost > 0)
                    {
                        retrievedItemPrice = refPaiList.paintList[index].paintCost.ToString("n0") + "cr";
                    }
                    break;

                case itemCategory.WHEELS:
                    if (refWheList.wheelsList[index].wheelCost > 0)
                    {
                        retrievedItemPrice = refWheList.wheelsList[index].wheelCost.ToString("n0") + "cr";
                    }
                    break;
            }

            return retrievedItemPrice;
        }

        //Determine whether the player has this index currently selected according to the category
        bool ItemTileSelectStatus(int index)
        {
            bool isSelected = false;

            switch (currentCategory)
            {
                case itemCategory.VEHICLE:
                    isSelected = GameManager.gMang.selectedVehicleIndex == index;
                    break;

                case itemCategory.PAINT:
                    isSelected = GameManager.gMang.selectedPaintIndex == index;
                    break;

                case itemCategory.WHEELS:
                    isSelected = GameManager.gMang.selectedWheelIndex == index;
                    break;
            }

            return isSelected;
        }

        public void CursorOnItemTile_Enter(int tileIndex)
        {
            #region
            int vehIndex = GameManager.gMang.selectedVehicleIndex;
            int paiIndex = GameManager.gMang.selectedPaintIndex;
            int wheIndex = GameManager.gMang.selectedWheelIndex;
            int speedScore = 0;
            int accelScore = 0;
            int handlScore = 0;
            int coolnScore = 0;
            #endregion

            hoverOverTileIndex = tileIndex;
            cursorHoverOverTile = true;
            parentCompareVehicleInfo.gameObject.SetActive(true);

            #region Set text
            switch (currentCategory)
            {
                case itemCategory.VEHICLE:
                    textCompVehName.text = refVehList.vehicleList[tileIndex].vehicleName;

                    textCompPaiNTyre.text = 
                        refPaiList.paintList[GameManager.gMang.selectedPaintIndex].paintName + " + " 
                        + refWheList.wheelsList[GameManager.gMang.selectedWheelIndex].wheelName;

                    #region add up attribute scores from each selected item
                    speedScore = refVehList.vehicleList[tileIndex].attSpeedValue + refPaiList.paintList[paiIndex].modSpeedValue + refWheList.wheelsList[wheIndex].modSpeedValue;
                    accelScore = refVehList.vehicleList[tileIndex].attAccelValue + refPaiList.paintList[paiIndex].modAccelValue + refWheList.wheelsList[wheIndex].modAccelValue;
                    handlScore = refVehList.vehicleList[tileIndex].attHandValue + refPaiList.paintList[paiIndex].modHandValue + refWheList.wheelsList[wheIndex].modHandValue;
                    coolnScore = refVehList.vehicleList[tileIndex].attCoolValue + refPaiList.paintList[paiIndex].modCoolValue + refWheList.wheelsList[wheIndex].modCoolValue;
                    #endregion

                    #region fill bars
                    //Speed
                    textCompSpeed.text = "(" + speedScore.ToString() + "/150)";
                    float normCompSpeedScore = (float)speedScore / 150;
                    fillCompSpeed1.fillAmount = Mathf.Max(normCompSpeedScore, fillSpeed.fillAmount);
                    fillCompSpeed2.fillAmount = Mathf.Min(normCompSpeedScore, fillSpeed.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompSpeedScore - fillSpeed.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompSpeed1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompSpeed1.color = Color.red;
                    }
                    #endregion

                    //Acceleration
                    textCompAccel.text = "(" + accelScore.ToString() + "/150)";
                    float normCompAccelScore = (float)accelScore / 150;
                    fillCompAccel1.fillAmount = Mathf.Max(normCompAccelScore, fillAccel.fillAmount);
                    fillCompAccel2.fillAmount = Mathf.Min(normCompAccelScore, fillAccel.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompAccelScore - fillAccel.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompAccel1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompAccel1.color = Color.red;
                    }
                    #endregion

                    //Handling
                    textCompHandl.text = "(" + handlScore.ToString() + "/150)";
                    float normCompHandlScore = (float)handlScore / 150;
                    fillCompHandl1.fillAmount = Mathf.Max(normCompHandlScore, fillHandl.fillAmount);
                    fillCompHandl2.fillAmount = Mathf.Min(normCompHandlScore, fillHandl.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompHandlScore - fillHandl.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompHandl1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompHandl1.color = Color.red;
                    }
                    #endregion

                    //Coolness
                    textInfoCool.text = "(" + coolnScore.ToString() + "/150)";
                    float normCompCoolnScore = (float)coolnScore / 150;
                    fillCompCooln1.fillAmount = Mathf.Max(normCompCoolnScore, fillCooln.fillAmount);
                    fillCompCooln2.fillAmount = Mathf.Min(normCompCoolnScore, fillCooln.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompCoolnScore - fillCooln.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompCooln1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompCooln1.color = Color.red;
                    }
                    #endregion
                    #endregion region
                    break;

                case itemCategory.PAINT:
                    #region add up attribute scores from each selected item
                    speedScore = refVehList.vehicleList[vehIndex].attSpeedValue + refPaiList.paintList[tileIndex].modSpeedValue + refWheList.wheelsList[wheIndex].modSpeedValue;
                    accelScore = refVehList.vehicleList[vehIndex].attAccelValue + refPaiList.paintList[tileIndex].modAccelValue + refWheList.wheelsList[wheIndex].modAccelValue;
                    handlScore = refVehList.vehicleList[vehIndex].attHandValue + refPaiList.paintList[tileIndex].modHandValue + refWheList.wheelsList[wheIndex].modHandValue;
                    coolnScore = refVehList.vehicleList[vehIndex].attCoolValue + refPaiList.paintList[tileIndex].modCoolValue + refWheList.wheelsList[wheIndex].modCoolValue;
                    #endregion

                    #region fill bars
                    //Speed
                    textCompSpeed.text = "(" + speedScore.ToString() + "/150)";
                    normCompSpeedScore = (float)speedScore / 150;
                    fillCompSpeed1.fillAmount = Mathf.Max(normCompSpeedScore, fillSpeed.fillAmount);
                    fillCompSpeed2.fillAmount = Mathf.Min(normCompSpeedScore, fillSpeed.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompSpeedScore - fillSpeed.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompSpeed1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompSpeed1.color = Color.red;
                    }
                    #endregion

                    //Acceleration
                    textCompAccel.text = "(" + accelScore.ToString() + "/150)";
                    normCompAccelScore = (float)accelScore / 150;
                    fillCompAccel1.fillAmount = Mathf.Max(normCompAccelScore, fillAccel.fillAmount);
                    fillCompAccel2.fillAmount = Mathf.Min(normCompAccelScore, fillAccel.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompAccelScore - fillAccel.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompAccel1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompAccel1.color = Color.red;
                    }
                    #endregion

                    //Handling
                    textCompHandl.text = "(" + handlScore.ToString() + "/150)";
                    normCompHandlScore = (float)handlScore / 150;
                    fillCompHandl1.fillAmount = Mathf.Max(normCompHandlScore, fillHandl.fillAmount);
                    fillCompHandl2.fillAmount = Mathf.Min(normCompHandlScore, fillHandl.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompHandlScore - fillHandl.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompHandl1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompHandl1.color = Color.red;
                    }
                    #endregion

                    //Coolness
                    textInfoCool.text = "(" + coolnScore.ToString() + "/150)";
                    normCompCoolnScore = (float)coolnScore / 150;
                    fillCompCooln1.fillAmount = Mathf.Max(normCompCoolnScore, fillCooln.fillAmount);
                    fillCompCooln2.fillAmount = Mathf.Min(normCompCoolnScore, fillCooln.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompCoolnScore - fillCooln.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompCooln1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompCooln1.color = Color.red;
                    }
                    #endregion
                    #endregion region
                    break;

                case itemCategory.WHEELS:
                    #region add up attribute scores from each selected item
                    speedScore = refVehList.vehicleList[vehIndex].attSpeedValue + refPaiList.paintList[paiIndex].modSpeedValue + refWheList.wheelsList[tileIndex].modSpeedValue;
                    accelScore = refVehList.vehicleList[vehIndex].attAccelValue + refPaiList.paintList[paiIndex].modAccelValue + refWheList.wheelsList[tileIndex].modAccelValue;
                    handlScore = refVehList.vehicleList[vehIndex].attHandValue + refPaiList.paintList[paiIndex].modHandValue + refWheList.wheelsList[tileIndex].modHandValue;
                    coolnScore = refVehList.vehicleList[vehIndex].attCoolValue + refPaiList.paintList[paiIndex].modCoolValue + refWheList.wheelsList[tileIndex].modCoolValue;
                    #endregion

                    #region fill bars
                    //Speed
                    textCompSpeed.text = "(" + speedScore.ToString() + "/150)";
                    normCompSpeedScore = (float)speedScore / 150;
                    fillCompSpeed1.fillAmount = Mathf.Max(normCompSpeedScore, fillSpeed.fillAmount);
                    fillCompSpeed2.fillAmount = Mathf.Min(normCompSpeedScore, fillSpeed.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompSpeedScore - fillSpeed.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompSpeed1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompSpeed1.color = Color.red;
                    }
                    #endregion

                    //Acceleration
                    textCompAccel.text = "(" + accelScore.ToString() + "/150)";
                    normCompAccelScore = (float)accelScore / 150;
                    fillCompAccel1.fillAmount = Mathf.Max(normCompAccelScore, fillAccel.fillAmount);
                    fillCompAccel2.fillAmount = Mathf.Min(normCompAccelScore, fillAccel.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompAccelScore - fillAccel.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompAccel1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompAccel1.color = Color.red;
                    }
                    #endregion

                    //Handling
                    textCompHandl.text = "(" + handlScore.ToString() + "/150)";
                    normCompHandlScore = (float)handlScore / 150;
                    fillCompHandl1.fillAmount = Mathf.Max(normCompHandlScore, fillHandl.fillAmount);
                    fillCompHandl2.fillAmount = Mathf.Min(normCompHandlScore, fillHandl.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompHandlScore - fillHandl.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompHandl1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompHandl1.color = Color.red;
                    }
                    #endregion

                    //Coolness
                    textInfoCool.text = "(" + coolnScore.ToString() + "/150)";
                    normCompCoolnScore = (float)coolnScore / 150;
                    fillCompCooln1.fillAmount = Mathf.Max(normCompCoolnScore, fillCooln.fillAmount);
                    fillCompCooln2.fillAmount = Mathf.Min(normCompCoolnScore, fillCooln.fillAmount);
                    #region fill bar - is comp higher or lower than base
                    if (Mathf.Sign(normCompCoolnScore - fillCooln.fillAmount) > 0)
                    {
                        //Comp is higher than base
                        fillCompCooln1.color = Color.green;
                    }

                    else
                    {
                        //Comp is lower than base
                        fillCompCooln1.color = Color.red;
                    }
                    #endregion
                    #endregion region
                    break;
            }
            #endregion
        }

        public void CursorOnItemTile_Exit()
        {
            cursorHoverOverTile = false;
            parentCompareVehicleInfo.gameObject.SetActive(false);
        }

        //Hide tiles before refreshing
        void HideTiles()
        {
            foreach(Transform child in itemTileContainer)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
