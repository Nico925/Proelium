using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlackFox
{
    public class ExternalAgent : MonoBehaviour, IDamageable
    {
        Transform target;
        public float life = 10;
        public float velocity = 5;
        public float damage = 1;
        AlertIndicator alertIndicator;
        public GameObject particleSistem;

        List<IDamageable> damageablesList;
        AudioSource source;

        public float Life
        {
            get { return life; }
            set { life = value; }
        }

        private void Start()
        {
            Vector3 desiredDirection = transform.position - GameManager.Instance.LevelMng.Core.transform.position;
            source = GetComponent<AudioSource>();
            source.clip = GameManager.Instance.AudioMng.ExternalAgentsAudio.ExternalAgentMovement.Clip;
            source.volume = GameManager.Instance.AudioMng.ExternalAgentsAudio.ExternalAgentMovement.Volume;
            if (Vector3.Cross(desiredDirection, transform.forward).magnitude > .1f)
                Destroy(gameObject);
        }

        private void Update()
        {
            MoveTowards();
            PlayMovemntSound(true);
        }

        void PlayMovemntSound(bool _toggle)
        {
            if (_toggle)
            {
                if (!source.isPlaying && source.clip != null)
                    source.Play();
            }
            else
                source.Stop();
        }


        void PlayCollisionSound()
        {
            source.clip = GameManager.Instance.AudioMng.ExternalAgentsAudio.Collisions[Random.Range(0, GameManager.Instance.AudioMng.ExternalAgentsAudio.Collisions.Count)].Clip;
            source.volume = GameManager.Instance.AudioMng.ExternalAgentsAudio.Collisions[Random.Range(0, GameManager.Instance.AudioMng.ExternalAgentsAudio.Collisions.Count)].Volume;

            if (!source.isPlaying && source.clip != null)
                source.Play();
        }


        void MoveTowards()
        {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * velocity, ForceMode.Acceleration);
        }

        public void Initialize(Transform _target, List<IDamageable> _damageables)
        {
            target = _target;
            damageablesList = _damageables;
            alertIndicator = GetComponent<AlertIndicator>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            GetComponentInChildren<ParticleSystem>().Stop();
            if (collision.gameObject.GetComponent<ExternalAgent>() != null)
            { 
                Deactivate();
                PlayCollisionSound();
                Destroy(gameObject);
            }
            // Controlla se l'oggetto con cui ha colliso ha l'interfaccia IDamageable e salva un riferimento di tale interfaccia
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                //Controlla se all'interno della lista di oggetti Danneggiabili, passata da SpawnExternalAgent
                foreach (IDamageable item in damageablesList)
                {
                    // E' presente l'oggetto con cui l'agente esterno è entrato in collisione.
                    if (item.GetType() == damageable.GetType())
                    {
                        if (collision.gameObject.GetComponent<Ship>() != null) {
                            GameManager.Instance.CoinMng.CoinController.InstantiateCoin(transform.position);
                        }
                        Deactivate();
                        PlayCollisionSound();
                        damageable.Damage(damage, gameObject);        // Se è un oggetto che può danneggiare, richiama la funzione che lo danneggia
                        Destroy(gameObject);                    //Distrugge l'agente esterno
                        break;                                  // Ed esce dal foreach.
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Wall")
                GetComponent<Collider>().isTrigger = false;
        }

        #region Interface

        public void Damage(float _damage, GameObject _attacker)
        {
            Life -= _damage;
            transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
            if (Life < 1)
            {
                Deactivate();
                GameManager.Instance.CoinMng.CoinController.InstantiateCoin(transform.position);
                transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => { Destroy(gameObject); });
            }
        }

        void Deactivate()
        {
            PlayMovemntSound(false);
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(Instantiate(particleSistem, transform.position, Quaternion.identity), 4);
        }

        #endregion
    }
}

