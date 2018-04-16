using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace BlackFox
{
    public class PlayerManager : MonoBehaviour
    {
        public List<Player> Players = new List<Player>();

        #region API
        #region Player
        /// <summary>
        /// Instanzia i player
        /// </summary>
        public void InstantiatePlayers()
        {
            for (int i = 1; i <= 4; i++)
            {
                Player newPlayer = gameObject.AddComponent<Player>();
                newPlayer.Setup((PlayerLabel)i);
                Players.Add(newPlayer);
            }
        }

        /// <summary>
        /// Cambia lo stato del player specificato
        /// </summary>
        /// <param name="_playerState"></param>
        /// <param name="_playerID"></param>
        public void ChangePlayerState(PlayerState _playerState, PlayerLabel _playerID)
        {
            foreach (Player player in Players)
            {
                if (player.ID == _playerID)
                    player.PlayerCurrentState = _playerState;
            }
        }

        /// <summary>
        /// Cambia lo stato di tutti i player
        /// </summary>
        /// <param name="_playerState"></param>
        public void ChangeAllPlayersState(PlayerState _playerState)
        {
            foreach (Player player in Players)
                player.PlayerCurrentState = _playerState;
        }

        public void ChangeAllPlayersStateExceptOne(PlayerState _playerState, PlayerLabel _playerID, PlayerState _otherPlayersState)
        {
            foreach (Player player in Players)
            {
                if (player.ID == _playerID)
                    player.PlayerCurrentState = _playerState;
                else
                    player.PlayerCurrentState = _otherPlayersState;
            }
        }

        public void StopPlayersAudio()
        {
            foreach (Player player in Players)
            {
                player.Avatar.ship.audioSourceController.StopAll();
            }
        }

        /// <summary>
        /// Ritorna il riferimento del player corrispondente all'indice passato
        /// </summary>
        /// <param name="_playerIndex"></param>
        /// <returns></returns>
        public Player GetPlayer(PlayerLabel _playerIndex)
        {
            foreach (Player player in Players)
            {
                if (player.ID == _playerIndex)
                    return player;
            }
            return null;
        }

        public List<Player> GetAllOtherPlayers(Player _player)
        {
            List<Player> PlayersToReturn = new List<Player>();
            foreach (Player player in Players)
            {
                if (player != _player)
                    PlayersToReturn.Add(player);
            }
            return PlayersToReturn;
        }
        #endregion

        #region Avatar
        /// <summary>
        /// Setup all the avatars of the current players
        /// </summary>
        /// <param name="_forceIstance">If true ask for a new istance if there are none</param>
        public void SetupAvatars(bool _forceIstance = false)
        {
            foreach (Player player in Players)
                player.AvatarSetup(_forceIstance);
        }

        public void ChangeAvatarsState(AvatarState _state)
        {
            foreach (Player player in Players)
            {
                player.Avatar.State = _state;
            }
        }

        public void DestroyAllAvatar()
        {
            foreach (Player player in Players)
            {
                Destroy(player.Avatar.gameObject);
            }
        }
        #endregion

        /// <summary>
        /// Azzera le munizioni di tutti i player
        /// </summary>
        public void CleanAllAmmo()
        {
            foreach (Player player in Players)
            {
                player.Avatar.ship.shooter.Ammo = 0;
            }
        }

        #endregion
    }

    /// <summary>
    /// Stati in cui il player può essere
    /// </summary>
    public enum PlayerState
    {
        Blocked = 0,
        MenuInput = 1,
        PlayInput = 2
    }
}

