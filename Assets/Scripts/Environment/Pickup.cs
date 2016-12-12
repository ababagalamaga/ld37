﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public bool Selected;
    public bool IsGlasses;
    public string Property;
    public Color Highlight;
    public Color Real;

    private bool _nextFrameHide;

	// Use this for initialization
	void Start () {
	    _nextFrameHide = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Selected) {
	        GetComponent<Renderer>().material.SetColor(Property, Highlight);
	    } else {
	        GetComponent<Renderer>().material.SetColor(Property, Real);
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
