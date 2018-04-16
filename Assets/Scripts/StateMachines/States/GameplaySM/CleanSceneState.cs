using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    public class CleanSceneState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("CleanSceneState");
            GameManager.Instance.LevelMng.CleanPins();
            GameManager.Instance.LevelMng.PowerUpMng.CleanSpawned();
            GameManager.Instance.LevelMng.SpawnerMng.CleanSpawnersSpawnedElements();
            GameManager.Instance.PlayerMng.ChangeAvatarsState(AvatarState.Disabled);         
        }

        public override void OnUpdate()
        {
            if (OnStateEnd != null)
                OnStateEnd();
        }
    }
}
