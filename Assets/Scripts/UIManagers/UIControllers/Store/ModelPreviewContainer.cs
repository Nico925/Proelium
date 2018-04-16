using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace BlackFox {
    public class ModelPreviewContainer : BaseMenu {

        List<RawImage> Images = new List<RawImage>();
        StoreController storeController;
        int _IndexSelected;

        public int IndexSelected
        {
            get { return _IndexSelected; }
            set
            {
                _IndexSelected = value;
                if (_IndexSelected > 4 - 1)
                    _IndexSelected = 0;

                if (_IndexSelected < 0)
                    _IndexSelected = 4 - 1;
            }
        }

        public void Init(StoreController _controller)
        {
            storeController = _controller;
            Images = GetComponentsInChildren<RawImage>().ToList();
        }

        #region Menu Actions

        public override void Selection(Player _player)
        {
            //GameManager.Instance.DataMng.PurchaseAvatar()
        }


        //public void SetSliderValues(int[] _values)
        //{
        //    for (int i = 0; i < sliders.Length; i++)
        //    {
        //        sliders[i].value = _values[i];
        //    }
        //}

        

        #endregion
    }
}