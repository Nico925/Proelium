using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{
    public class MainMenuController : BaseMenu
    {

        public Image Panel;
        float tempa = 1;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                /// sfuma l'alfa del panello che contiene i bottoni ( i bottoni non vengono sfumati )
                tempa = tempa - Time.deltaTime ;
                Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, tempa);
            }
        }


        public void Init()
        {
            GameManager.Instance.UiMng.CurrentMenu = this;
            FindISelectableChildren();
            foreach (ISelectable button in SelectableButtons)
            {
                (button as SelectableButton).Init(GameManager.Instance.UiMng.SelectedButton, GameManager.Instance.UiMng.DeselectionButton);
            }

            SelectableButtons[0].IsSelected = true;
        }

        public override void GoDownInMenu(Player _player)
        {
            base.GoDownInMenu(_player);
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void GoUpInMenu(Player _player)
        {
            base.GoUpInMenu(_player);
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void Selection(Player _player)
        {
            GameManager.Instance.PlayerMng.ChangeAllPlayersState(PlayerState.Blocked);
            GameManager.Instance.LoadingCtrl.ActivateLoadingPanel(() => {

                switch (CurrentIndexSelection)
                {
                    
                    case 0:
                        GameManager.Instance.flowSM.SetPassThroughOrder(new List<StateBase>() { new AvatarSelectionState() });
                        break;
                    case 1:
                        GameManager.Instance.flowSM.SetPassThroughOrder(new List<StateBase>() { new StoreState() });
                        break;
                    case 2:
                        GameManager.Instance.flowSM.SetPassThroughOrder(new List<StateBase>() { new ManualState() });
                        break;
                    case 3:
                        GameManager.Instance.flowSM.SetPassThroughOrder(new List<StateBase>() { new CreditsState() });
                        break;
                    case 4:
                        GameManager.Instance.QuitApplication();
                        break;
                }
            });

            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Selection);
        }
    }
}