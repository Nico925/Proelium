using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlackFox {

    public class Turrent : MonoBehaviour,IDamageable,IShooter {

        public float MaxLife = 5;
        public float Life;
        public float TimeShoot;

        bool IsInactive;
        float Timer;
        float TimerToInactivate = 5;

        ShooterBase shooter;

        List<GameObject> ItemInRange = new List<GameObject>();
        List<IDamageable> damageables = new List<IDamageable>();                        // Lista di Oggetti facenti parte dell'interfaccia IDamageable


        void Start()
        {
            IsInactive = false;
            Timer = TimeShoot;
            Life = MaxLife;
            LoadIDamageablePrefab();
            shooter = GetComponent<ShooterBase>();
        }

        void Update()
        {
            if (IsInactive == false)
            {
                FollowTarget();
            }
            else
            {
                TimerToInactivate -= Time.deltaTime;
                if (TimerToInactivate <= 0)
                {
                    IsInactive = false;
                    TimerToInactivate = 5;
                    Life = MaxLife;
                }
               
            }
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Avatar>() != null)
                ItemInRange.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (GameObject item in ItemInRange)
            {
                if (item == other.gameObject)
                    ItemInRange.Remove(item);
            }
        }

        private void LoadIDamageablePrefab()
        {
            //WARNING - se l'oggetto che che fa parte della lista di GameObject non ha l'interfaccia IDamageable non farà parte degli oggetti danneggiabili.

            List<GameObject> DamageablesPrefabs = PrefabUtily.LoadAllPrefabsWithComponentOfType<IDamageable>("Prefabs", gameObject);

            foreach (var k in DamageablesPrefabs)
            {
                if (k.GetComponent<IDamageable>() != null)
                    damageables.Add(k.GetComponent<IDamageable>());
            }
        }

        void FollowTarget()
        {
            if(ItemInRange.Count == 1)
            {
               
                CallShooter();
              }
            else
            {
               
                foreach (GameObject item in ItemInRange)
                {
                
                }
            }

        }

        void CallShooter()
        {
            Vector3 _target = new Vector3(ItemInRange[0].transform.position.x, 0f, ItemInRange[0].transform.position.z);
            transform.DOLookAt(_target, 0.5f);
            Timer -= Time.deltaTime;
            if (Timer <= 0f)
            {
                shooter.ShootBullet();
                Timer = TimeShoot;
            }
        }

        #region Interfaces
        #region IDamageable
        public void Damage(float _damage, GameObject _attacker)
        {
            Life -= _damage;
            if (Life < 1)
            {
                IsInactive = true;
            }
        }
        #endregion
        #region IShooter
        public List<IDamageable> GetDamageable()
        {
            return damageables;
        }
         
        public GameObject GetOwner()
        {
            return gameObject;
        }
        #endregion
        #endregion
    }
}