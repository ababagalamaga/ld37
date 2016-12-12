using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior4 : MonoBehaviour {

    public int Bottles;

    private int _bottles;

	// Use this for initialization
	void Start () {
	    _bottles = Bottles;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PickBottle() {
        _bottles -= 1;
    }

    public int BottlesLeft() {
        return _bottles;
    }
}
