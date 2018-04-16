using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    [CreateAssetMenu(fileName = "PowerUpAudioData", menuName = "Audio/NewPowerUpAudioData", order = 1)]
    public class PowerUpAudioData : ScriptableObject
    {
        public List<AudioParameter> PowerUpSpawn;
        public AudioParameter KamikazeActivation;
        public AudioParameter AmmoCleanerActivation;
        public AudioParameter CleanSweepActivation;
        public AudioParameter TankActivation;
        public AudioParameter InvertCommandsActivation;
    }
}