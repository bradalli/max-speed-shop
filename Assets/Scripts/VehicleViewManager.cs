using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Bradalli.Mastered.CarConfigurator
{
    public class VehicleViewManager : MonoBehaviour
    {
        [SerializeField] float rotateSpeed;
        [SerializeField] int displayedVehicle, displayedPaint, displayedWheel;
        [SerializeField] int currVehIndex, currPaiIndex, currWheIndex;
        [SerializeField] GameObject[] vehiclePool, wheelPool;
        [SerializeField] Material[] paintMaterials;

        Vector3 poolPosition = new Vector3(0, -10, 0);
        bool vehChanged;
        

        void Start()
        {
            #region Pool vehicles, wheels, and paint
            //Pool vehicle game objects
            SO_VehicleList vList = GameManager.gMang.vehListObj;
            int vehListCapacity = vList.vehicleList.Capacity;
            vehiclePool = new GameObject[vehListCapacity];
            for (int i = 0; i < vehListCapacity; i++)
            {
                if (vList.vehicleList[i].vehiclePrefab != null)
                    vehiclePool[i] = Instantiate(vList.vehicleList[i].vehiclePrefab, poolPosition, Quaternion.identity, transform);

            }

            //Pool wheel game objects
            SO_WheelsList wList = GameManager.gMang.wheListObj;
            int wheListCapacity = wList.wheelsList.Capacity;
            wheelPool = new GameObject[wheListCapacity];
            for (int i = 0; i < wheListCapacity; i++)
            {
                if (wList.wheelsList[i].wheelPrefab != null)
                    wheelPool[i] = Instantiate(wList.wheelsList[i].wheelPrefab, poolPosition, Quaternion.identity, transform);

            }

            //Reference paint list
            SO_PaintList pList = GameManager.gMang.paiListObj;
            int paiListCapacity = pList.paintList.Capacity;
            paintMaterials = new Material[paiListCapacity];
            for (int i = 0; i < paiListCapacity; i++)
            {
                if (pList.paintList[i].paintMaterial != null)
                    paintMaterials[i] = pList.paintList[i].paintMaterial;

            }
            #endregion

            RefreshView();
        }

        float lastMousePosX;
        private void FixedUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                float mouseVelX = -(Input.mousePosition.x - lastMousePosX);
                transform.Rotate(0, mouseVelX * rotateSpeed * Time.deltaTime, 0);

                lastMousePosX = Input.mousePosition.x;
            }

            else
            {
                lastMousePosX = Input.mousePosition.x;
            }
        }

        //View is refreshed every time a selected item is changed via UI Manager
        public void RefreshView()
        {
            //Reference selected indices from Game Manager
            currVehIndex = GameManager.gMang.selectedVehicleIndex;
            currPaiIndex = GameManager.gMang.selectedPaintIndex;
            currWheIndex = GameManager.gMang.selectedWheelIndex;

            #region Refresh Vehicle
            if (currVehIndex != displayedVehicle)
            {
                vehChanged = true;

                //Move old vehicle
                if(displayedVehicle > 0)
                    vehiclePool[displayedVehicle].transform.position = poolPosition;

                //Replace with new vehicle
                if (currVehIndex > 0)
                    vehiclePool[currVehIndex].transform.position = Vector3.zero;

                //Set new displayed vehicle int
                displayedVehicle = currVehIndex;
            }
            #endregion

            #region Refresh Wheels
            if (currWheIndex != displayedWheel ^ vehChanged)
            {
                //Move old wheels
                if (displayedWheel > 0)
                {
                    wheelPool[displayedWheel].transform.position = poolPosition;
                }

                //Replace with new wheel
                if (currWheIndex > 0)
                {
                    wheelPool[currWheIndex].transform.position = Vector3.zero;
                }

                //Set new displayed vehicle int
                displayedWheel = currWheIndex;

                //Adjust wheels and chassis
                AdjustWheelsAndChassis();
            }
            #endregion

            #region Refresh Paint
            //Only change paint material if a vehicle is selected, a vehicle type has changed, or the paint type has changed
            if (vehChanged ^ currPaiIndex != displayedPaint)
            {
                //Reference target vehicle materials
                if(displayedVehicle > 0)
                {
                    MeshRenderer targetVehicleMRend = vehiclePool[displayedVehicle].GetComponent<MeshRenderer>();
                    Material[] vehMats = targetVehicleMRend.materials;

                    //Find correct material index
                    int paintMatIndex = 0;
                    for (int i = 0; i < vehMats.Length; i++)
                    {
                        Material mat = vehMats[i];
                        if (mat.name.Contains("Paint_"))
                            paintMatIndex = i;
                    }

                    //Assign new paint material
                    vehMats[paintMatIndex] = paintMaterials[currPaiIndex];
                    targetVehicleMRend.materials = vehMats;
                }
                displayedPaint = currPaiIndex;
            }
            #endregion

            vehChanged = false;
        }

        void AdjustWheelsAndChassis()
        {

            #region Reference individual wheels and their connection points
            Transform wheelFL = null;
            Transform wheelFR = null;
            Transform wheelBL = null;
            Transform wheelBR = null;
            Transform connFL = null;
            Transform connFR = null;
            Transform connBL = null;
            Transform connBR = null;
            if (displayedWheel > 0)
            {
                //Wheels
                wheelFL = wheelPool[currWheIndex].transform.Find("Wheel_FL");
                wheelFR = wheelPool[currWheIndex].transform.Find("Wheel_FR");
                wheelBL = wheelPool[currWheIndex].transform.Find("Wheel_BL");
                wheelBR = wheelPool[currWheIndex].transform.Find("Wheel_BR");

                if (displayedVehicle > 0)
                {
                    //Wheel Connection points
                    connFL = vehiclePool[currVehIndex].transform.Find("CON_Wheel_FL");
                    connFR = vehiclePool[currVehIndex].transform.Find("CON_Wheel_FR");
                    connBL = vehiclePool[currVehIndex].transform.Find("CON_Wheel_BL");
                    connBR = vehiclePool[currVehIndex].transform.Find("CON_Wheel_BR");
                }
            }
            #endregion

            #region Adjust chassis position and rotation to account for wheel size (if there are wheels)
            if(displayedVehicle > 0)
            {
                Transform vehicleT = vehiclePool[displayedVehicle].transform;

                if (displayedWheel > 0)
                {/*
                    float frontWheelHeight = (wheelFL.GetComponent<MeshRenderer>().bounds.size.y / 2) - connFL.localPosition.y;
                    float backWhellHeight = (wheelBL.GetComponent<MeshRenderer>().bounds.size.y / 2) - connBL.localPosition.y;

                    float vehicleHeight = Mathf.Abs(frontWheelHeight - backWhellHeight) / 2;

                    vehicleT.position = new Vector3(0, vehicleHeight, 0);*/

                    float wheelFrontRadius = wheelFL.GetComponent<MeshRenderer>().bounds.size.y / 2;
                    float wheelBackRadius = wheelBL.GetComponent<MeshRenderer>().bounds.size.y / 2;

                    float connDistZ = connFL.localPosition.z - connBL.localPosition.z;
                    float connDistY = (connFL.localPosition.y - wheelFrontRadius) - (connBL.localPosition.y - wheelBackRadius);
                    float vehicleAngleX = Mathf.Tan(connDistY / connDistZ) * Mathf.Rad2Deg;

                    float frontWheelHeight = (wheelFL.GetComponent<MeshRenderer>().bounds.size.y / 2) - connFL.localPosition.y;
                    float backWheelHeight = (wheelBL.GetComponent<MeshRenderer>().bounds.size.y / 2) - connBL.localPosition.y;

                    float vehicleHeight = connDistY * (wheelFrontRadius / wheelBackRadius);

                    vehicleT.position = new Vector3(0, Mathf.Min(frontWheelHeight, backWheelHeight), 0);
                    vehicleT.localEulerAngles = new Vector3(vehicleAngleX, 0, 0);
                }

                else
                {
                    vehicleT.position = Vector3.zero;
                    vehicleT.eulerAngles = Vector3.zero;
                }
            }
            #endregion

            #region Position wheels

            if(displayedWheel > 0)
            {
                if (displayedVehicle > 0)
                {
                    wheelFL.position = connFL.position;
                    wheelFL.rotation = connFL.rotation;
                    wheelFR.position = connFR.position;
                    wheelFR.rotation = connFR.rotation;
                    wheelBL.position = connBL.position;
                    wheelBL.rotation = connBL.rotation;
                    wheelBR.position = connBR.position;
                    wheelBR.rotation = connFR.rotation;
                }

                else
                {
                    wheelFL.localPosition = new Vector3(-.25f, wheelFL.GetComponent<MeshRenderer>().bounds.size.y / 2, .5f);
                    wheelFL.localEulerAngles = new Vector3(0, -180, 0);
                    wheelFR.localPosition = new Vector3(.25f, wheelFL.GetComponent<MeshRenderer>().bounds.size.y / 2, .5f);
                    wheelBL.localPosition = new Vector3(-.25f, wheelFL.GetComponent<MeshRenderer>().bounds.size.y / 2, -.5f);
                    wheelBL.localEulerAngles = new Vector3(0, -180, 0);
                    wheelBR.localPosition = new Vector3(.25f, wheelFL.GetComponent<MeshRenderer>().bounds.size.y / 2, -.5f);
                }
            }
            #endregion
        }
    }
}

