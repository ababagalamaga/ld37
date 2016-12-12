using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviorLava : MonoBehaviour {

    public GameObject LavaFailTrigger;
    public GameObject LavaSuccessTrigger;
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
	    if (LavaFailTrigger.GetComponent<LavaFailNotifier>().Triggered) {
	        _room.ObjectiveSucced = false;

	        for (int i = 0; i < StoneObjects.Count; i++) {
	            if (RealObjects.Count - 1 <= i) {
	                var realObject = Instantiate(RealObjects[i], transform);
	                realObject.transform.position = StoneObjects[i].transform.position;
	                realObject.transform.rotation = StoneObjects[i].transform.rotation;

                    Destroy(StoneObjects[i]);
                }
	        }

            Destroy(LavaObject);
            Destroy(LavaSuccessTrigger);
	        LavaSuccessTrigger = null;

            Destroy(this);
	    } else if (LavaSuccessTrigger != null && LavaSuccessTrigger.GetComponent<LavaSuccessNotifier>().Triggered) {
	        _room.ObjectiveSucced = true;
        }
    }
}
