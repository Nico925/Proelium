using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    [CreateAssetMenu(fileName = "LevelName", menuName = "Levels/NewLevel", order = 1)]
    public class Level : ScriptableObject {

        public int LevelNumber;
        public string LevelName;
        public GameObject ArenaPrefab;

        [Header("Level Options")]
        public LevelOptions LevelOptions;
        public float MinPowerUpRatio;
        public float MaxPowerUpRatio;
        public PowerUpOptions PowerUpOptions;

        [Header("Spawners")]
        public ArrowsSpawerOptions ArrowsSpawner;
        public BlackHoleSpawnerOptions BlackHoleSpawner;
        public ExternalElementOptions ExternalElementSpawner;
        public TurretSpawnerOptions TurretSpawner;
        public WaveSpawnerOptions WaveSpawner;
    } 
}
