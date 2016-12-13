using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior6 : MonoBehaviour {

    public GameObject Mom;
    private Room _room;

    // Use this for initialization
    void Start () {
        _room = GetComponent<Room>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (_room.PlayerInRoom()) {
	        if (Mom.GetComponent<MomSuccessNotifier>() != null && Mom.GetComponent<MomSuccessNotifier>().Triggered) {
	            _room.ObjectiveSucced = true;

	            StartCoroutine(End());
	        }
	    }
	}

    IEnumerator End() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().RotationEnabled(false);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyVignetteSettings(true, 5, 1.0f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyContrastSettings(true, 5, 1.0f);

        for(float passed = 0f; passed <= 10; passed += Time.deltaTime) {
            yield return null;
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().RestartGame();
    }
}
