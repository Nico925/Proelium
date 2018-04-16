using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{
    public class GameUIController : MonoBehaviour
    {
        List<PlayerHud> Huds = new List<PlayerHud>();

        public Image kamikazePowerUpAlert;
        float timer = 0;
        public Text CoinCollectedText;

        List<GameObject> getHudPlayers()
        {
            List<GameObject> tempHudPlayers = new List<GameObject>();
            foreach (RectTransform item in GetComponentsInChildren<RectTransform>())
            {
                if(item.tag == "HUDPlayer")
                {
                    tempHudPlayers.Add(item.gameObject);
                }
            }
            return tempHudPlayers;
        }


        #region API

        public void Init()
        {
            kamikazePowerUpAlert.color = new Color(kamikazePowerUpAlert.color.r, kamikazePowerUpAlert.color.g, kamikazePowerUpAlert.color.b, 0);
            List<Player> players = GameManager.Instance.PlayerMng.Players;
            List<GameObject> HudPlayer = getHudPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                PlayerHud temphud = new PlayerHud();
                temphud.player = players[i];
                temphud.Hud = HudPlayer[i];
                temphud.GetImage().sprite = players[i].AvatarData.ColorSets[players[i].AvatarData.ColorSetIndex].HudColor;
                Huds.Add(temphud);
            }
        }

        /// <summary>
        /// Quando viene richiamata va a leggere i punti del player richiesto nel levelMng e li aggiorna nella UI.
        /// </summary>
        /// <param name="_player">Il giocatore a cui aggiornare i punti uccisione nella Ui</param>
        public void SetKillPointsUI(Player _player)
        {
            for (int i = 0; i < Huds.Count; i++)
            {
                if (Huds[i].player == _player)
                    Huds[i].GetText().text = GameManager.Instance.LevelMng.GetPlayerKillPoints(_player.ID).ToString();
            }
        }

        public void ResetKillPointsUI()
        {
            for (int i = 0; i < Huds.Count; i++)
                Huds[i].GetText().text = "0";
        }

        /// <summary>
        /// Popup per visualizzare la scritta "Kamikaze"
        /// </summary>
        public void RunKamikazeAlert()
        {
            kamikazePowerUpAlert.DOFade(2, 1).OnComplete(() => { kamikazePowerUpAlert.DOFade(-1, 1); });
        }

        #endregion

        class PlayerHud
        {
            public Player player;
            public GameObject Hud;

            public Text GetText()
            {
                return Hud.GetComponentInChildren<Text>();
            }

            public Image GetImage()
            {
                return Hud.GetComponentInChildren<Image>();
            }

        }

    }
}
