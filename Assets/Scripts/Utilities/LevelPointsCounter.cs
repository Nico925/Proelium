using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BlackFox
{
    public class LevelPointsCounter
    {
        LevelManager levelManager;

        int AddPoints
        {
            get { return levelManager.levelOptions.AddPoints; }
        }
        int SubPoints
        {
            get { return levelManager.levelOptions.SubPoints; }
        }
        int PointsToWin
        {
            get { return levelManager.levelOptions.PointsToWin; }
        }

        public List<PlayerLabel> PlayerToPlayOff = new List<PlayerLabel>();

        List<PlayerStats> playerStats = new List<PlayerStats>();


        public LevelPointsCounter(LevelManager _levelManager, List<Player> _players)
        {
            levelManager = _levelManager;
            foreach (Player player in _players) {
                playerStats.Add(new PlayerStats(player));
            }
        }

        /// <summary>
        /// Aggiorna i punti uccsione del player che è stato ucciso e di quello che ha ucciso
        /// </summary>
        /// <param name="_killer"></param>
        /// <param name="_victim"></param>
        void AddKillPoints(PlayerLabel _killer)
        {
            foreach (PlayerStats player in playerStats)
            {
                if (player.Player.ID == _killer)
                {
                    player.KillPoints += AddPoints;
                    levelManager.CheckRoundStatus();
                    break;
                }
            }
        }

        /// <summary>
        /// Aggiorna i punti uccisione del player che è morto
        /// </summary>
        /// <param name="_victim"></param>
        void SubKillPoints(PlayerLabel _victim)
        {
            foreach (PlayerStats player in playerStats)
            {
                if (player.Player.ID == _victim && player.KillPoints > 0)
                {
                    player.KillPoints -= SubPoints;
                    break;
                }
            }
        }


        #region API
        public void UpdateKillPoints(PlayerLabel _killer, PlayerLabel _victim)
        {
            AddKillPoints(_killer);
            SubKillPoints(_victim);
        }

        public void UpdateKillPoints(PlayerLabel _victim)
        {
            SubKillPoints(_victim);
        }

        /// <summary>
        /// Ritorna i punti uccisione del player che chiama la funzione
        /// </summary>
        /// <param name="_playerID">Indice del Player</param>
        /// <returns></returns>
        public int GetPlayerKillPoints(PlayerLabel _playerID)
        {
            foreach (PlayerStats player in playerStats)
                if (player.Player.ID == _playerID)
                    return player.KillPoints;
            return -1;
        }

        public PlayerStats GetPlayerStats(PlayerLabel _playerID)
        {
            foreach (PlayerStats player in playerStats)
                if (player.Player.ID == _playerID)
                    return player;
            return null;
        }

        /// <summary>
        /// Ritorna la lista delle statistiche ordinata per vittorie
        /// </summary>
        /// <returns></returns>
        public List<PlayerStats> GetPlayerStatsByKillPointsInOrderDesc()
        {
            List<PlayerStats> tempPlayerStats = playerStats.OrderByDescending(t => t.KillPoints).ToList();
            return tempPlayerStats;
        }

        /// <summary>
        /// Ritorna le vittorie del player che chiama la funzione
        /// </summary>
        /// <param name="_playerID"></param>
        /// <returns></returns>
        public int GetPlayerVictories(int _playerID)
        {
            foreach (PlayerStats player in playerStats)
                if ((int)player.Player.ID == _playerID)
                    return player.Victories;
            return -1;
        }

        public void AddPlayerVictory(PlayerLabel _playerID)
        {
            foreach (PlayerStats player in playerStats)
                if (player.Player.ID == _playerID)
                     player.Victories += 1;
        }

        /// <summary>
        /// Azzera i punti uccisione di tutti i player
        /// </summary>
        public void ClearAllKillPoints()
        {
            foreach (PlayerStats player in playerStats)
                player.ResetKillPoints();
        }
        #endregion
    }

    /// <summary>
    /// Contenitore dei punti del player
    /// </summary>
    public class PlayerStats
    {
        Player player;
        int killPoints;
        int victories;

        public Player Player
        {
            get { return player; }
        }

        public int KillPoints
        {
            get { return killPoints; }
            set { killPoints = value; }
        }

        public int Victories
        {
            get { return victories; }
            set { victories = value; }
        }

        public PlayerStats(Player _player)
        {
            player = _player;
        }

        public void ResetKillPoints()
        {
            killPoints = 0;
        }
    }
}