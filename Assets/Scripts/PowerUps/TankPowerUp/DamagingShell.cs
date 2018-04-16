using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    public class DamagingShell : MonoBehaviour {

        List<IDamageable> _damageables = new List<IDamageable>();
        float duration = 0;
        float damage;

		ParticleSystem ParticlesPrefab;
		ParticleSystem particles;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_time">la durata del power up</param>
        public void Init(float _time, float _damage)
        {
            _damageables = GetComponent<Ship>().GetDamageable();
            duration = _time;
            damage = _damage;
			ParticlesPrefab = Resources.Load<ParticleSystem> ("Prefabs/Particles/PowerUpParticles/Carro armato");
			particles = Instantiate (ParticlesPrefab, transform);
        }

        private void Update()
        {
            if(duration > 0 && GetComponent<Ship>().Avatar.State == AvatarState.Enabled) { 
                duration -= Time.deltaTime;
				if (duration <= 0) {
					Destroy (this); 
					if (particles != null)
						Destroy (particles);
				}
            } else
            {
                Destroy(this);
                if (particles != null)
                    Destroy(particles);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            IDamageable collidingDamageable = collision.gameObject.GetComponent<IDamageable>();
            if (collidingDamageable != null)
            {
                foreach (IDamageable damageable in _damageables)
                {
                    if (damageable.GetType() == collidingDamageable.GetType())
                        collidingDamageable.Damage(damage, gameObject);
                }
            }
        }
    }
}