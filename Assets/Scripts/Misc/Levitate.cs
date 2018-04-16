using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Levitate : MonoBehaviour {

    float amplitude = 1;
	
	// Update is called once per frame
	void Update () {
        amplitude = Mathf.Sin(Time.time)/100;
        transform.position = new Vector3(transform.position.x, transform.position.y + amplitude, transform.position.z);
	}
}
