using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{

    public class ManualController : BaseMenu
    {
        int _currentImgSelectet = 0;

        public int CurrentImgSelected {
            get { return _currentImgSelectet; }
            set {
                _currentImgSelectet = value;

                if (CurrentImgSelected > ManualImages.Count - 1)
                    CurrentImgSelected = 0;

                if (CurrentImgSelected < 0)
                    CurrentImgSelected = ManualImages.Count - 1;

                manualImg.sprite = ManualImages[_currentImgSelectet];
            }
        }

        public List<Sprite> ManualImages = new List<Sprite>();
        Image manualImg;

        private void Start()
        {
            manualImg = GetComponentInChildren<Image>();
        }

        public override void GoRightInMenu(Player _player)
        {
            CurrentImgSelected++;
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void GoLeftInMenu(Player _player)
        {
            CurrentImgSelected--;
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

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