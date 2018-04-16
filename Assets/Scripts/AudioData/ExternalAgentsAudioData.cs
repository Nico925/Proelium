using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    [CreateAssetMenu(fileName = "ExternalAgentsAudioData", menuName = "Audio/NewExternalAgentsAudioData", order = 3)]
    public class ExternalAgentsAudioData : ScriptableObject
    {
        public AudioParameter ExternalAgentMovement;
        public List<AudioParameter> Collisions;
    }
}