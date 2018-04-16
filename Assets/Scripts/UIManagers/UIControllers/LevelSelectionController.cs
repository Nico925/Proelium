using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BlackFox
{
    public class LevelSelectionController : BaseMenu
    {

        void Start()
        {
            //FindISelectableChildren();
            GameManager.Instance.UiMng.CurrentMenu = this;
        }

        #region Menu Actions
        public override void GoDownInMenu(Player _player) { }

        public override void GoUpInMenu(Player _player) { }

        public override void GoLeftInMenu(Player _player)
        {
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
            base.GoUpInMenu(_player);
        }

        public override void GoRightInMenu(Player _player)
        {
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
            base.GoDownInMenu(_player);
        }

        public override void Selection(Player _player)
        {
            GameManager.Instance.PlayerMng.ChangeAllPlayersState(PlayerState.Blocked);
            GameManager.Instance.LoadingCtrl.ActivateLoadingPanel(() => {

                switch (CurrentIndexSelection)
                {
                    case 0:
                        GameManager.Instance.SelectLevel(0);
                        break;
                    default:
                        break;
                }
                GameManager.Instance.flowSM.SetPassThroughOrder(new List<StateBase>() { new AvatarSelectionState() });
            });

            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Selection);
        }

        public override void GoBack(Player _player)
        {
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Back);
            GameManager.Instance.LoadingCtrl.ActivateLoadingPanel(() => {
                GameManager.Instance.flowSM.SetPassThroughOrder(new List<StateBase>() { new MainMenuState() });
            });
        }
        #endregion
    }
}