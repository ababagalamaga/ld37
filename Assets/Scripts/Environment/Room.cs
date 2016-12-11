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
    void Awake () {
        _gameController = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameController>();
        _moved = false;

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
            if (child.gameObject.GetComponent<BoxCollider>() != null) {
                child.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            if (child.gameObject.GetComponent<Rigidbody>() != null) {
                child.gameObject.GetComponent<Rigidbody>().Sleep();
            }
            if (child.gameObject.GetComponent<MeshRenderer>() != null) {
                child.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
	}

    public void Initialize() {
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
            if (child.gameObject.GetComponent<BoxCollider>() != null) {
                child.gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            if (child.gameObject.GetComponent<Rigidbody>() != null) {
                child.gameObject.GetComponent<Rigidbody>().WakeUp();
            }
            if (child.gameObject.GetComponent<MeshRenderer>() != null) {
                child.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!_moved) {
            _gameController.MoveToNext();
            _moved = true;
            Debug.Log("HERE");
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
