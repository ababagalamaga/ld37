using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public bool Selected;

    public bool IsGlasses;
    public bool IsBottle;
    public bool IsKey;

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
            var currentRoom = playerController.CurrentRoom();
            if (currentRoom != null) {
                currentRoom.ObjectiveSucced = true;
            }
        }

        if (IsBottle) {
            var currentRoom = playerController.CurrentRoom();
            if (currentRoom != null) {
                var behavior4 = currentRoom.GetComponent<RoomBehavior4>();
                var behavior5 = currentRoom.GetComponent<RoomBehavior5>();

                if (behavior4 != null) {
                    behavior4.PickBottle();
                    var value = (float)behavior5.BottlesLeft() / behavior5.Bottles;
                    playerController.ApplyBlurSettings(value > 0.0f, 0.1f, value);
                    if (behavior5.BottlesLeft() == 0) {
                        currentRoom.ObjectiveSucced = true;
                    }
                } else if (behavior5 != null) {
                    behavior5.PickBottle();
                    var value = (float)behavior5.BottlesLeft() / behavior5.Bottles;
                    playerController.ApplyBlurSettings(value > 0.0f, 0.1f, value);
                    if (behavior5.BottlesLeft() == 0) {
                        currentRoom.ObjectiveSucced = true;
                    }
                }
            }
        }

        if (IsKey) {
            var currentRoom = playerController.CurrentRoom();
            if (currentRoom != null) {
                currentRoom.ObjectiveSucced = true;
            }
        }
        Destroy(gameObject);
    }
}
