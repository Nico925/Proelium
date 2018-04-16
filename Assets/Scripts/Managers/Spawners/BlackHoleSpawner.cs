using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class BlackHoleSpawner : SpawnerBase
    {
        Vector3 randomPos;
        int BlackHoleSpawned = 0;

        new public BlackHoleSpawnerOptions Options;

        GameObject container;

        float Timer;
        BlackHoleState _currentState;

        public BlackHoleState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
            }
        }

        public enum BlackHoleState
        {
            Timer, Spawn, Stop,
        }

        #region Spawner Life Flow
        public override SpawnerBase OptionInit(SpawnerOptions options)
        {
            Options = options as BlackHoleSpawnerOptions;
            return this;
        }

        public override void Init()
        {
            Timer = Options.TimerToSpawn;
            CurrentState = BlackHoleState.Timer;

            container = new GameObject("BlackHoleContainer");
            container.transform.parent = GameManager.Instance.LevelMng.Arena.transform;
            ID = "BlackHoleSpawner";
        }

        void Update()
        {
            if (IsActive)
            {
                switch (CurrentState)
                {
                    case BlackHoleState.Timer:
                        Timer -= Time.deltaTime;
                        if (Timer <= 0 && BlackHoleSpawned <= Options.BlackHoleToSpawn)
                        {
                            if (BlackHoleSpawned == Options.BlackHoleToSpawn)
                            {
                                CurrentState = BlackHoleState.Stop;
                            }
                            else
                            {
                                CurrentState = BlackHoleState.Spawn;
                            }
                        }
                        break;
                    case BlackHoleState.Spawn:
                        SpawnBlackHole();
                        BlackHoleSpawned++;
                        Timer = Options.TimerToSpawn;
                        CurrentState = BlackHoleState.Timer;
                        break;
                    case BlackHoleState.Stop:
                        enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Restart()
        {
            CleanSpawned();
            Init();
        }

        public override void CleanSpawned()
        {
            if (container != null)
                Destroy(container);
        }
        #endregion

        /// <summary>
        /// Istanzia il buco nero in una posizione randomica
        /// </summary>
        void SpawnBlackHole()
        {
            randomPos = new Vector3(Random.Range(Options.minRandomX, Options.maxRandomX), 0, Random.Range(Options.minRandomZ, Options.maxRandomZ));
            Instantiate(Options.BlackHolePrefab, randomPos, Quaternion.identity, container.transform);
        }
    }

    [System.Serializable]
    public class BlackHoleSpawnerOptions : SpawnerOptions
    {
        public float minRandomX;
        public float maxRandomX;
        public float minRandomZ;
        public float maxRandomZ;
        public GameObject BlackHolePrefab;
        public int BlackHoleToSpawn = 3;
        public float TimerToSpawn = 10;
    }
}