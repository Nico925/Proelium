using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class PauseState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("PauseState");
            Time.timeScale = 0;
            GameManager.Instance.UiMng.CurrentMenu = GameManager.Instance.UiMng.canvasGame.pauseMenuController;
            GameManager.Instance.UiMng.canvasGame.pauseMenuController.ChildrenPanel.SetActive(true);
            GameManager.Instance.UiMng.canvasGame.pauseMenuController.Init();
        }

        public override void OnEnd()
        {
            Time.timeScale = 1;
            GameManager.Instance.PlayerMng.ChangeAllPlayersState(PlayerState.PlayInput);
            GameManager.Instance.LevelMng.IsGamePaused = false;
            GameManager.Instance.UiMng.canvasGame.pauseMenuController.ChildrenPanel.SetActive(false);
        }
    }
}