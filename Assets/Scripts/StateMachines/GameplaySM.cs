using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BlackFox {

    /// <summary>
    /// State machine che gestisce il flow di gameplay
    /// </summary>
    public class GameplaySM : StateMachineBase
    {
        public void Init()
        {
            Debug.Log("Start_GamePlaySM");
            CurrentState = new PreInitState();
        }

        protected override void OnCurrentStateEnded()
        {
            switch (CurrentState.StateName) {
                case "BlackFox.PreInitState":
                    CurrentState = new RoundInitState();
                    break;
                case "BlackFox.RoundInitState":
                    CurrentState = new PreStartState();
                    break;
                case "BlackFox.PreStartState":
                    CurrentState = new PlayState();
                    break;
                case "BlackFox.PlayState":
                    CurrentState = new CleanSceneState();
                    break;
                case "BlackFox.PauseState":
                    CurrentState = new PlayState();
                    break;
                case "BlackFox.CleanSceneState":
                    CurrentState = new RoundEndState();
                    break;
                case "BlackFox.RoundEndState":
                    ChangeRoundCondition();
                    break;
                case "BlackFox.UpgradeMenuState":
                    CurrentState = new RoundInitState();
                    break;
                case "BlackFox.TotalVictoryState":
                    CurrentState = new GameOverState();
                    break;
                case "BlackFox.GameOverState":
                    if (GameplaySM.OnMachineEnd != null)
                        GameplaySM.OnMachineEnd("GameplaySM");
                    break;
            }   
        }

        void ChangeRoundCondition()
        {
            if (GameManager.Instance.LevelMng.CheckIfLevelIsWon())
                CurrentState = new TotalVictoryState();
            else if (!GameManager.Instance.LevelMng.Core.IsCoreAlive())
                CurrentState = new RoundInitState();
            else
                CurrentState = new UpgradeMenuState();
        }
        protected override bool CheckRules(StateBase _newState, StateBase _oldState) 
        {
            if (_oldState == null) 
                return true;

            switch (_newState.StateName)
            {
                case "BlackFox.PreInitState":
                        return true;
                case "BlackFox.RoundInitState":
                    if (_oldState.StateName == "BlackFox.PreInitState" || _oldState.StateName == "BlackFox.UpgradeMenuState" || _oldState.StateName == "BlackFox.RoundEndState")
                        return true;
                    break;
                case "BlackFox.PreStartState":
                    if (_oldState.StateName == "BlackFox.RoundInitState")
                        return true;
                    break;
                case "BlackFox.PlayState":
                    if (_oldState.StateName == "BlackFox.PreStartState" || _oldState.StateName == "BlackFox.PauseState")
                        return true;
                    break;
                
                case "BlackFox.PauseState":
                    if (_oldState.StateName == "BlackFox.PlayState")
                        return true;
                    break;
                case "BlackFox.CleanSceneState":
                    if (_oldState.StateName == "BlackFox.PlayState" || _oldState.StateName == "BlackFox.PauseState")
                        return true;
                    break;
                case "BlackFox.RoundEndState":
                    if (_oldState.StateName == "BlackFox.CleanSceneState")
                        return true;
                    break;
                case "BlackFox.TotalVictoryState":
                    if (_oldState.StateName == "BlackFox.RoundEndState")
                        return true;
                    break;
                case "BlackFox.UpgradeMenuState":
                    if (_oldState.StateName == "BlackFox.RoundEndState")
                        return true;
                    break;
                case "BlackFox.GameOverState":
                    if (_oldState.StateName == "BlackFox.RoundEndState" || _oldState.StateName == "BlackFox.TotalVictoryState")
                        return true;
                    break;

            }
            return false;
        }
    }
}
