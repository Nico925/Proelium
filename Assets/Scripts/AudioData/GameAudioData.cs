using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    [CreateAssetMenu(fileName = "GameAudioData", menuName = "Audio/NewGameAudioData", order = 4)]
    public class GameAudioData : ScriptableObject
    {
        public AudioParameter Count1DownAudio;
        public AudioParameter Count2DownAudio;
        public AudioParameter Count3DownAudio;
        public AudioParameter Round1Audio;
        public AudioParameter Round2Audio;
        public AudioParameter Round3Audio;
        public AudioParameter Round4Audio;
        public AudioParameter Round5Audio;
        public AudioParameter GameplayMusic;
        public List<AudioParameter> CoinCollected;
    }
}