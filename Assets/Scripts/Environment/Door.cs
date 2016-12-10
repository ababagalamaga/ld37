using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _triggered;

	// Use this for initialization
	void Start () {
	    _triggered = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other) {
        _triggered = true;
        Debug.Log("triggered");
        //Destroy(other.gameObject);
    }
}
