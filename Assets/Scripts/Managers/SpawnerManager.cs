using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    /// <summary>
    /// It manages a list of Spawners during their LifeFlow
    /// </summary>
    public class SpawnerManager : MonoBehaviour
    {
        public List<SpawnerBase> Spawners = new List<SpawnerBase>();
        
        #region API
        public void InstantiateNewSpawners(Level _currentLevel)
        {
            Spawners.Add(_currentLevel.ArrowsSpawner.CreateInstance(_currentLevel.ArrowsSpawner, transform));
            Spawners.Add(_currentLevel.BlackHoleSpawner.CreateInstance(_currentLevel.BlackHoleSpawner, transform));
            Spawners.Add(_currentLevel.ExternalElementSpawner.CreateInstance(_currentLevel.ExternalElementSpawner, transform));
            Spawners.Add(_currentLevel.TurretSpawner.CreateInstance(_currentLevel.TurretSpawner, transform));
            Spawners.Add(_currentLevel.WaveSpawner.CreateInstance(_currentLevel.WaveSpawner, transform));

            Spawners.RemoveAll(s => s == null);
        }      

        public void InitSpawners()
        {
            foreach (SpawnerBase spawner in Spawners)
            {
                if(spawner != null)
                    spawner.Init();
            }
        }

        public void ReStartSpawners()
        {
            foreach (SpawnerBase spawner in Spawners)
            {
                if (spawner != null)
                    spawner.Restart();
            }
        }

        public void CleanSpawnersSpawnedElements()
        {
            foreach (SpawnerBase spawner in Spawners)
            {
                if (spawner != null)
                    spawner.CleanSpawned();
            }
        }

        public void ToggleSpawners(bool _value)
        {
            foreach (SpawnerBase spawner in Spawners)
            {
                if (spawner != null)
                    spawner.Toggle(_value);
            }
        }
        #endregion
    }
}
