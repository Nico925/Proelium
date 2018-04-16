using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlackFox
{
    /// <summary>
    /// Reppresenta lo stato di gameplay della flow state machine.
    /// </summary>
    public class GameplayState : StateBase
    {

        public override void OnStart()
        {
            Debug.Log("GameplayState");
            StateMachineBase.OnMachineEnd += OnMachineEnd;
            GameManager.Instance.UiMng.CreateGameMenu();
            GameManager.Instance.InstantiateLevelManager();
            GameManager.Instance.LevelMng.Init();
            GameManager.Instance.PlayerMng.ChangeAllPlayersState(PlayerState.PlayInput);
            if (EventManager.OnMusicChange != null)
                EventManager.OnMusicChange(AudioManager.Music.MenuTheme, false);
        }

        public override void OnEnd()
        {
            GameManager.Instance.UiMng.DestroyGameMenu();
            GameManager.Instance.DestroyLevelManager();
            StateMachineBase.OnMachineEnd -= OnMachineEnd;
        }

        void OnMachineEnd(string _machineName)
        {
            if(_machineName == "GameplaySM")
            {
                Debug.Log("GameplaySM_Stop");
                if (OnStateEnd != null)
                    OnStateEnd();
            }   
        }
    }
}
