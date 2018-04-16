using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BlackFox {
    public class AvatarSelectionController : BaseMenu {

        AvatarSelectionManager avatarSelectionManager;

        public Text ConfirmText;

        public UIControllerID MenuID;
        Slider[] sliders;

        AvatarSelectionControllerState _currentState;
        public AvatarSelectionControllerState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                if (_currentState == AvatarSelectionControllerState.Ready)
                {
                    avatarSelectionManager.CheckUpgradeControllersState();
                    GameManager.Instance.PlayerMng.ChangePlayerState(PlayerState.Blocked, Player.ID);
                    ConfirmText.text = "Ready";
                }
            }
        }

        public void SetSliderValues(int[] _values)
        {
            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i].value = _values[i];
            }
        }


        public void Setup(AvatarSelectionManager _avatarSelectionManager, Player _player) {
            avatarSelectionManager = _avatarSelectionManager;
            Player = _player;
            CurrentState = AvatarSelectionControllerState.Unready;
            ConfirmText.text = "Press RT to continue";
            sliders = GetComponentsInChildren<Slider>();
        }


        public override void GoRightInMenu(Player _player) {
            GameManager.Instance.SRMng.rooms[(int)_player.ID - 1].ShowNextModel();
        }

        public override void GoLeftInMenu(Player _player) {
            GameManager.Instance.SRMng.rooms[(int)_player.ID - 1].ShowPreviousModel();
        }

        public override void GoUpInMenu(Player _player)
        {
            GameManager.Instance.SRMng.rooms[(int)_player.ID - 1].ShowNextColor();
        }

        public override void GoDownInMenu(Player _player)
        {
            GameManager.Instance.SRMng.rooms[(int)_player.ID - 1].ShowPreviousColor();
        }

        public override void Selection(Player _player)
        {
            CurrentState = AvatarSelectionControllerState.Ready;
        }
    }

    public enum AvatarSelectionControllerState
    {
        Unready = 0,
        Ready = 1
    }
}