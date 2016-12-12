using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior2 : MonoBehaviour {

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
            var playerMovement = _player.GetComponent<PlayerMovement>();
	        playerMovement.SpeedMultiplier = 1.0f;

            /*if (ObjectiveSucced) {
                _room.ObjectiveSucced = true;
                Destroy(this);
            }*/
        }
	}
}
