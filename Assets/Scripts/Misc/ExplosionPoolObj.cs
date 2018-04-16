using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{

    public class ExplosionPoolObj : MonoBehaviour, IPoollableObject
    {
        ParticleSystem ExplosionParticles { get { return GetComponent<ParticleSystem>(); } }

        public PoolManager poolManager { get; set; }

        public GameObject GameObject { get { return gameObject; } }

        public bool IsActive { get; set; }

        public void Activate()
        {
            IsActive = true;
            if (ExplosionParticles.isPlaying)
                ExplosionParticles.Stop();
            ExplosionParticles.Play();
        }

        public void Deactivate()
        {
            IsActive = false;
            poolManager.ReleasedPooledObject(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsActive)
                return;
            if (!ExplosionParticles.isPlaying)
                Deactivate();
        }
    }
}