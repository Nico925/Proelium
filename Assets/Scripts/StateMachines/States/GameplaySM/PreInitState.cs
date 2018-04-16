using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class PreInitState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("PreInitState");
            //GameManager.Instance.UiMng.CreateLoadingCanvas();
            //GameManager.Instance.UiMng.loadingCanvas.FadeIn();

            GameManager.Instance.LevelMng.InstantiateArena();
            GameManager.Instance.LevelMng.InstantiateAvatarSpawner();
            GameManager.Instance.LevelMng.InstantiateRopeManager();
            GameManager.Instance.LevelMng.InstantiateUpgradePointsManager();
            GameManager.Instance.LevelMng.InstantiatePowerUpManager();
            GameManager.Instance.LevelMng.InstantiateSpawnerManager();
            GameManager.Instance.LevelMng.InstantiateExplosionPoolManager();
            GameManager.Instance.LevelMng.SpawnerMng.InstantiateNewSpawners(GameManager.Instance.LevelMng.CurrentLevel);
            GameManager.Instance.CoinMng.InstantiateCoinController();

            if (Camera.main.GetComponentInChildren<AudioListener>() != null)
                Camera.main.GetComponentInChildren<AudioListener>().enabled = false;

            GameManager.Instance.PlayerMng.ChangeAllPlayersState(PlayerState.Blocked);

            GameManager.Instance.LevelMng.SetupCore();
            GameManager.Instance.UiMng.canvasGame.upgradeMenuManager.Setup(GameManager.Instance.PlayerMng.Players);
            GameManager.Instance.PlayerMng.SetupAvatars(true);
            GameManager.Instance.UiMng.canvasGame.gameUIController.Init();

        }

        public override void OnUpdate()
        {
            if (OnStateEnd != null)
                OnStateEnd();
        }
    }
}