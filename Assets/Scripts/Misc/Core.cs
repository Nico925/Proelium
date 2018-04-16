using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BlackFox
{
    public class Core : MonoBehaviour, IDamageable
    {
        float life;
        public float MaxLife = 10;      // La vita massima che può avere il Core e che viene impostata al riavvio di un round perso

        public ParticleSystem Particles;
        Image Ring;

        Tweener damageTween;

        private ParticlesController _particlesController;

        public ParticlesController ParticlesController
        {
            get
            {
                if (_particlesController != null)
                    return _particlesController;
                else return null;
            }
            set { _particlesController = value; }
        }

        public void Setup()
        {
            Ring = GetComponentInChildren<Image>();
            life = MaxLife;
            ParticlesController = GetComponent<ParticlesController>();
            OnDataChange();
        }

        void OnDataChange()
        {
            Ring.fillAmount = life / MaxLife;

            if (Ring.fillAmount < 0.3f)
            {
                Ring.color = Color.red;
            }
            else if (Ring.fillAmount > 0.7f)
            {
                Ring.color = Color.green;
            }
            else
            {
                Ring.color = Color.yellow;
            }
        }

        #region API
        public void Init()
        {
            if (damageTween != null)
                damageTween.Complete();
            damageTween = transform.DOScale(Vector3.one, 0.1f);
            if (life <= 0)
            {
                life = MaxLife;
                OnDataChange();
            }
        }

        public bool IsCoreAlive() {
            if (life < 1)
                return false;
            else
                return true;
        }
        #endregion

        #region Interfacce

        public void Damage(float _damage, GameObject _attacker)
        {
            life -= _damage;
            OnDataChange();
            if (damageTween != null)
                damageTween.Complete();
            damageTween = transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f);
            //DamageParticles.transform.position = new Vector3(DamageParticles.transform.position.x + UnityEngine.Random.Range(0.1f, 0.5f), 
            //  DamageParticles.transform.position.y + UnityEngine.Random.Range(0.1f, 0.5f), DamageParticles.transform.position.z + UnityEngine.Random.Range(0.1f, 0.5f));
            //ParticlesController.PlayParticles(ParticlesController.ParticlesType.Damage);

            if (!IsCoreAlive())
            {
                if(!Particles.isPlaying)
                    StartCoroutine(CoreExplosionEffect());
            }
        }

        IEnumerator CoreExplosionEffect()
        {
            Particles.Play();
            yield return new WaitForSeconds(Particles.main.duration);
            GameManager.Instance.LevelMng.CheckRoundStatus();
        }

        #endregion
    }
}