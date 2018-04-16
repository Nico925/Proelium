using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class CoinManager : MonoBehaviour
    {

        public GameObject CoinControllerPrefab;

        [HideInInspector]
        public RoundCoinController CoinController;

        public float CoinLife = 10;

        private int _totalCoin = -1;

        private int coinSaved { get { return PlayerPrefs.GetInt("Coins"); } }

        public int TotalCoin
        {
            get {
                if(_totalCoin < 0)
                {
                    if (PlayerPrefs.HasKey("Coins"))
                        _totalCoin = PlayerPrefs.GetInt("Coins");
                    else
                        _totalCoin = 0;
                }
                return _totalCoin;
            }
            set {
                _totalCoin = value;
                PlayerPrefs.SetInt("Coins", _totalCoin);
            }
        }
                
        public void InstantiateCoinController()
        {
            CoinController = Instantiate(CoinControllerPrefab, transform).GetComponent<RoundCoinController>();
            CoinController.Init(this, CoinLife);
        }
        
        /// <summary>
        ///dove segue ho messo degli appunti per ricordare per quando si farà il boss e lo spawn in altri casi delle monete 
        /// </summary>


        //void Spawncoin(GameObject coinprefab, Transform.position){ }

        //Instantiate(coinprefab, position);


        //void bossdefeated(){ }
        // Spawncoin
    }
}