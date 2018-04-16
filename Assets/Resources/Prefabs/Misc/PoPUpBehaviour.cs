using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PoPUpBehaviour : MonoBehaviour {
    public float AscensionMultiplier = 10;
	// Use this for initialization
	void Start () {
        transform.DOScale(new Vector3(1f, 1f, 1f), 1f).OnComplete(() => {
            transform.localScale = Vector3.zero;
        }).SetEase(Ease.OutBounce);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * AscensionMultiplier, transform.position.z);
	}
}
