using System.Collections.Generic;
using UnityEngine;
using Rope;
using System;

namespace BlackFox {
    public class Avatar : MonoBehaviour, IPowerUpCollector {

        /// <summary>
        /// Player who control this avatar
        /// </summary>
        [HideInInspector]
        public Player Player;
        public List<Player> Enemies { get { return GameManager.Instance.PlayerMng.GetAllOtherPlayers(Player); } }
        /// <summary>
        /// Index of th e player
        /// </summary>
        public PlayerLabel PlayerId {
            get {
                if (Player == null)
                    return PlayerLabel.None;
                return Player.ID;
            }
        }

        /// <summary>
        /// Reference of the model to visualize
        /// </summary>
        public AvatarData AvatarData {
            get { return Player.AvatarData; }
        }

        private AvatarState _state;
        public AvatarState State {
            get { return _state; }
            set {
                if (Player != null) {
                    if (_state != value)
                        OnStateChange(value, _state);
                    _state = value;
                }
            }
        }

        private int _upgradePoints;

        public int UpgradePoints
        {
            get { return _upgradePoints; }
            set { _upgradePoints = value; }
        }


        [HideInInspector]
        public RopeController rope;
        [HideInInspector]
        public Ship ship;

        public AvatarUI avatarUI;

        /// <summary>
        /// Crea e prende riferimento dell'AvatarUI
        /// </summary>
        void CreateShipUI()
        {
            avatarUI = GameManager.Instance.UiMng.CreateAvatarUI(ship.gameObject);
        }

        /// <summary>
        /// Menage the state switches
        /// </summary>
        /// <param name="_newState"></param>
        /// <param name="_oldState"></param>
        void OnStateChange(AvatarState _newState, AvatarState _oldState)
        {
            switch (_newState)
            {
                case AvatarState.Disabled:
                    if (ship != null)
                    {
                        ship.RemoveAllPins();
                        ship.ToggleAbilities(false);
                    }
                    if (rope != null)
                    {
                        rope.DestroyDynamically();
                        rope = null;
                    }
                    break;
                case AvatarState.Ready:
                    InitShip();
                    rope.GetComponent<LineRenderer>().enabled = false;
                    break;
                case AvatarState.Enabled:
                    ship.ToggleAbilities(true);
                    ship.transform.localScale = Vector3.one;
                    rope.GetComponent<LineRenderer>().enabled = true;
                    break;
            }
        }

        #region API
        /// <summary>
        /// Required to setup the player (also launched on Start of this class)
        /// </summary>
        public void Setup(Player _player)
        {
            Player = _player;
            LoadUpgradesValues();
            SetupShip();
        }

        public void SetupShip()
        {
            InstantiateShip();
            ship.Setup(this, LoadIDamageableForShip());
            CreateShipUI();
        }

        public void InitShip(bool withRope = true)
        {
            ship.Init();
            if (withRope)
                SetupRope();
        }

        public void SetupRope()
        {
            if (GameManager.Instance.LevelMng.RopeMng != null && rope == null)
                GameManager.Instance.LevelMng.RopeMng.AttachNewRope(this);
        }

        public void InstantiateShip()
        {
            // TODO : controllare che la ship non sia doppia
            Transform transf = GameManager.Instance.LevelMng.AvatarSpwn.GetMySpawnPoint(PlayerId);
            ship = Instantiate(AvatarData.BasePrefab, transf.position, transf.rotation , transform).GetComponent<Ship>();
        }
        /// <summary>
        /// Set new layers of collision for Rope and Pin
        /// </summary>
        /// <param name="_ropeLayer">Ordinal number of collision Layer</param>
        /// <param name="_pinLayer">Ordinal number of collision Layer</param>
        public void SetNewCollisionLayers(int _ropeLayer, int _pinLayer)
        {
            rope.SetCollisionLayer(_ropeLayer);
            ship.SetNewPinLayer(_pinLayer);
        }

        /// <summary>
        /// L'ancia un evento alla distruzione della ship, 
        /// passando come parametri chi ha distrutto la ship e l'ID del player a cui appartiene
        /// </summary>
        /// <param name="_attacker">L'avatar che ha distrutto la ship</param>
        public void ShipDestroy(Avatar _attacker)
        {
            State = AvatarState.Disabled;
            if (EventManager.OnAgentKilled != null) {
                if (_attacker != null)
                    EventManager.OnAgentKilled(_attacker, this);
                else
                    EventManager.OnAgentKilled(null, this);
            }
        }

        /// <summary>
        /// Scatena l'evento per aggiornare i proiettili nella UI
        /// </summary>
        /// <param name="_ammo">Le munizioni che rimangono</param>
        public void OnAmmoUpdate()
        {
            // TODO : da rivedere
            EventManager.OnAmmoValueChange(this);
        }

        #region Upgrade
        AvatarUpgradesConfig UpgradesConfig
        {
            get { return AvatarData.avatarUpgradesConfig; }
        }

        public List<IUpgrade> Upgrades = new List<IUpgrade>();

        public IUpgrade GetUpgrade(UpgardeTypes _id)
        {
            foreach (IUpgrade upgrade in Upgrades)
            {
                if (upgrade.ID == _id)
                    return upgrade;
            }
            return null;
        }

        void LoadUpgradesValues()
        {
            Upgrades.Add(new PowerUpDurationUpgrade(UpgradesConfig.PowerUpDurationUpgrade));
            Upgrades.Add(new ShootingDistanceUpgrade(UpgradesConfig.ShootingDistanceUpgrade));
            Upgrades.Add(new PinRegenUpgrade(UpgradesConfig.PinRegenUpgrade));
        }
        #endregion
        #endregion

        /// <summary>
        /// Carica la lista dei damageable per la propria ship da resources
        /// </summary>
        List<IDamageable> LoadIDamageableForShip()
        {
            List<IDamageable> damageablesList = new List<IDamageable>();
            List<GameObject> prefabToRemove = new List<GameObject>() { ship.gameObject, GameManager.Instance.LevelMng.Core.gameObject};
            List<GameObject> damageablesPrefabs = PrefabUtily.LoadAllPrefabsWithComponentOfType<IDamageable>("Prefabs", prefabToRemove);

            foreach (GameObject itemInRes in damageablesPrefabs)
            {
                if (itemInRes.GetComponent<IDamageable>() != null)
                    damageablesList.Add(itemInRes.GetComponent<IDamageable>());
            }

            return damageablesList;
        }

        public void CollectPowerUp(IPowerUp _powerUp)
        {
            
        }

        /// <summary>
        /// Funzione per invertire i comandi di moviemti nella ship
        /// </summary>
        /// <param name="_time">Per quanto tempo devono rimanere invertiti</param>
        public void InvertCommands(float _time, Avatar _activatorOf)
        {
            ship.SetInverter(_time, _activatorOf);
        }       
    }

    public enum AvatarState
    {
        Disabled = 0,
        Ready = 1,
        Enabled = 2
    }

    [Serializable]
    public class AvatarUpgradesConfig
    {
        public float[] ShootingDistanceUpgrade;
        public float[] PowerUpDurationUpgrade;
        public float[] PinRegenUpgrade;
    }
}