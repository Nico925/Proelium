using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class ElementZero : MonoBehaviour, IDamageable
    {

        public float Life = 5;
        public float MaxLife = 15;
        Core core;
        public float DamageToCore = 1f;
        public float WhenDamage = 3;
        float TimeToWaitToDamage;
        float TimeToRecharge;
        bool CanDamageCore;
        bool CanRegenerateLife;
        GameManager gameManager;
        float RandomNum;
        public bool Round3 = false;

        // Use this for initialization
        void Start()
        {
            gameManager = GameManager.Instance;
            core = FindObjectOfType<Core>();
            TimeToWaitToDamage = WhenDamage;
            TimeToRecharge = WhenDamage;
            CanDamageCore = false;
            CanRegenerateLife = true;

            if (Round3 == false)
            {
                RandomNum = UnityEngine.Random.Range(0f, 1f);
                if (RandomNum < 0.5)
                {
                    transform.position = new Vector3(transform.position.x * -1, transform.position.y, transform.position.z);
                }
            }

            //if (gameManager.GetGameUIController() != null)
            //    gameManager.ElementZeroValueUpdate(Life, MaxLife);
        }


        // Update is called once per frame
        void Update()
        {
            //Aggiorna costantemente la sua slider
            //if (gameManager.GetGameUIController() != null)
            //    gameManager.ElementZeroValueUpdate(Life, MaxLife);

            // Se CanRegenerateLife è vera può ricaricarsi la vita
            if (CanRegenerateLife == true)
            {
                ChargeLife();
            }
            // altrimenti comincia un timer, allo scadere del tempo CanRegenerateLife ritorna vera permettendogli di rigenerarsi la vita (se la vita non è già al massimo)
            else
            {
                if (Life < MaxLife)
                {
                    TimeToRecharge -= Time.deltaTime;
                    if (TimeToRecharge <= 0)
                    {
                        CanRegenerateLife = true;
                        TimeToRecharge = WhenDamage;
                    }
                }
            }

            // controlla se è il momento di danneggiare il core
            if (CanDamageCore == true)
            {
                DamageCore();
            }

        }

        /// <summary>
        /// Funzione che rigenera la vita la vita dell'elemento zero nel tempo e quando arriva al massimo della vita infligge danno al core
        /// </summary>
        void ChargeLife()
        {
            Life = Life + 1 * Time.deltaTime * 0.3f;

            //Quando la vita è al massimo infligge danno al core
            if (Life >= MaxLife)
            {
                CanDamageCore = true;
                Life = MaxLife;
                CanRegenerateLife = false;
            }
            else
            {
                CanDamageCore = false;
            }
        }

        /// <summary>
        /// Funzione che aspetta lo scadere di un timer per poi infliggere danno al core
        /// </summary>
        void DamageCore()
        {
            TimeToWaitToDamage -= Time.deltaTime;
            if (TimeToWaitToDamage <= 0)
            {
                core.Damage(DamageToCore, gameObject);
                TimeToWaitToDamage = WhenDamage;
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Avatar>() != null)
            {
                Damage(DamageToCore * 2, gameObject);
            }
        }

        #region Interfacce

        #region IDamageable
        /// <summary>
        /// Procura danno all'elemento zero
        /// </summary>
        /// <param name="_damage">Quanto danno deve subire</param>
        /// <param name="_attacker">Chi glielo procura</param>
        public void Damage(float _damage, GameObject _attacker)
        {
            Life -= _damage;

            // Quando la vita arriva a 0 dannegga una prima volta il Core e setta la variabile CanDamageCore a true

            if (Life <= 0)
            {
                core.Damage(DamageToCore, gameObject);
                CanDamageCore = true;
                Life = 0;
            }
            //altrimenti resetta i timer per danneggiare il core o ricaricarsi la vita
            else
            {
                CanDamageCore = false;
                TimeToWaitToDamage = WhenDamage;
                TimeToRecharge = WhenDamage;
            }
            //blocca temporaneamente la rigenerazione della vita
            CanRegenerateLife = false;
        }
        #endregion

        #endregion

    }
}