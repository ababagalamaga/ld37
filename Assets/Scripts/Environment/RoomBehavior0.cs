using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior0 : MonoBehaviour
{
    public bool ObjectiveSucced = false;

    private GameController _gameController;

	// Use this for initialization
	void Start () {
	    _gameController = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (ObjectiveSucced) {
	        _gameController.UnlockNext();
            Destroy(this);
	    }
	}
}
