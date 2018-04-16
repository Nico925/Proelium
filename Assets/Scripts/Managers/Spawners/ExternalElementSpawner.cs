using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlackFox
{
    public class ExternalElementSpawner : SpawnerBase
    {
        new public ExternalElementOptions Options;
        
        Transform target;                                           //Target of the ExternalElements
        float nextTime;                                             //Timer
        List<IDamageable> Damageables = new List<IDamageable>();    //Lista di oggetti danneggiabili

        GameObject container;

        #region Powerup region
        [HideInInspector]
        public bool IsKamikazeTime = false;

        private float _powerupduration;

        public float PowerupDuration
        {
            get { return _powerupduration; }
            set { _powerupduration = value; }
        }

        public void ActiveKamikazeTime(float _time)
        {
            IsKamikazeTime = true;
            PowerupDuration = _time;
        }

        #endregion

        #region Spawner Life Flow
        public override void Init()
        {
            ID = "ExternalElementSpawner";
            if (Options.ExternalAgent == null)
            {
                Options.ExternalAgent[0] = (GameObject)Resources.Load("Prefabs/ExternalAgents/ExternalAgent1");
                Options.ExternalAgent[1] = (GameObject)Resources.Load("Prefabs/ExternalAgents/ExternalAgent2");

            }

            target = GameManager.Instance.LevelMng.Core.transform;
            nextTime = Time.time + Random.Range(Options.MinTime, Options.MaxTime);
            LoadIDamageablePrefab();
            

            container = new GameObject("ExternalAgentContainer");
            container.transform.parent = GameManager.Instance.LevelMng.Arena.transform;
        }

        public override SpawnerBase OptionInit(SpawnerOptions options)
        {
            Options = options as ExternalElementOptions;
            return this;
        }

        void Update()
        {
            PowerupDuration -= Time.deltaTime;
            if (IsActive)
            {
                if (Time.time >= nextTime)
                {
                    InstantiateExternalAgent();
                    if (IsKamikazeTime)
                    {
                        nextTime = Time.time + Random.Range(Options.MinTime/Options.KamikazeRatioMultiplayer, Options.MaxTime/Options.KamikazeRatioMultiplayer);
                        
                        if (PowerupDuration <= 0)
                            IsKamikazeTime = false;

                    }
                    else
                        nextTime = Time.time + Random.Range(Options.MinTime, Options.MaxTime);
                }
                GravityAround();
            }
        }

        public override void Restart()
        {
            CleanSpawned();
            Init();
        }

        public override void CleanSpawned()
        {
            if(container != null)
                Destroy(container);
        }
        #endregion

        /// <summary>
        /// Rotate around the target position and keep facing it
        /// </summary>
        void GravityAround()
        {
            transform.LookAt(target);
            transform.RotateAround(target.position, Vector3.up, Options.AngularSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Load damageable items (classes with IDamageable) from prefabs
        /// </summary>
        void LoadIDamageablePrefab()
        {
            //WARNING - If a GameObject in the list do not have the IDamageable interface, it will not be damaged
            List<GameObject> DamageablesPrefabs = PrefabUtily.LoadAllPrefabsWithComponentOfType<IDamageable>("Prefabs", Options.ExternalAgent);
            foreach (var k in DamageablesPrefabs)
            {
                if (k.GetComponent<IDamageable>() != null)
                    Damageables.Add(k.GetComponent<IDamageable>());
            }
        }

        /// <summary>
        /// Instatiate an External Agent
        /// </summary>
        void InstantiateExternalAgent()
        {
            GameObject instantiateEA = Instantiate(Options.ExternalAgent[Random.Range(0, Options.ExternalAgent.Count)], transform.position, transform.rotation, container.transform);
            ExternalAgent eA = instantiateEA.GetComponent<ExternalAgent>();
            eA.Initialize(target, Damageables);
        }
    }

    [System.Serializable]
    public class ExternalElementOptions : SpawnerOptions
    {
        public List<GameObject> ExternalAgent;                        //Prefab of the ExternalAgent to instantiate         

        public float MinTime = 10;                              //Min time between Spawns
        public float MaxTime = 20;                              //Max time between Spawns
        public float AngularSpeed = 1;                          //Rotation speed
        public float KamikazeRatioMultiplayer = 10;             //Multiplayer of ratio of spawn
    }
}
