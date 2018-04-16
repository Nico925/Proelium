using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class CreditsState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("CreditsState");
            GameManager.Instance.UiMng.CreateCreditsMenu();
            GameManager.Instance.UiMng.CurrentMenu = GameManager.Instance.UiMng.creditsMenuController;
            GameManager.Instance.LoadingCtrl.DeactivateLoadingPanel();
            GameManager.Instance.PlayerMng.ChangeAllPlayersStateExceptOne(PlayerState.MenuInput, PlayerLabel.One, PlayerState.Blocked);
        }

        public override void OnEnd()
        {
            GameManager.Instance.UiMng.DestroyCreditsMenu();
        }
    }
}