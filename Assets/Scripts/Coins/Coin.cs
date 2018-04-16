using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {

    public class Coin : MonoBehaviour {

        float lifeTime;

        RoundCoinController coinController;
        AudioSource source;

        public void Init(RoundCoinController _controller, float _coinLife)
        {
            coinController = _controller;
            source = GetComponent<AudioSource>();
            lifeTime = _coinLife;
        }


        private void Update()
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Ship>() != null)
            {
                coinController.CoinCollected++;
                if (EventManager.OnGameAction != null)
                    EventManager.OnGameAction(AudioManager.AudioInGame.CoinCollected);
                Destroy(gameObject);
            }
        }
    }
}