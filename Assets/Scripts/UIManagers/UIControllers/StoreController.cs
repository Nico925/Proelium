using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace BlackFox
{
    public class StoreController : BaseMenu
    {
        ShowRoomController room;
        List<Slider> sliders;
        public Image Chain;
        Text price;
        public Text CoinText;
        public Text PriceText;

        public void Init()
        {
            room = GameManager.Instance.ShopRoomMng.rooms[0];
            sliders = GetComponentsInChildren<Slider>().ToList();
            SetSliderValues(GameManager.Instance.ShopRoomMng.datas[0].SelectionParameters[0], null);
            price = Chain.GetComponentInChildren<Text>();
            Chain.gameObject.SetActive(false);
            CoinText.text = GameManager.Instance.CoinMng.TotalCoin.ToString();
        }
        
        #region ShowRoom Events

        private void OnEnable()
        {
            EventManager.OnShowRoomValueUpdate += SetSliderValues;
        }

        private void OnDisable()
        {
            EventManager.OnShowRoomValueUpdate -= SetSliderValues;
        }

        /// <summary>
        /// Passa i valori delle slider al controller giusto
        /// </summary>
        /// <param name="_values"></param>
        /// <param name="_player"></param>
        void SetSliderValues(int[] _values, Player _player)
        {
            for (int i = 0; i < sliders.Count; i++)
            {
                sliders[i].value = _values[i];
            }
        }

        #endregion

        #region Menu Actions

        public override void GoRightInMenu(Player _player)
        {
            room.ShowNextModel(true);
            ToggleChain();
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void GoLeftInMenu(Player _player)
        {
            room.ShowPreviousModel(true);
            ToggleChain();
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void GoUpInMenu(Player _player)
        {
            room.ShowNextColor(true);
            ToggleChain();
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void GoDownInMenu(Player _player)
        {
            room.ShowPreviousColor(true);
            ToggleChain();
            if (EventManager.OnMenuAction != null)
                EventManager.OnMenuAction(AudioManager.UIAudio.Movement);
        }

        public override void Selection(Player _player)
        {

            if (!GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].IsPurchased && (GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].Price <= GameManager.Instance.CoinMng.TotalCoin))
            {
                GameManager.Instance.DataMng.PurchaseAvatar(GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent]);
                GameManager.Instance.CoinMng.TotalCoin -= GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].Price;
                Chain.gameObject.SetActive(false);
                if (EventManager.OnMenuAction != null)
                    EventManager.OnMenuAction(AudioManager.UIAudio.Selection);
            }
            else if (!GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].ColorSets[room.colorIndex].IsPurchased && (GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].ColorSets[room.colorIndex].Price <= GameManager.Instance.CoinMng.TotalCoin))
            {
                GameManager.Instance.DataMng.PurchaseColorSet(GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].ColorSets[room.colorIndex]);
                GameManager.Instance.CoinMng.TotalCoin -= GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].ColorSets[room.colorIndex].Price;
                Chain.gameObject.SetActive(false);
                if (EventManager.OnMenuAction != null)
                    EventManager.OnMenuAction(AudioManager.UIAudio.Selection);
            }

            CoinText.text = GameManager.Instance.CoinMng.TotalCoin.ToString();
            PlayerPrefs.Save();
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
        #endregion
        

        void ToggleChain()
        {
            if (GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].ColorSets[room.colorIndex].IsPurchased && GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].IsPurchased)
            {
                Chain.gameObject.SetActive(false);
            }
            else {
                Chain.gameObject.SetActive(true);
                if(!GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].IsPurchased)
                    PriceText.text = "Ship Price : " + (GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].Price).ToString();
                else
                    PriceText.text = "Skin Price : " +(GameManager.Instance.ShopRoomMng.datas[room.IndexOfCurrent].ColorSets[room.colorIndex].Price).ToString();

            }
        }
    }
}