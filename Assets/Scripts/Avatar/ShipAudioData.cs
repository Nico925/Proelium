using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlackFox {
    [CreateAssetMenu(fileName = "AvatarData", menuName = "Avatar/NewShipAudio", order = 3)]
    public class ShipAudioData : ScriptableObject
    {
        public AudioParameter PinPlaced;
        public AudioParameter Death;
        public AudioParameter NoAmmo;
        public List<AudioParameter> Movements;
        public List<AudioParameter> Shoots;
        public List<AudioParameter> Collisions;
    }
}