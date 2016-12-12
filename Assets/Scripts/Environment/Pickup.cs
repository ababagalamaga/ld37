using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public bool Selected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Selected) {
	        GetComponent<Renderer>().material.SetFloat("_Outline", 0.017f);
	    } else {
	        GetComponent<Renderer>().material.SetFloat("_Outline", 0.0f);
        }
    }
}
