using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{

    public class PlayerUpgradeController : BaseMenu
    {
        public UIControllerID MenuID;

        public List<Image> Cups = new List<Image>();

        /// <summary>
        /// Variabile che si incrementa ogni volta che viene applicato un potenziamento. 
        /// Una volta uguale ai punti upgrade dell'avatar, non è più possibile upgradare.
        /// </summary>
        int UpgradeCounter = 0;

        public int AvatarUpgradePoints {
            get
            {
                return Player.Avatar.UpgradePoints;
            }
            set
            {
                Player.Avatar.UpgradePoints = value;
            }
        }

        UpgradeMenuManager UpgradeMng;

        public Text PlayerIdText;
        public Text ConfirmText;
        public Text UpgradePintsText;

        UpgradeControllerState _currentState;
        public UpgradeControllerState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                if(_currentState == UpgradeControllerState.Ready)
                {
                    UpgradeMng.CheckUpgradeControllersState();
                    GameManager.Instance.PlayerMng.ChangePlayerState(PlayerState.Blocked, Player.ID);
                    ConfirmText.text = "Ready";
                }
            }
        }

        public List<IUpgrade> Upgrades
        {
            get { return Player.Avatar.Upgrades; }
        }

        /// <summary>
        /// Aggiorna la scritta che indica quanti punti potenziamento sono rimati
        /// </summary>
        void UpgradeGraphics()
        {
            UpgradePintsText.text = (AvatarUpgradePoints - UpgradeCounter).ToString();
        }

        /// <summary>
        /// Si prende la quantità di vittorie relative al proprio player in base all'indice del menù
        /// </summary>
        void ShowVictories()
        {
            int victories = GameManager.Instance.LevelMng.GetPlayerVictory((int)MenuID);
            if (victories > 0)
            {
                for (int i = 0; i < victories; i++)
                {
                    Cups[i].enabled = true;
                } 
            }
        }


        #region API

        public void Setup(UpgradeMenuManager _upgradeMng, Player _player)
        {
            UpgradeMng = _upgradeMng;
            Player = _player;
        }

        public void Init()
        {
            for (int i = 0; i < SelectableButtons.Count && i < Upgrades.Count; i++)
            {
                (SelectableButtons[i] as ISelectableUpgrade).SetIUpgrade(Upgrades[i]);
                (SelectableButtons[i] as SelectableUpgrade).Init(UpgradeMng.ActiveSlide, UpgradeMng.DeactiveSlider);
            }
            SelectableButtons[0].IsSelected = true;
            ShowVictories();

            CurrentState = UpgradeControllerState.Unready;
            UpgradeCounter = 0;
            UpgradeGraphics();
            ConfirmText.text = "Press RT to continue";
            PlayerIdText.text = "Player " + Player.ID;
        }

        public override void Selection(Player _player)
        {
            for (int i = 0; i < Upgrades.Count; i++)
                Upgrades[i] = (SelectableButtons[i] as ISelectableUpgrade).GetData();
            CurrentState = UpgradeControllerState.Ready;
            AvatarUpgradePoints -= UpgradeCounter;
        }

        public override void GoRightInMenu(Player _player)
        {
            if (UpgradeCounter < AvatarUpgradePoints && (_selectableButtons[currentIndexSelection] as SelectableUpgrade).Upgrade.CurrentUpgradeLevel < (_selectableButtons[currentIndexSelection] as SelectableUpgrade).Upgrade.MaxLevel)
            {
                UpgradeCounter++;
                (_selectableButtons[currentIndexSelection] as SelectableUpgrade).AddValue();
                UpgradeGraphics();
            }
        }

        public override void GoLeftInMenu(Player _player)
        {
            if (UpgradeCounter > 0 && (_selectableButtons[currentIndexSelection] as SelectableUpgrade).Upgrade.CurrentUpgradeLevel > (_selectableButtons[currentIndexSelection] as SelectableUpgrade).Upgrade.MinLevel)
            {
                UpgradeCounter--;
                (_selectableButtons[currentIndexSelection] as SelectableUpgrade).RemoveValue();
                UpgradeGraphics();
            }
        }

        #endregion
    }

    /// <summary>
    /// Serve per associare al upgrade controller il player corrispondente
    /// </summary>
    public enum UIControllerID
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }

    public enum UpgradeControllerState
    {
        Unready = 0,
        Ready = 1
    }
}