using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class RoundEndState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("RoundEndState");
            GameManager.Instance.LevelMng.AvatarSpwn.BreakSpawnsInQueue();
            GameManager.Instance.LevelMng.SpawnerMng.ToggleSpawners(false);
            GameManager.Instance.LevelMng.PowerUpMng.Toggle(false);
            GameManager.Instance.UiMng.CurrentMenu = GameManager.Instance.UiMng.canvasGame.endRoundUI;
            GameManager.Instance.UiMng.canvasGame.endRoundUI.SetEndRoundPanelStatus(true);
            GameManager.Instance.PlayerMng.ChangeAllPlayersStateExceptOne(PlayerState.MenuInput, PlayerLabel.One, PlayerState.Blocked);
            GameManager.Instance.PlayerMng.CleanAllAmmo();
        }
        
        public override void OnEnd()
        {
            GameManager.Instance.UiMng.canvasGame.endRoundUI.SetEndRoundPanelStatus(false);
            GameManager.Instance.LevelMng.ClearPoints();
            GameManager.Instance.UiMng.canvasGame.gameUIController.ResetKillPointsUI();
        }
    }
}
