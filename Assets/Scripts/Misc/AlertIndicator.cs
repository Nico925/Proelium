using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackFox;
using DG.Tweening;


public class AlertIndicator : MonoBehaviour {
    
    Image Indicator;

    private bool _offScreen;

    public bool OffScreen {
        get { return _offScreen; }
        set {
            _offScreen = value;
            Indicator.gameObject.SetActive(_offScreen);
        }
    }

    private void Start()
    {
        Indicator = Instantiate(Resources.Load<GameObject>("Prefabs/UI/AlertIndicator")).GetComponent<Image>();
        Indicator.transform.SetParent(GameManager.Instance.UiMng.canvasGame.transform);
    }

    void Update ()
    {
        float offset = 0;//Indicator.rectTransform.sizeDelta.x;
        //OffScreen = false;
        Indicator.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.z, transform.position.y));
        Indicator.transform.position = new Vector3(Indicator.transform.position.x, Screen.height - Indicator.transform.position.y, Indicator.transform.position.z);

        if (Indicator.transform.position.x > Screen.width)
        {
            Indicator.transform.position = new Vector3(Screen.width - offset, Indicator.transform.position.y, 0);
            OffScreen = true;
        }

        if (Indicator.transform.position.x < 0)
        {
            Indicator.transform.position = new Vector3(0 + offset, Indicator.transform.position.y, 0);
            OffScreen = true;
        }

        if (Indicator.transform.position.y > Screen.height)
        {
            Indicator.transform.position = new Vector3(Indicator.transform.position.x, Screen.height - offset, 0);
            OffScreen = true;
        }

        if (Indicator.transform.position.y < 0)
        {
            Indicator.transform.position = new Vector3(Indicator.transform.position.x, 0 + offset,0);
            OffScreen = true;
        }  
    }

    private void OnDestroy()
    {
		if(Indicator != null)
        	Destroy(Indicator.gameObject);
    }
}
