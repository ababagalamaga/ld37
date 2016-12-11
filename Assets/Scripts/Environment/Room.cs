using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Room : MonoBehaviour {

    public float CameraHeight;
    public float BloorAmount;
    public float PlayerSpeed;
    public float PlayerMaxVelocityChange;
    public float PlayerJumpAcceleration;
    public bool ObjectiveSucced;

    private bool _initialized;
    private bool _playerInRoom;
    private PlayerController _playerController;

    // Use this for initialization
    void Awake () {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _initialized = false;
        _playerInRoom = false;
        ObjectiveSucced = false;

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

        _playerController.ApplySettings(this);
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
