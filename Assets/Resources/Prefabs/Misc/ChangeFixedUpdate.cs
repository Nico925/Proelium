using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFixedUpdate : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            Time.fixedDeltaTime += Time.fixedDeltaTime / 10;
            Debug.Log(Time.fixedDeltaTime);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Time.fixedDeltaTime -= Time.fixedDeltaTime / 10;
            Debug.Log(Time.fixedDeltaTime);
        }
    }
}
