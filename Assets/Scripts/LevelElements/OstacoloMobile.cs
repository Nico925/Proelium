using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{

    public class OstacoloMobile : MonoBehaviour
    {
        
        public float ForceMultiplier = 1;
        public float SpeedRotation = 2;
        public float damage = 1;
        Rigidbody rigid;
        List<IDamageable> damageables = new List<IDamageable>();                        // Lista di Oggetti facenti parte dell'interfaccia IDamageable
        Core core;


        // Use this for initialization
        void Start()
        {
            core = FindObjectOfType<Core>();
            rigid = GetComponent<Rigidbody>();
            //Push the MobileObstacle outword from the Core
            Vector3 initDir = transform.position - core.transform.position;
            rigid.MoveRotation( Quaternion.LookRotation(initDir));
            rigid.AddForce(initDir.normalized * ForceMultiplier, ForceMode.Impulse);
        }

        private void Update()
        {
            rigid.AddForce(rigid.velocity.normalized * ForceMultiplier, ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null && collision.gameObject.GetComponent<Core>() == null)
            {
                damageable.Damage(damage, gameObject);
            }
        }
    }
}