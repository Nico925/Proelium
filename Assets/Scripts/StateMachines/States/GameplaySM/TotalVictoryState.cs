using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{

    public class TotalVictoryState : StateBase
    {

        public override void OnStart()
        {
            GameManager.Instance.LoadingCtrl.DeactivateLoadingPanel();
            GameManager.Instance.UiMng.CurrentMenu = GameManager.Instance.UiMng.canvasGame.endRoundUI;
            GameManager.Instance.UiMng.canvasGame.endRoundUI.ActiveTotalVictory();
            GameManager.Instance.UiMng.canvasGame.endRoundUI.SetTotalVictoryImage(GameManager.Instance.LevelMng.LevelWinner);
        }

        public override void OnEnd()
        {
            GameManager.Instance.UiMng.canvasGame.endRoundUI.SetEndRoundPanelStatus(false);
        }
    }
}