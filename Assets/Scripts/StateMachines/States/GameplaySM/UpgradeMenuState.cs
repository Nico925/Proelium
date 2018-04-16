using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {

    public class UpgradeMenuState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("UpgradeMenuState");
            GameManager.Instance.PlayerMng.ChangeAllPlayersState(PlayerState.MenuInput);
            GameManager.Instance.UiMng.CurrentMenu = GameManager.Instance.UiMng.canvasGame.upgradeMenuManager;
            GameManager.Instance.UiMng.canvasGame.upgradeMenuManager.UpgradePanel.SetActive(true);
            GameManager.Instance.UiMng.canvasGame.upgradeMenuManager.InitControllers();
            GameManager.Instance.LoadingCtrl.DeactivateLoadingPanel();
        }

        public override void OnEnd()
        {
            GameManager.Instance.UiMng.canvasGame.upgradeMenuManager.UpgradePanel.SetActive(false);
        }
    }
}