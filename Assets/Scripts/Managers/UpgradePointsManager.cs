using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{

    public class UpgradePointsManager : MonoBehaviour
    {

        public int PointsForWinner = 2;
        public int PointsForLosers = 1;

        /// <summary>
        /// Assegna i punti di upgrade ai player
        /// </summary>
        /// <param name="_player"></param>
        public void GivePoints(PlayerLabel _player)
        {
            foreach (Player player in GameManager.Instance.PlayerMng.Players)
            {
                if(player.ID == _player)
                    player.Avatar.UpgradePoints += PointsForWinner;
                else
                    player.Avatar.UpgradePoints += PointsForLosers;
            }
        }

        public void CheatPoints(PlayerLabel _player)
        {
            if (_player == PlayerLabel.Different)
                foreach (Player player in GameManager.Instance.PlayerMng.Players)
                    player.Avatar.UpgradePoints += 20;
        }
    }
}