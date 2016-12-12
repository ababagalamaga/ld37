using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviorLava : MonoBehaviour {

    public GameObject LavaTrigger;
    public GameObject LavaObject;
    public List<GameObject> StoneObjects;  
    public List<GameObject> RealObjects;

    private Room _room;

	// Use this for initialization
	void Start () {
	    _room = GetComponent<Room>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (LavaTrigger.GetComponent<LavaTriggeredNotifier>().Triggered) {
	        _room.ObjectiveSucced = false;

	        for (int i = 0; i < StoneObjects.Count; i++) {
	            if (RealObjects.Count - 1 <= i) {
	                Instantiate(RealObjects[i], transform);
	                Destroy(StoneObjects[i]);
                }
	        }

            Destroy(LavaObject);

            Destroy(this);
	    }
	}
}
