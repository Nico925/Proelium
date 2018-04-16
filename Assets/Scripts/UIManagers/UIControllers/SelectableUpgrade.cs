using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{

    public class SelectableUpgrade : MonoBehaviour, ISelectableUpgrade
    {
        #region ISelectable

        bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                CheckIsSelected(isSelected);
            }
        }

        int index;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }


        public void SetIndex(int _index)
        {
            Index = _index;
        }

        public Image SelectionImage;
        Sprite selectedImg;
        Sprite deselectedImg;

        public void CheckIsSelected(bool _isSelected)
        {
            SelectionImage = GetComponentInChildren<Image>();

            if (_isSelected)
                SelectionImage.sprite = selectedImg;
            else
                SelectionImage.sprite = deselectedImg;
        }
        #endregion

        public Slider slider;
        public Text text;

        public IUpgrade Upgrade;

        
        public void Init(Sprite _activeSlider, Sprite _deactiveSlider)
        {
            selectedImg = _activeSlider;
            deselectedImg = _deactiveSlider;
        }

        public void AddValue()
        {
            if (Upgrade.CurrentUpgradeLevel < Upgrade.MaxLevel)
            {
                slider.value += 1;
                Upgrade.CurrentUpgradeLevel += 1;
            }
        }

        public void RemoveValue()
        {
            if(Upgrade.CurrentUpgradeLevel > Upgrade.MinLevel)
            {
                slider.value -= 1;
                Upgrade.CurrentUpgradeLevel -= 1;
            }
        }

        public void SetIUpgrade(IUpgrade _upgrade)
        {
            Upgrade = _upgrade;
            Upgrade.CurrentUpgradeLevel = Upgrade.MinLevel;
            slider.value = Upgrade.CurrentUpgradeLevel;
            slider.maxValue = Upgrade.MaxLevel;
            switch (Upgrade.ID)
            {
                case UpgardeTypes.FireRate:
                    text.text = "Fire Rate";
                    break;
                case UpgardeTypes.PinRegeneration:
                    text.text = "Pin Regeneration";
                    break;
                case UpgardeTypes.PowerUpDuration:
                    text.text = "Power Up Duration";
                    break;
                case UpgardeTypes.RopeLength:
                    text.text = "Rope Lenght";
                    break;
                case UpgardeTypes.BulletsRange:
                    text.text = "Bullets Range";
                    break;
                case UpgardeTypes.AmmoRecharge:
                    text.text = "Ammo Recharge";
                    break;
                default:
                    break;
            }
        }

        public IUpgrade GetData()
        {
            Upgrade.MinLevel = Upgrade.CurrentUpgradeLevel;
            return Upgrade;
        }
    }
}