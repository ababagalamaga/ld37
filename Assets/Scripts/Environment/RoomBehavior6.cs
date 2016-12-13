using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior6 : MonoBehaviour {

    public GameObject Mom;
    private Room _room;
    private GameObject _player;

    // Use this for initialization
    void Start () {
        _room = GetComponent<Room>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
	    if (_room.PlayerInRoom()) {
	        if (Mom.GetComponent<MomSuccessNotifier>() != null && Mom.GetComponent<MomSuccessNotifier>().Triggered) {
	            _room.ObjectiveSucced = true;
                Time.timeScale = 0;
            }
	    }
	}
}
