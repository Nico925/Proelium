using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlackFox {
    [RequireComponent(typeof(MovementController))]
    public class Ship : MonoBehaviour, IShooter, IDamageable
    {
        [HideInInspector]
        public Avatar Avatar;
        public ShipConfig Config
        {
            get { return Avatar.AvatarData.shipConfig; }
        }
        [HideInInspector]
        public ShipAudioSourceController audioSourceController;
        [HideInInspector]
        public GameObject Model;

        private ParticlesController _particlesController;

        public ParticlesController ParticlesController
        {
            get {
                if (_particlesController != null)
                    return _particlesController;
                else return null;
            }
            set { _particlesController = value; }
        }


        MovementController movment;
        PlacePin pinPlacer;
        Tweener damageTween;

        #region PowerUp comandi invertiti

        private bool _isInverted;
        /// <summary>
        /// indica se i comandi sono invertiti
        /// </summary>
        public bool IsInverted
        {
            get { return _isInverted; }
            set { _isInverted = value; }
        }
        Avatar inverter = null;
        float timeofInvertion;

        #endregion

        // Life fields
        private float _life;
        public float Life {
            get { return _life; }
            private set {
                _life = value;
                if(EventManager.OnLifeValueChange != null)
                    EventManager.OnLifeValueChange(Avatar);
            }
        }

        private void Update()
        {
            if (Avatar.State == AvatarState.Enabled)
            {
                if(!GameManager.Instance.LevelMng.IsGamePaused)
                    CheckInputStatus(Avatar.Player.InputStatus);

                if(inverter != null && inverter.State != AvatarState.Enabled)
                {
                    timeofInvertion = 0;
                    IsInverted = false;
                    ParticlesController.StopParticles(ParticlesController.ParticlesType.InvertCommand);
                }
                else if(IsInverted)
                {
                    timeofInvertion -= Time.deltaTime;
                    if (timeofInvertion <= 0)
                    {
                        IsInverted = false;
                        ParticlesController.StopParticles(ParticlesController.ParticlesType.InvertCommand);
                    }
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {         
            audioSourceController.PlayCollisionAudio();
        }

        #region API
        public void Setup(Avatar _avatar, List<IDamageable> _damageablesPrefabs)
        {
            Avatar = _avatar;
            rigid = GetComponent<Rigidbody>();
            InstantiateModel();
            damageables = _damageablesPrefabs;
            ChangeColor(Avatar.AvatarData.ColorSets[Avatar.AvatarData.ColorSetIndex].ShipMaterialMain);

            shooter = GetComponentInChildren<Shooter>();
            shooter.Init(this);
            movment = GetComponent<MovementController>();
            movment.Init(this, rigid);
            pinPlacer = GetComponentInChildren<PlacePin>();
            pinPlacer.Setup(this);
            ParticlesController = GetComponent<ParticlesController>();
            ParticlesController.Init();

            audioSourceController = GetComponentInChildren<ShipAudioSourceController>();
            audioSourceController.Init(this);
        }

        public void InstantiateModel()
        {
            Model = Instantiate(Avatar.AvatarData.ModelPrefab, transform.position, transform.rotation, transform);
        }

        /// <summary>
        /// Initialize initial values of Avatar
        /// </summary>
        public void Init()
        {
            Life = Config.MaxLife;
            pinPlacer.Init();
        }

        public void ChangeColor(Material _mat)
        {
            foreach (var m in Model.GetComponentsInChildren<MeshRenderer>())
            {
                Material[] mats = new Material[] { _mat };
                m.materials = mats;
            } 
        }       
        
        public void SetInverter(float _timeOfInvertion, Avatar _inverter)
        {
            inverter = _inverter;
            timeofInvertion = _timeOfInvertion;
            IsInverted = true;
            ParticlesController.PlayParticles(ParticlesController.ParticlesType.InvertCommand);
        }
        #endregion

        // Input Fields
        Vector3 _leftStickDirection;

        public Vector3 LeftStickDirection
        {
            get { return _leftStickDirection; }
            private set { _leftStickDirection = value; }
        }

        Vector3 rightStickDirection;

        void CheckInputStatus(InputStatus _inputStatus)
        {            
            LeftStickDirection = new Vector3(_inputStatus.LeftThumbSticksAxisX, 0, _inputStatus.LeftThumbSticksAxisY);
            rightStickDirection = new Vector3(_inputStatus.RightThumbSticksAxisX, 0, _inputStatus.RightThumbSticksAxisY);

            // Se un giocatore avversario prende il Powerup per invertire i comandi
            if (IsInverted)
                Move(-LeftStickDirection);
            else
                Move(LeftStickDirection);

            DirectFire(rightStickDirection);

            if (_inputStatus.LeftTrigger == ButtonState.Pressed)
            {
                PlacePin();
            }

            if (_inputStatus.RightTrigger == ButtonState.Pressed)
            {
                Shoot();
                nextFire = Time.time + Config.FireRate;
            }
            else if (_inputStatus.RightTrigger == ButtonState.Held )
            {
                if(Time.time > nextFire)
                { 
                    Shoot();
                    nextFire = Time.time + FireRate;
                }
            }

            if (_inputStatus.Start == ButtonState.Pressed)
            {
                GameManager.Instance.LevelMng.PauseGame(Avatar.Player.ID);
            }
        }

        #region PinPlacer
        /// <summary>
        /// Remove all the placed Pins of this ship
        /// </summary>
        public void RemoveAllPins()
        {
            pinPlacer.RemoveAllPins();
        }
        /// <summary>
        /// Set new collision layer of the pins
        /// </summary>
        /// <param name="_newLayer">Ordinal number of the layer</param>
        public void SetNewPinLayer(int _newLayer)
        {
            pinPlacer.SetCollisionLayer(_newLayer);
        }
        #endregion

        #region Shoot
        public Shooter shooter { get; private set; }

        public float FireRate { get { 
                    if (Avatar.GetUpgrade(UpgardeTypes.FireRate) != null)
                        return Avatar.GetUpgrade(UpgardeTypes.FireRate).CalculateValue(Config.FireRate);
                    else
                        return Config.FireRate;
                    }
        }

        //Shooting fields
        float nextFire;

        /// <summary>
        /// List of element damageable by this player
        /// </summary>
        List<IDamageable> damageables = new List<IDamageable>();

        /// <summary>
        /// Chiama la funzione AddAmmo di shooter
        /// </summary>
        public void AddShooterAmmo()
        {
            shooter.AddAmmo();
        }

        public void NoAmmoAudio()
        {
            audioSourceController.PlayNoAmmoAudio();
        }

        #region IShooter
        /// <summary>
        /// Ritorna la lista degli oggetti danneggiabili
        /// </summary>
        /// <returns></returns>
        public List<IDamageable> GetDamageable() {
            return damageables;
        }
        /// <summary>
        /// Return the one who shot
        /// </summary>
        /// <returns></returns>
        public GameObject GetOwner() {
            return gameObject;
        }
        #endregion

        #endregion

        #region IDamageable
        /// <summary>
        /// Danneggia la vita dell'agente a cui è attaccato e ritorna i punti da assegnare all'agente che lo ha copito
        /// </summary>
        /// <param name="_damage">La quantità di danni che subisce</param>
        /// <returns></returns>
        public void Damage(float _damage, GameObject _attacker) {
            if (damageTween != null)
                damageTween.Complete();

            Life -= _damage;
            StopCoroutine("Rumble");
            StartCoroutine(Rumble(0.2f));
            damageTween = transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
            ParticlesController.PlayParticles(ParticlesController.ParticlesType.Damage);
            if (Life < 1)
            {
                GameManager.Instance.LevelMng.ExplosionPoolMng.GetPooledObject(transform.position);
                if (_attacker.GetComponent<Ship>() != null)
                    Avatar.ShipDestroy(_attacker.GetComponent<Ship>().Avatar);
                else
                    Avatar.ShipDestroy(null);

                audioSourceController.PlayDeathAudio();

                transform.DOScale(Vector3.zero, 0.5f);
                return;
            }
        }

        IEnumerator Rumble(float _rumbleTime)
        {
            Avatar.Player.ControllerVibration(0.5f, 0.5f);
            yield return new WaitForSeconds(_rumbleTime);
            Avatar.Player.ControllerVibration(0f, 0f);
        }
        #endregion

        #region Ship Abilities
        //Variabili per gestire la fisca della corda
        Rigidbody rigid;
        Vector3 previousSpeed;

        /// <summary>
        /// Set all the Player abilities as active/inactive
        /// </summary>
        /// <param name="_active"></param>
        public void ToggleAbilities(bool _active = true)
        {
            pinPlacer.enabled = _active;
            shooter.enabled = _active;
            movment.enabled = _active;
            if(!_active)
                audioSourceController.StopAll();
            GetComponent<CapsuleCollider>().enabled = _active;
        }

        void DirectFire(Vector3 _direction)
        {
            shooter.SetFireDirection(_direction);
        }

        void Shoot()
        {
            shooter.ShootBullet();
            audioSourceController.PlayShootAudio();
        }

        void PlacePin()
        {
            if (pinPlacer.PlaceThePin())
            {
                AddShooterAmmo();
                audioSourceController.PlayAmmoRechargeAudio();
            }
        }

        void Move(Vector3 _target)
        {
            movment.Move(_target);

            if (_target.magnitude > 0.2f)
                ParticlesController.PlayParticles(ParticlesController.ParticlesType.Movement);  
            else
                ParticlesController.StopParticles(ParticlesController.ParticlesType.Movement);
            if (Avatar.rope != null)
                ExtendRope(_target.magnitude);
        }

        void ExtendRope(float _amount)
        {
            if (_amount >= .95f) {
                Avatar.rope.ExtendRope(1);
            }
            previousSpeed = rigid.velocity;
        }
        #endregion
    }

    [Serializable]
    public class ShipConfig
    {
        [Header("Ship Parameters")]
        public float MaxLife;
        public float FireRate;
        public MovementControllerConfig movementConfig;
        public ShooterConfig shooterConfig;
        public PlacePinConfig placePinConfig;
    }
}