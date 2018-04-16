using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{
    public class PauseMenuController : BaseMenu
    {        
        public GameObject ChildrenPanel;

        private void Start()
        {
            ChildrenPanel.SetActive(false);
            FindISelectableChildren();
        }

        public void Init()
        {
            if(SelectableButtons.Count == 0)
                FindISelectableChildren();
            foreach (ISelectable button in SelectableButtons)
            {
                (button as SelectableButton).Init(GameManager.Instance.UiMng.SelectedButton, GameManager.Instance.UiMng.DeselectionButton);
            }

            SelectableButtons[0].IsSelected = true;
        }

        public override void GoDownInMenu(Player _player)
        {
            Debug.Log(SelectableButtons[CurrentIndexSelection]);
            base.GoDownInMenu(_player);
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void GoUpInMenu(Player _player)
        {
            Debug.Log(SelectableButtons[CurrentIndexSelection]);
            base.GoUpInMenu(_player);
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void Selection(Player _player)
        {
            switch (CurrentIndexSelection)
            {
                case 0:
                    GameManager.Instance.LevelMng.gameplaySM.CurrentState.OnStateEnd();
                    break;
                case 1:
                    GameManager.Instance.LevelMng.gameplaySM.SetPassThroughOrder(new List<StateBase>() { new CleanSceneState(), new GameOverState() });
                    break;
            }

            CurrentIndexSelection = 0;

            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Selection);
        }
    }
}