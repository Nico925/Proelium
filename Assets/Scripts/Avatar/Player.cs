using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class Player : MonoBehaviour
    {
        protected PlayerLabel _ID;
        public PlayerLabel ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private Avatar _avatar;
        public Avatar Avatar
        {
            get { return _avatar; }
            protected set { _avatar = value; }
        }

        private AvatarData _avatarData;
        public AvatarData AvatarData
        {
            get {
                if (_avatarData == null) 
                    _avatarData = Instantiate(Resources.Load("ShipModels/Owl") as AvatarData);
                return _avatarData;
            }
            set { _avatarData = value; }
        }

        public float SpawnTime = 0;

        private void Update()
        {
            switch (PlayerCurrentState)
            {
                case PlayerState.Blocked:
                    break;
                case PlayerState.MenuInput:
                    InputStatus = playerInput.GetPlayerInputStatus();
                    CheckMenuInputStatus();
                    break;
                case PlayerState.PlayInput:
                    InputStatus = playerInput.GetPlayerInputStatus();
                    break;
                default:
                    break;
            }
        }

        #region API
        /// <summary>
        /// Mut be used to setup an istance of a Player
        /// </summary>
        /// <param name="_playerIndex"></param>
        public void Setup(PlayerLabel _playerID)
        {
            ID = _playerID;
            playerInput = new PlayerInput(_playerID);
        }

        #region Avatar
        /// <summary>
        /// Makes a new istance of the Avatar using the already loaded model
        /// </summary>
        public void InstanceAvatar()
        {
            DestroyAvatar();
            Avatar = new GameObject("Avatar" + ID).AddComponent<Avatar>();
            Avatar.transform.parent = transform;
        }

        /// <summary>
        /// Setup the istance of the Avatar or instaciate a new one
        /// </summary>
        /// <param name="_forceIstance">If true and no other istance of Avatar, istance a new one</param>
        public void AvatarSetup(bool _forceIstance = false)
        {
            if (ID == PlayerLabel.None)
                return;
            if (_forceIstance == true)
                InstanceAvatar();
            Avatar.Setup(this);
        }

        /// <summary>
        /// Destroy the Avatar istance and the connected rope if there is one
        /// </summary>
        public void DestroyAvatar()
        {
            if (Avatar)
                Destroy(Avatar.gameObject);
        }
        #endregion
        #endregion

        #region PlayerSM
        PlayerState _playerCurrentState;
        /// <summary>
        /// Stato attuale.
        /// </summary>
        public PlayerState PlayerCurrentState
        {
            get { return _playerCurrentState; }
            set {
                if(_playerCurrentState != value)
                    StateChange(value, _playerCurrentState);
                _playerCurrentState = value;
            }
        }

        void StateChange(PlayerState _newState, PlayerState _oldState)
        {
            switch (_newState)
            {
                case PlayerState.Blocked:
                    InputStatus.Reset();
                    break;
                case PlayerState.MenuInput:
                    break;
                case PlayerState.PlayInput:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Input
        //Input fields
        PlayerInput playerInput;
        private InputStatus _inputStatus;
        public InputStatus InputStatus
        {
            get {
                if (_inputStatus == null)
                    _inputStatus = new InputStatus();
                return _inputStatus;
            }
            set { _inputStatus = value; }
        }

        bool isReleasedY = true;
        bool isReleasedX = true;

        /// <summary>
        /// Controlla l'inpunt da passare al menù corrente 
        /// </summary>
        /// <param name="inputStatus"></param>
        void CheckMenuInputStatus()
        {
            if (InputStatus.LeftThumbSticksAxisY <= 0.2 && InputStatus.LeftThumbSticksAxisY >= -0.2)
                isReleasedY = true;

            if ((InputStatus.DPadUp == ButtonState.Pressed || InputStatus.LeftThumbSticksAxisY >= 0.5) && isReleasedY)
            {
                isReleasedY = false;
                GameManager.Instance.UiMng.GoUpInMenu(this);
            }

            if ((InputStatus.DPadDown == ButtonState.Pressed || InputStatus.LeftThumbSticksAxisY <= -0.5) && isReleasedY)
            {
                isReleasedY = false;
                GameManager.Instance.UiMng.GoDownInMenu(this);
            }

            if (InputStatus.LeftThumbSticksAxisX <= 0.2 && InputStatus.LeftThumbSticksAxisX >= -0.2)
                isReleasedX = true;

            if ((InputStatus.DPadRight == ButtonState.Pressed || InputStatus.LeftThumbSticksAxisX >= 0.5) && isReleasedX)
            {
                isReleasedX = false;
                GameManager.Instance.UiMng.GoRightInMenu(this);
            }

            if ((InputStatus.DPadLeft == ButtonState.Pressed || InputStatus.LeftThumbSticksAxisX <= -0.5) && isReleasedX)
            {
                isReleasedX = false;
                GameManager.Instance.UiMng.GoLeftInMenu(this);
            }

            if (InputStatus.RightTrigger == ButtonState.Pressed)
            {
                GameManager.Instance.UiMng.SelectInMenu(this);
            }

            if (InputStatus.LeftTrigger == ButtonState.Pressed)
            {
                GameManager.Instance.UiMng.GoBackInMenu(this);
            }
        }

        public void ControllerVibration(float _leftMotor, float _rightMotor)
        {
            if(InputStatus.IsConnected)
                playerInput.SetControllerVibration(_leftMotor, _rightMotor);
        }
        #endregion
    }

    public enum PlayerLabel
    {
        None = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Different = 5
    }
}