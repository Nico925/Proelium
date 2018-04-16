using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{
    public class SelectableButton : MonoBehaviour, ISelectable
    {
        Sprite SelectedButton;
        Sprite DeselectionButton;

        Image ButtonImage;

        bool isSelected;

        public bool IsSelected {
            get { return isSelected; }
            set { isSelected = value;
                CheckIsSelected(isSelected);
                }
        }

        int index;

        public int Index
        {
            get { return index;}

            set { index = value; }
        }

        public void Init(Sprite _selectedImg, Sprite _deselectedImg)
        {
            SelectedButton =_selectedImg;
            DeselectionButton = _deselectedImg;
            ButtonImage = GetComponent<Image>();
        }
        public void SetIndex(int _index)
        {  
            Index = _index;
        }
        
        public void CheckIsSelected(bool _isSelected)
        {
            if (_isSelected)
                ButtonImage.sprite = SelectedButton;
            else
                ButtonImage.sprite = DeselectionButton;
        }
    }
}