using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace BlackFox
{
    public class PlayerInput
    {
        // Variabili per il funzionamento dei controller
        PlayerIndex playerIndex;
        ButtonState rightTriggerOldState;
        ButtonState leftTriggerOldState;
        GamePadState state;
        GamePadState prevState;

        public PlayerInput(PlayerLabel _playerID)
        {
            switch (_playerID)
            {
                case PlayerLabel.None:
                    break;
                case PlayerLabel.One:
                    playerIndex = PlayerIndex.One;
                    break;
                case PlayerLabel.Two:
                    playerIndex = PlayerIndex.Two;
                    break;
                case PlayerLabel.Three:
                    playerIndex = PlayerIndex.Three;
                    break;
                case PlayerLabel.Four:
                    playerIndex = PlayerIndex.Four;
                    break;
                case PlayerLabel.Different:
                    break;
            }
        }

        #region API
        /// <summary>
        /// Ritorna l'input del player nella forma di struttura
        /// </summary>
        /// <returns></returns>
        public InputStatus GetPlayerInputStatus()
        {
            InputStatus inputStatus = ControllerInput();
            if (!inputStatus.IsConnected)
                inputStatus = KeyboardInput();

            return inputStatus;
        }

        public void SetControllerVibration(float _leftMotor, float _rightMotor)
        {
            GamePad.SetVibration(playerIndex, _leftMotor, _rightMotor);
        }
        #endregion

        #region ControllerInput
        /// <summary>
        /// Controlla l'input da controller (usando il plugin XInputDotNetPure)
        /// </summary>
        InputStatus ControllerInput()
        {
            InputStatus inputStatus = new InputStatus();

            prevState = state;
            state = GamePad.GetState(playerIndex, GamePadDeadZone.Circular);

            if (!state.IsConnected)
            {
                inputStatus.IsConnected = state.IsConnected;
                return inputStatus;
            }
            else
                inputStatus.IsConnected = state.IsConnected;

            inputStatus.RightTriggerAxis = state.Triggers.Right;

            // Right Trigger as button
            if (inputStatus.RightTriggerAxis <= 0.1f)
            {
                // rilasciato
                rightTriggerOldState = inputStatus.RightTrigger = ButtonState.Released;                
            }
            else
            {
                if (rightTriggerOldState == ButtonState.Released)
                    rightTriggerOldState = inputStatus.RightTrigger = ButtonState.Pressed;

                else
                    rightTriggerOldState = inputStatus.RightTrigger = ButtonState.Held;
            }

            inputStatus.LeftTriggerAxis = state.Triggers.Left;

            // Left Trigger as button
            if (inputStatus.LeftTriggerAxis <= 0.1f)
            {
                // rilasciato
                leftTriggerOldState = inputStatus.LeftTrigger = ButtonState.Released;
            }
            else
            {
                if (leftTriggerOldState == ButtonState.Released)
                    leftTriggerOldState = inputStatus.LeftTrigger = ButtonState.Pressed;

                else
                    leftTriggerOldState = inputStatus.LeftTrigger = ButtonState.Held;
            }

            inputStatus.LeftThumbSticksAxisX = state.ThumbSticks.Left.X;
            inputStatus.LeftThumbSticksAxisY = state.ThumbSticks.Left.Y;

            inputStatus.RightThumbSticksAxisX = state.ThumbSticks.Right.X;
            inputStatus.RightThumbSticksAxisY = state.ThumbSticks.Right.Y;

            if (prevState.Buttons.RightShoulder == XInputDotNetPure.ButtonState.Released && state.Buttons.RightShoulder == XInputDotNetPure.ButtonState.Pressed)
            {
                inputStatus.RightShoulder = ButtonState.Pressed;
            }

            if (prevState.Buttons.A == XInputDotNetPure.ButtonState.Released && state.Buttons.A == XInputDotNetPure.ButtonState.Pressed)
            {
                inputStatus.A = ButtonState.Pressed;
            }

            if (prevState.Buttons.B == XInputDotNetPure.ButtonState.Released && state.Buttons.B == XInputDotNetPure.ButtonState.Pressed)
            {
                inputStatus.B = ButtonState.Pressed;
            }

            if (prevState.DPad.Up == XInputDotNetPure.ButtonState.Released && state.DPad.Up == XInputDotNetPure.ButtonState.Pressed)
            {
                inputStatus.DPadUp = ButtonState.Pressed;
            }

            if (prevState.DPad.Left == XInputDotNetPure.ButtonState.Released && state.DPad.Left == XInputDotNetPure.ButtonState.Pressed)
            {
                inputStatus.DPadLeft = ButtonState.Pressed;
            }

            if (prevState.DPad.Down == XInputDotNetPure.ButtonState.Released && state.DPad.Down == XInputDotNetPure.ButtonState.Pressed)
            {
                inputStatus.DPadDown = ButtonState.Pressed;
            }

            if (prevState.DPad.Right == XInputDotNetPure.ButtonState.Released && state.DPad.Right == XInputDotNetPure.ButtonState.Pressed)
            {
                inputStatus.DPadRight = ButtonState.Pressed;
            }

            if (prevState.Buttons.Start == XInputDotNetPure.ButtonState.Released && state.Buttons.Start == XInputDotNetPure.ButtonState.Pressed)
            {
                inputStatus.Start = ButtonState.Pressed;
            }

            return inputStatus;
        }
        #endregion

        #region KeyboardInput
        /// <summary>
        /// Controlla l'input da tastiera (la tastiera non funziona se è collegato il controller dello stesso player Index)
        /// </summary>
        InputStatus KeyboardInput()
        {
            InputStatus inputStatus = new InputStatus();

            inputStatus.LeftThumbSticksAxisX = Input.GetAxis("Key" + (int)playerIndex + "_Horizonatal");
            inputStatus.LeftThumbSticksAxisY = Input.GetAxis("Key" + (int)playerIndex + "_Forward");

            inputStatus.RightThumbSticksAxisX = Input.GetAxis("Key" + (int)playerIndex + "_ShootX");
            inputStatus.RightThumbSticksAxisY = Input.GetAxis("Key" + (int)playerIndex + "_ShootY");

            if (Input.GetButtonDown("Key" + (int)playerIndex + "_PlacePin"))
            {
                inputStatus.LeftTrigger = ButtonState.Pressed;
            }

            if (Input.GetButtonDown("Key" + (int)playerIndex + "_Fire"))
            {
                inputStatus.RightTrigger = ButtonState.Pressed;
            }
            else if (Input.GetButton("Key" + (int)playerIndex + "_Fire"))
            {
                inputStatus.RightTrigger = ButtonState.Held;
            }

            if (Input.GetButtonDown("Key" + (int)playerIndex + "_DPadUp"))
            {
                inputStatus.DPadUp = ButtonState.Pressed;
            }

            if (Input.GetButtonDown("Key" + (int)playerIndex + "_DPadLeft"))
            {
                inputStatus.DPadLeft = ButtonState.Pressed;
            }

            if (Input.GetButtonDown("Key" + (int)playerIndex + "_DPadDown"))
            {
                inputStatus.DPadDown = ButtonState.Pressed;
            }

            if (Input.GetButtonDown("Key" + (int)playerIndex + "_DPadRight"))
            {
                inputStatus.DPadRight = ButtonState.Pressed;
            }

            if (Input.GetButtonDown("Key" + (int)playerIndex + "_Submit"))
            {
                inputStatus.RightTrigger = ButtonState.Pressed;
            }

            if (Input.GetButtonDown("Key" + (int)playerIndex + "_Deselect"))
            {
                inputStatus.LeftTrigger = ButtonState.Pressed;
            }

            if (Input.GetButtonDown("Key" + (int)playerIndex + "_Pause"))
            {
                inputStatus.Start = ButtonState.Pressed;
            }

            return inputStatus;
        }
        #endregion
    }

    /// <summary>
    /// Stato del bottone
    /// </summary>
    public enum ButtonState
    {
        Released = 0,
        Pressed = 1,
        Held = 2
    }

    /// <summary>
    /// Struttura che contine tutti i comandi del joystick
    /// </summary>
    public class InputStatus
    {
        public bool IsConnected;

        public float LeftTriggerAxis;
        public float RightTriggerAxis;

        public float LeftThumbSticksAxisX;
        public float LeftThumbSticksAxisY;

        public float RightThumbSticksAxisX;
        public float RightThumbSticksAxisY;

        public ButtonState A;
        public ButtonState B;
        public ButtonState X;
        public ButtonState Y;

        public ButtonState LeftShoulder;
        public ButtonState RightShoulder;

        public ButtonState LeftTrigger;
        public ButtonState RightTrigger;

        public ButtonState LeftThumbSticks;
        public ButtonState RightThumbSticks;

        public ButtonState DPadUp;
        public ButtonState DPadLeft;
        public ButtonState DPadDown;
        public ButtonState DPadRight;

        public ButtonState Start;
        public ButtonState Select;

        /// <summary>
        /// Reset the value of each field as default
        /// </summary>
        public void Reset()
        {
            IsConnected = false;

            LeftTriggerAxis = 0;
            RightTriggerAxis = 0;

            LeftThumbSticksAxisX = 0;
            LeftThumbSticksAxisY = 0;

            RightThumbSticksAxisX = 0;
            RightThumbSticksAxisY = 0;

            A = ButtonState.Released;
            B = ButtonState.Released;
            X = ButtonState.Released;
            Y = ButtonState.Released;

            LeftShoulder = ButtonState.Released;
            RightShoulder = ButtonState.Released;

            LeftTrigger = ButtonState.Released;
            RightTrigger = ButtonState.Released;

            LeftThumbSticks = ButtonState.Released;
            RightThumbSticks = ButtonState.Released;

            DPadUp = ButtonState.Released;
            DPadLeft = ButtonState.Released;
            DPadDown = ButtonState.Released;
            DPadRight = ButtonState.Released;

            Start = ButtonState.Released;
            Select = ButtonState.Released;
        }
    }
}