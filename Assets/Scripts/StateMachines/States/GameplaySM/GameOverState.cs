using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {

    public class GameOverState : StateBase {

        public override void OnStart() {
            Debug.Log("GameOverState");
            GameManager.Instance.PlayerMng.ChangeAllPlayersState(PlayerState.Blocked);
            GameManager.Instance.PlayerMng.DestroyAllAvatar();
            if (EventManager.OnMusicChange != null)
                EventManager.OnMusicChange(AudioManager.Music.GameTheme, true);
        }

        public override void OnUpdate() {
            if (OnStateEnd != null)
                OnStateEnd();
        }
    }
}
