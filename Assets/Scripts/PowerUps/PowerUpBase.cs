using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    public abstract class PowerUpBase : MonoBehaviour, IPowerUp {

        public PowerUpID ID;
        public bool AutoUse = false;
        public float PowerUpDuration = 10;
        public float SpawnRatio;

        protected IPowerUpCollector collector;
        protected List<IPowerUpCollector> enemyCollectors = new List<IPowerUpCollector>();
        protected AudioSource audioSurce;

        /// <summary>
        /// Variabile che determina se il powerup deve essere distrutto una volta raccolto o meno.
        /// </summary>
        protected bool DestroyAfterUse = true;

        private float _lifeTime = 10;
        public float LifeTime
        {
            get { return _lifeTime; }
            set { _lifeTime = value; }
        }

        protected virtual void NotifyCollect(IPowerUpCollector _collector) {
            _collector.CollectPowerUp(this);
        }

        public abstract void UsePowerUp();

        private void Start()
        {
            Init();
        }

        protected virtual void Init() { }

        private void Update()
        {
            LifeTime -= Time.deltaTime;
            if (LifeTime <= 0)
                Destroy(gameObject); 
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponentInParent<IPowerUpCollector>() == null)
                return;

            collector = other.GetComponentInParent<IPowerUpCollector>();
            PowerUpDuration = (collector as Avatar).GetUpgrade(UpgardeTypes.PowerUpDuration).CalculateValue(PowerUpDuration);
            foreach (Player player in other.GetComponentInParent<Avatar>().Enemies)
            {
                enemyCollectors.Add(player.Avatar);
            }
            if (collector != null) {
                NotifyCollect(collector);
                if (AutoUse)
                {
                    UsePowerUp();
                    if (EventManager.OnPowerUpAction != null)
                        EventManager.OnPowerUpAction(ID);
                }
                if (DestroyAfterUse)
                    Destroy(gameObject, Time.deltaTime);

            }
        }
    }

    public enum PowerUpID
    {
        Kamikaze,
        AmmoCleaner,
        CleanSweep,
        Tank,
        InvertCommands
    }

    [System.Serializable]
    public class PowerUpOptions
    {
        public List<PowerupsPercentage> Percentages = new List<PowerupsPercentage>();
    }

}
