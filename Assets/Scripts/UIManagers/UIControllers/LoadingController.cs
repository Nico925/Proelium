using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BlackFox {
    public class LoadingController : MonoBehaviour {

        public Image panelImg;
        public float FadeInTime = .4f;
        public float FadeOutTime = .3f;
        #region API

        public void ActivateLoadingPanel(TweenCallback _action = null) {
            if (_action != null)
                panelImg.DOFade(1.2f, FadeInTime).OnComplete(_action);
            else
                panelImg.DOFade(1.2f, FadeInTime);
        }

        public void DeactivateLoadingPanel(TweenCallback _action = null) {
            if (_action != null)
                panelImg.DOFade(0, FadeOutTime).OnComplete(_action);
            else
                panelImg.DOFade(0, FadeOutTime);
        }

        #endregion


    }
}