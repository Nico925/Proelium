using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{
    public class EndRoundlUI : BaseMenu
    {
        public Sprite VictoryImg;
        public Sprite DefeatImg;
        public Sprite TotalVictory;
        public Image RecapImage;

        public Text[] PlayerPoints = new Text[4];
        public Text[] PlayersText = new Text[4];
        public Text ActionText;
        public GameObject BackGround;
        public GameObject EndLevelPanel;

        bool CanSelect = false;

        void Start()
        {
            BackGround.SetActive(false);
        }

        /// <summary>
        /// Cerca il totale di ogni player e lo mostra in una casella di testo ordinato per numero di uccisioni
        /// </summary>
        void ShowAvatarsKillPoints()
        {
            List<PlayerStats> pointsList = GameManager.Instance.LevelMng.GetPlayerKillPointsInOrderDesc();

            for (int i = 0; i < PlayersText.Length || i < PlayerPoints.Length; i++)
            {
                PlayersText[i].text = "PLAYER " + (int)pointsList[i].Player.ID;
                PlayerPoints[i].text = pointsList[i].KillPoints.ToString();
                if (pointsList[i].KillPoints == GameManager.Instance.LevelMng.levelOptions.PointsToWin)
                {
                    PlayersText[i].color = Color.yellow;
                    PlayerPoints[i].color = Color.yellow;
                }
            }
        }

        IEnumerator Wait(float _timeToWait)
        {
            yield return new WaitForSeconds(_timeToWait);
            ActionText.text = "Press RT to continue";
            CanSelect = true;
        }
        
        #region API
        /// <summary>
        /// Attiva End Round Panel e mostrare i punti degli avatar
        /// </summary>
        public void SetEndRoundPanelStatus(bool _status)
        {
            if (_status)
            {
                ShowAvatarsKillPoints();
                StartCoroutine(Wait(2f));
            }
            BackGround.SetActive(_status);
        }

        public void ActiveTotalVictory()
        {
            BackGround.SetActive(true);
            RecapImage.gameObject.SetActive(true);
            PlayersText[0].GetComponentsInParent<Transform>()[1].gameObject.SetActive(false);
            CanSelect = true;
        }

        public override void Selection(Player _player)
        {
            if (CanSelect)
            {
                GameManager.Instance.LoadingCtrl.ActivateLoadingPanel(() => {
                    GameManager.Instance.LevelMng.gameplaySM.CurrentState.OnStateEnd();
                });
                if (EventManager.OnMenuAction != null)
                    EventManager.OnMenuAction(AudioManager.UIAudio.Selection);
                CanSelect = false;
                ActionText.text = "";
            }
        }

        /// <summary>
        /// Cambia l'immagine del drappo da visualizzare
        /// </summary>
        /// <param name="_string">"Victory" per impostare il drappo di vittoria. "Defeat" per impostare il drappo di sconfitta</param>
        public void SetRecapImage(string _string)
        {
            switch (_string)
            {
                case "Victory":
                    RecapImage.sprite = VictoryImg;
                    break;
                case "Defeat":
                    RecapImage.sprite = DefeatImg;
                    break;
                default:
                    break;
            }
        }

        public void SetTotalVictoryImage(Player _winner)
        {
            RecapImage.sprite = TotalVictory;
            RecapImage.GetComponentInChildren<Text>().text = "Player " +  _winner.ID;
        }

        #endregion
    }
}