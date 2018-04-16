using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class TurretSpawner : SpawnerBase
    {
        new public TurretSpawnerOptions Options;
        [HideInInspector]
        public int Spawnedturrent = 0;

        GameObject container;

        #region Spawner Life Flow
        public override SpawnerBase OptionInit(SpawnerOptions options)
        {
            Options = options as TurretSpawnerOptions;
            return this;
        }

        public override void Init()
        {
            Vector3 randomPosition = new Vector3(Random.Range(-70.0f, 100.0f), 0, Random.Range(-100.0f, 100.0f));
            Instantiate(Options.Turrent, randomPosition, Quaternion.identity, container.transform);

            container = new GameObject("TurretContainer");
            container.transform.parent = GameManager.Instance.LevelMng.Arena.transform;
            ID = "TurretSpawner";
        }

        void Update()
        {
            if (IsActive)
            {
                if (Time.time <= 0 && Spawnedturrent <= Options.MaxSpawnTurrent)
                {
                    if (Spawnedturrent == Options.MaxSpawnTurrent)
                    {
                        Options.Turrent = null;
                    }
                    else
                    {
                        Spawnedturrent++;
                    }
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
    }

    [System.Serializable]
    public class TurretSpawnerOptions : SpawnerOptions
    {
        public GameObject Turrent;
        public int MaxSpawnTurrent = 4;
    }
}