using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Rope;

namespace BlackFox
{
    /// <summary>
    /// Informazioni riguardo a ciò che contiene l'avatar
    /// </summary>
    [CreateAssetMenu(fileName = "AvatarData", menuName = "Avatar/NewShip", order = 1)]
    public class AvatarData : ScriptableObject
    {
        public string DataName;
        public bool IsPurchased;
        public int Price;
        public Ship BasePrefab;
        public GameObject ModelPrefab;
        public List<ColorSetData> ColorSets;
        public ShipAudioData ShipAudioSet;
        public ShipConfig shipConfig;
        public RopeConfig ropeConfig;

        [HideInInspector]
        public int ColorSetIndex;

        [HideInInspector]
        public int[][] SelectionParameters = new int[4][] {
            new int[5] { 4, 1, 1, 2, 2 }, // Tank
            new int[5] { 2, 4, 4, 1, 3 }, // Scout
            new int[5] { 3, 2, 2, 2, 3 }, // Standard
            new int[5] { 1, 3, 3, 4, 1 }, // Assault
        };


        [Header("Avatar Upgrades")]
        public AvatarUpgradesConfig avatarUpgradesConfig;
    }
}
