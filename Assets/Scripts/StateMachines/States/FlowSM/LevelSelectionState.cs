using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BlackFox
{
    public class LevelSelectionState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("LevelSelectionState");
            GameManager.Instance.UiMng.CreateLevelSelectionMenu();
            GameManager.Instance.LoadingCtrl.DeactivateLoadingPanel();
            GameManager.Instance.PlayerMng.ChangeAllPlayersStateExceptOne(PlayerState.MenuInput, PlayerLabel.One, PlayerState.Blocked);
        }

        public override void OnEnd()
        {
            GameManager.Instance.UiMng.DestroyLevelSelectionMenu();
        }
    }
}