using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    public class CreditsMenuController :  BaseMenu
    {
        public override void GoBack(Player _player)
        {
            GameManager.Instance.PlayerMng.ChangeAllPlayersState(PlayerState.Blocked);
            GameManager.Instance.LoadingCtrl.ActivateLoadingPanel(() => {
                GameManager.Instance.flowSM.SetPassThroughOrder(new List<StateBase>() { new MainMenuState() });
            });
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Back);
        }
    }
}