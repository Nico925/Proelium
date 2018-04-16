using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class PlayState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("PlayState");
            if (EventManager.OnMusicChange != null)
                EventManager.OnMusicChange(AudioManager.Music.GameTheme, true);
            GameManager.Instance.LevelMng.SpawnerMng.ToggleSpawners(true);
            GameManager.Instance.PlayerMng.ChangeAllPlayersState(PlayerState.PlayInput);
            GameManager.Instance.LevelMng.RoundBegin();
            GameManager.Instance.LevelMng.PowerUpMng.Toggle(true);
            EventManager.OnAgentKilled += HandleOnAgentKilled;
        }

        public override void OnEnd()
        {
            // passaggio informazioni essenziali al gestore del livello
            EventManager.OnAgentKilled -= HandleOnAgentKilled;
        }

        #region Events Handler

        void HandleOnAgentKilled(Avatar _killer, Avatar _victim)
        {
            GameManager.Instance.LevelMng.UpdateKillPoints(_killer, _victim);
            if(_killer != null)
            {
                GameManager.Instance.UiMng.canvasGame.gameUIController.SetKillPointsUI(_killer.Player);
                _killer.avatarUI.KillView();
            }
            GameManager.Instance.UiMng.canvasGame.gameUIController.SetKillPointsUI(_victim.Player);
            if (GameManager.Instance.LevelMng.IsRoundActive)
                GameManager.Instance.LevelMng.AvatarSpwn.SpawnAvatar(_victim.Player, 3);

        }
        #endregion
    }
}
