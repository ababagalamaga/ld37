using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public float CameraHeight;
    public float BloorAmount;
    public float Speed;

    private bool _moved;
    private GameController _gameController;

    // Use this for initialization
    void Start () {
        _gameController = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameController>();
        _moved = false;
	}

    public void Initialize() {
        
    }

    void OnTriggerEnter(Collider other) {
        if (!_moved) {
            _gameController.MoveToNext();
            _moved = true;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
