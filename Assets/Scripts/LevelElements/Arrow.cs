using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    public class Arrow : MonoBehaviour
    {
        public float Force = 1000;

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Avatar>() != null)
            {
                other.GetComponent<Rigidbody>().AddForce(transform.forward * Force, ForceMode.Acceleration);
            }
        }
    }
}
