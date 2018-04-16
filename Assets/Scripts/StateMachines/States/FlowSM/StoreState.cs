using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class StoreState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("StoreState");
            GameManager.Instance.UiMng.CreateStoreMenu();
            GameManager.Instance.UiMng.CurrentMenu = GameManager.Instance.UiMng.storeController;
            GameManager.Instance.LoadingCtrl.DeactivateLoadingPanel();
            GameManager.Instance.PlayerMng.ChangeAllPlayersStateExceptOne(PlayerState.MenuInput, PlayerLabel.One, PlayerState.Blocked);
        }

        public override void OnEnd()
        {
            GameManager.Instance.SRMng.rooms[0].ReSetFirstShowRoom();
            GameManager.Instance.UiMng.DestroyStoreMenu();
        }
    }
}