using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public bool Selected;
    public bool IsGlasses;

    private bool _nextFrameHide;

	// Use this for initialization
	void Start () {
	    _nextFrameHide = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Selected) {
	        GetComponent<Renderer>().material.SetFloat("_Outline", 0.017f);
	    } else {
	        GetComponent<Renderer>().material.SetFloat("_Outline", 0.0f);
        }
	}

    void LateUpdate() {
        if (_nextFrameHide) {
            Selected = false;
            _nextFrameHide = false;
        }
        if (Selected) {
            _nextFrameHide = true;
        }
    }

    public void Pick(PlayerController playerController) {
        if (IsGlasses) {
           playerController.ApplyBlurSettings(false, 0.1f, 0.0f);
        }
        Destroy(gameObject);
    }
}
