using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public float CameraHeight;
    public float BloorAmount;
    public float Speed;

    private bool _moved;
    private bool _initialized;
    private bool _playerInRoom;
    private GameController _gameController;

    // Use this for initialization
    void Awake () {
        _gameController = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameController>();
        _moved = false;
        _initialized = false;
        _playerInRoom = false;

        DeInitialize();
	}

    private void InitTransform(Transform transform) {
        if (transform.gameObject.GetComponent<BoxCollider>() != null) {
            transform.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        if (transform.gameObject.GetComponent<Rigidbody>() != null) {
            transform.gameObject.GetComponent<Rigidbody>().WakeUp();
        }
        if (transform.gameObject.GetComponent<MeshRenderer>() != null) {
            transform.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        foreach (Transform child in transform) {
            InitTransform(child);
        }
    }

    private void DeInitTransform(Transform transform) {
        if (transform.gameObject.GetComponent<BoxCollider>() != null) {
            transform.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        if (transform.gameObject.GetComponent<Rigidbody>() != null) {
            transform.gameObject.GetComponent<Rigidbody>().Sleep();
        }
        if (transform.gameObject.GetComponent<MeshRenderer>() != null) {
            transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        foreach (Transform child in transform) {
            DeInitTransform(child);
        }
    }

    public void Initialize() {
        InitTransform(transform);

        _initialized = true;
    }

    public void DeInitialize() {
        DeInitTransform(transform);

        _initialized = false;
    }

    public bool Initialized() {
        return _initialized;
    }

    void OnTriggerEnter(Collider other) {
        _playerInRoom = true;
    }

    void OnTriggerExit(Collider other) {
        _playerInRoom = false;
    }

    public bool PlayerInRoom() {
        return _playerInRoom;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
