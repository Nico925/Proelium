using System;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class PlacePin : MonoBehaviour
    {
        protected bool canPlace = true;
        SphereCollider probe;
        int layer = 9;

        PlacePinConfig placePinConfig
        {
            get {
                PlacePinConfig data;
                if (ship.Avatar.AvatarData.shipConfig.placePinConfig == null)
                    data = new PlacePinConfig();
                else
                    data = ship.Avatar.AvatarData.shipConfig.placePinConfig;
                return data; }
        }

        List<GameObject> pinsPlaced = new List<GameObject>();
        Transform initialTransf;
        Ship ship;
        float prectime;

        private void Update()
        {
            if (ship != null)
            {
                if (!GameManager.Instance.LevelMng.IsGamePaused || GameManager.Instance.LevelMng.IsRoundActive)
                {
                    prectime -= Time.deltaTime;
                    ship.Avatar.avatarUI.PinCountDown.fillAmount  -= Time.deltaTime / CurrentPinRate;
                }                
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.tag == "PinBlockArea")
                canPlace = false;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "PinBlockArea")
                canPlace = true;
        }

        public float CurrentPinRate
        {
            get
            {
                if (ship.Avatar.GetUpgrade(UpgardeTypes.PinRegeneration) != null)
                    return ship.Avatar.GetUpgrade(UpgardeTypes.PinRegeneration).CalculateValue(placePinConfig.CoolDownTime);
                else
                    return placePinConfig.CoolDownTime;
            }

        }

        #region API
        /// <summary>
        /// Set working values for the componet
        /// </summary>
        /// <param name="_owner"></param>
        public void Setup(Ship _owner)
        {
            ship = _owner;
            prectime = CurrentPinRate;
            initialTransf = transform;
            ProbeSetup();
        }

        public void Init()
        {
            canPlace = true;
            transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Change the layer of collision of each Pin
        /// </summary>
        /// <param name="_layerOrdinalNumber">Actual ordinal number of the layer of collision</param>
        public void SetCollisionLayer(int _layerOrdinalNumber)
        {
            layer = _layerOrdinalNumber;
            foreach (GameObject pin in pinsPlaced)
            {
                pin.layer = layer;
            }
        }

        /// <summary>
        /// Instance a Pin if possible
        /// </summary>
        /// <returns>True is instance succeded</returns>
        public bool PlaceThePin()
        {
            if (prectime <= 0 && canPlace == true)
            {
                GameObject pin = Instantiate(placePinConfig.PinPrefab, transform.position + transform.forward*placePinConfig.DistanceFromShipOrigin, transform.rotation);
                pin.layer = layer;
                pin.transform.localScale = Vector3.zero;
                pin.transform.DOScale(Vector3.one, 0.5f);
                pinsPlaced.Add(pin);
                foreach (Renderer pinRend in pin.GetComponentsInChildren<MeshRenderer>())
                {
                    pinRend.material = ship.Avatar.AvatarData.ColorSets[ship.Avatar.AvatarData.ColorSetIndex].PinMaterial;
                }
                pin.transform.parent = GameManager.Instance.LevelMng.PinsContainer;
                prectime = CurrentPinRate;
                ship.Avatar.avatarUI.PinCountDown.fillAmount = 1;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove all the placed Pins
        /// </summary>
        public void RemoveAllPins()
        {
            foreach (GameObject pin in pinsPlaced)
            {
                Destroy(pin);
            }
            pinsPlaced.Clear();
        }
        #endregion

        /// <summary>
        /// Instance a point-like sphere collider onte the Pin drop position to check if it is a legal position or not
        /// </summary>
        void ProbeSetup()
        {
            probe = gameObject.AddComponent<SphereCollider>();
            probe.radius = float.Epsilon;
            probe.center = new Vector3(0, 0, placePinConfig.DistanceFromShipOrigin);
            probe.isTrigger = true;
        }
    }

    [Serializable]
    public class PlacePinConfig
    {
        public GameObject PinPrefab;
        public float CoolDownTime = 3;
        public float DistanceFromShipOrigin;
    }
}
