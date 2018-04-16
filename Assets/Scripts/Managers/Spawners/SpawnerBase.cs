using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    public abstract class SpawnerBase : MonoBehaviour
    {
        public SpawnerOptions Options;
        public string ID;

        public int Level
        {
            get { return GameManager.Instance.LevelMng.CurrentLevel.LevelNumber; }
        }

        public int Round
        {
            get { return GameManager.Instance.LevelMng.RoundNumber; }
        }

        [HideInInspector]
        public bool IsActive = false;

        #region API
        /// <summary>
        /// Initialize the option of the Spawner
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual SpawnerBase OptionInit(SpawnerOptions options)
        {
            Options = options;
            return this;
        }

        /// <summary>
        /// Method needed to initialize the Spawner
        /// </summary>
        /// <param name="_sManager"></param>
        public virtual void Init() { }

        /// <summary>
        /// Activate/Deactivate the Spawner
        /// </summary>
        public virtual void Toggle(bool _value)
        {
            IsActive = _value;
        }

        /// <summary>
        /// Run it as beginning of round
        /// </summary>
        public virtual void Restart() { }

        /// <summary>
        /// Destroy all the Spawned gameobject from scene
        /// </summary>
        public virtual void CleanSpawned() { }
        #endregion
    }
    
    public class SpawnerOptions
    {
        public GameObject SpawnerPrefab;

        public SpawnerBase CreateInstance(SpawnerOptions _option, Transform _container)
        {
            SpawnerBase spawner = null;
            if (_option.SpawnerPrefab != null)
            { 
                spawner = GameObject.Instantiate<GameObject>(SpawnerPrefab, _container).GetComponent<SpawnerBase>();
                spawner.OptionInit(this);
            }
            return spawner;
        }
    }
}
