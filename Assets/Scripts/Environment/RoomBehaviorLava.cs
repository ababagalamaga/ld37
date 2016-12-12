using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviorLava : MonoBehaviour
{
    public bool ObjectiveSucced = false;

    private Room _room;

	// Use this for initialization
	void Start () {
	    _room = GetComponent<Room>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (ObjectiveSucced) {
	        _room.ObjectiveSucced = true;
            Destroy(this);
	    }
	}
}
