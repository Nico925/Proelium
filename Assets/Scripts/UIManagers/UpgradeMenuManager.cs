using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox { 
    public class UpgradeMenuManager : BaseMenu
    {
        public Sprite ActiveSlide;
        public Sprite DeactiveSlider;

        GameObject _upgradePanel;
        public GameObject UpgradePanel {
            get {
                if (_upgradePanel == null)
                {
                    Image[] tempList = GetComponentsInChildren<Image>();
                    foreach (Image item in tempList)
                    {
                        if (item.name == "UpgradeMenuSubPanel")
                            _upgradePanel = item.gameObject;
                    }
                }
                return _upgradePanel;
            }
            set { _upgradePanel = value;
                Debug.Log("");
            }
        }

        public List<PlayerUpgradeController> PlayerUpgradeControllers = new List<PlayerUpgradeController>();

        public void Setup(List<Player> _players)
        {
            foreach (Player player in _players)
            {
                foreach (PlayerUpgradeController controller in PlayerUpgradeControllers)
                {
                    if ((int)player.ID == (int)controller.MenuID)
                    {
                        controller.Setup(this, player);
                        controller.FindISelectableChildren();
                        break;
                    }
                }
            }
            UpgradePanel.SetActive(false);
        }

        public void InitControllers()
        { 
            foreach (PlayerUpgradeController controller in PlayerUpgradeControllers)
                controller.Init();
        }

        public void CheckUpgradeControllersState()
        {
            foreach (PlayerUpgradeController controller in PlayerUpgradeControllers)
            {
                if (controller.CurrentState == UpgradeControllerState.Unready)
                    return;
            }
            GameManager.Instance.LoadingCtrl.ActivateLoadingPanel(() => {
                GameManager.Instance.LevelMng.gameplaySM.CurrentState.OnStateEnd();
            });           
        }

        #region Menu Actions
        public override void GoUpInMenu(Player _player)
        {
            foreach (PlayerUpgradeController controller in PlayerUpgradeControllers)
            {
                if ((int)controller.MenuID == (int)_player.ID)
                {
                    controller.GoUpInMenu(_player);
                    break;
                }
            }

            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void GoDownInMenu(Player _player)
        {
            foreach (PlayerUpgradeController controller in PlayerUpgradeControllers)
            {
                if ((int)controller.MenuID == (int)_player.ID)
                {
                    controller.GoDownInMenu(_player);
                    break;
                }
            }

            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void GoLeftInMenu(Player _player)
        {
            foreach (PlayerUpgradeController controller in PlayerUpgradeControllers)
            {
                if ((int)controller.MenuID == (int)_player.ID)
                {
                    controller.GoLeftInMenu(_player);
                    break;
                }
            }

            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void GoRightInMenu(Player _player)
        {
            foreach (PlayerUpgradeController controller in PlayerUpgradeControllers)
            {
                if ((int)controller.MenuID == (int)_player.ID)
                {
                    controller.GoRightInMenu(_player);
                    break;
                }
            }

            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void Selection(Player _player)
        {
            foreach (PlayerUpgradeController controller in PlayerUpgradeControllers)
            {
                if ((int)controller.MenuID == (int)_player.ID)
                {
                    controller.Selection(_player);
                    break;
                }
            }

            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Selection);
        }
        #endregion
    }
}