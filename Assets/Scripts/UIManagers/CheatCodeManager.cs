using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{
    public class CheatCodeManager : MonoBehaviour
    {
        public GameObject CheatPanel;
        InputField inputField;

        private void Start()
        {
            inputField = GetComponentInChildren<InputField>();
            inputField.DeactivateInputField();
            CheatPanel.SetActive(false);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ActiveInputField();
            }
        }

        /// <summary>
        /// Azione da compiere dopo aver inserito una stringa nell'input field
        /// </summary>
        /// <param name="_cheat"></param>
        public void GetInput(string _cheat)
        {
            switch (_cheat)
            {
                case "ammo":
                    foreach (Player player in GameManager.Instance.PlayerMng.Players)
                    {
                        player.Avatar.ship.shooter.AmmoCheat();
                    }
                    Debug.Log("Infinite Ammo");
                    break;
                case "round":
                    GameManager.Instance.LevelMng.CheatCodeRoundEnd();
                    GameManager.Instance.LevelMng.UpgradePointsMng.CheatPoints(PlayerLabel.Different);
                    break;
                case "level":
                    GameManager.Instance.LevelMng.gameplaySM.SetPassThroughOrder(new List<StateBase>() { new CleanSceneState(), new GameOverState() });
                    break;
                case "damage":
                    foreach (Player player in GameManager.Instance.PlayerMng.Players)
                    {
                        player.Avatar.ship.shooter.DamageCheat();
                    }
                    break;
                case "NoBounds":
                    foreach (Player player in GameManager.Instance.PlayerMng.Players)
                    {
                        switch (player.ID)
                        {
                            case PlayerLabel.One:
                                player.Avatar.SetNewCollisionLayers(8, 9);
                                break;
                            case PlayerLabel.Two:
                                player.Avatar.SetNewCollisionLayers(10, 11);
                                break;
                            case PlayerLabel.Three:
                                player.Avatar.SetNewCollisionLayers(12, 13);
                                break;
                            case PlayerLabel.Four:
                                player.Avatar.SetNewCollisionLayers(14, 15);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "coins":
                    GameManager.Instance.CoinMng.TotalCoin += 100;
                    break;
                case "reset":
                    DataReset();
                    break;
                default:
                    Debug.LogWarning("Wrong CheatCode");
                    break;
            }

            inputField.text = "";
            inputField.DeactivateInputField();
            CheatPanel.SetActive(false);
        }

        void DataReset()
        {
            GameManager.Instance.DataMng.DataReset();
            GameManager.Instance.CoinMng.TotalCoin = 0;
            GameManager.Instance.SRMng.ResetShowRooms();
            GameManager.Instance.ShopRoomMng.ResetShowRooms();

            PlayerPrefs.Save();
        }

        /// <summary>
        /// Attiva e seleziona la casella di testo dell'input field
        /// </summary>
        void ActiveInputField()
        {
            CheatPanel.SetActive(true);
            inputField.Select();
            inputField.ActivateInputField();
        }
    }
}