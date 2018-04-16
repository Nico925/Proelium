using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class PreStartState : StateBase
    {
        public override void OnStart()
        {
            Debug.Log("PreStartState");
            GameManager.Instance.UiMng.SetRoundImage(GameManager.Instance.LevelMng.RoundNumber);
            GameManager.Instance.LoadingCtrl.DeactivateLoadingPanel(()=> {
                GameManager.Instance.UiMng.canvasGame.Counter.DoCountDown();
            });
        }
    }
}