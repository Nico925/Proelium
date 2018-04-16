using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class RotateOnPosition : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 30);
        }
    }
}
