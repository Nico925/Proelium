using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class SpawnPoint : MonoBehaviour
    {
        /// <summary>
        /// serve a dire a quale setup partiene la freccia
        /// </summary>
        public int IDSetup;        
        /// <summary>
        /// To be filled in Editor by Designer to set the trasform  as a SpawnPoint
        /// </summary>
        public List<SpawnType> ValidAs = new List<SpawnPoint.SpawnType>();
        /// <summary>
        /// Set it also as a AvatarSpawnPoint
        /// </summary>
        public AvatarSpawnType SpawnAvatar = AvatarSpawnType.None;

        public enum AvatarSpawnType
        {
            None,
            Blue,
            Red,
            Green,
            Purple
        }

        public enum SpawnType
        {
            ExternalElementSpawn,
            ArrowSpawner,
            TurretSpawn,
            WaveSpawn,
            BlackHoleSpawn
        }
    }
}
