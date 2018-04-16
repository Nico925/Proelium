using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class ManualState : StateBase
    {
        public override void OnStart()
        {
            GameManager.Instance.UiMng.CreateManualCanvas();

            GameManager.Instance.UiMng.CurrentMenu = GameManager.Instance.UiMng.manualController;

            GameManager.Instance.LoadingCtrl.DeactivateLoadingPanel();
            GameManager.Instance.PlayerMng.ChangeAllPlayersStateExceptOne(PlayerState.MenuInput, PlayerLabel.One, PlayerState.Blocked);

        }

        public override void OnEnd()
        {
            GameManager.Instance.UiMng.DestroyManualCanvas();
        }
    }
}