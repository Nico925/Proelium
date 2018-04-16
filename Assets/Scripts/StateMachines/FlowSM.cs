using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    /// <summary>
    /// State machine che gestisce il flow generale dell'applicazione.
    /// </summary>
    public class FlowSM : StateMachineBase
    {
        private void Start()
        {
            CurrentState = new LoadGameState();
        }

        protected override void OnCurrentStateEnded()
        {
            switch (CurrentState.StateName)
            {
                case "BlackFox.LoadGameState":
                    CurrentState = new MainMenuState();
                    break;
                case "BlackFox.MainMenuState":
                    CurrentState = new AvatarSelectionState();
                    break;
                case "BlackFox.AvatarSelectionState":
                    CurrentState = new GameplayState();
                    break;
                case "BlackFox.GameplayState":
                    CurrentState = new MainMenuState();
                    break;
            }            
        }

        protected override bool CheckRules(StateBase _newState, StateBase _oldState)
        {
            if (_oldState == null)
                return true;

            switch (_newState.StateName)
            {
                case "BlackFox.LoadGameState":
                    return true;
                case "BlackFox.MainMenuState":
                    if (_oldState.StateName == "BlackFox.LoadGameState" || _oldState.StateName == "BlackFox.AvatarSelectionState" || 
                        _oldState.StateName == "BlackFox.GameplayState" || _oldState.StateName == "BlackFox.CreditsState" ||
                        _oldState.StateName == "BlackFox.ManualState" || _oldState.StateName == "BlackFox.StoreState")
                        return true;
                    break;
                case "BlackFox.StoreState":
                    if (_oldState.StateName == "BlackFox.MainMenuState")
                        return true;
                    break;
                case "BlackFox.CreditsState":
                    if (_oldState.StateName == "BlackFox.MainMenuState")
                        return true;
                    break;
                case "BlackFox.ManualState":
                    if (_oldState.StateName == "BlackFox.MainMenuState")
                        return true;
                    break;
                case "BlackFox.AvatarSelectionState":
                    if (_oldState.StateName == "BlackFox.MainMenuState")
                        return true;
                    break;
                case "BlackFox.GameplayState":
                    if (_oldState.StateName == "BlackFox.AvatarSelectionState")
                        return true;
                    break;
                default:
                    return false;
            }

            return false;
        }
    }
}
