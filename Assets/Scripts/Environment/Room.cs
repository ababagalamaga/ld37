using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Room : MonoBehaviour {

    public float CameraHeight;
    public float PlayerSpeed;
    public float PlayerMaxVelocityChange;
    public float PlayerJumpAcceleration;
    public float PlayerHeadBobAmount;
    public float PlayerHeadBobStandingMult;
    public float PlayerHeadBobMinSpeed;
    public float PlayerHeadBobDuration;
    public float PlayerHeadBobError;
    public float PlayerHeadBobSpeedInfluence;
    public float PlayerHeadBobRotationAmount;
    public float PlayerHeadBobRotationPhase;
    public float PlayerHeadBobRotationPhaseMult;
    public float PlayerHeadBobRotationLerp;
    public float PlayerHeadBobRotationDuration;
    public float PlayerHeadBobForwardAmount;
    public float PlayerHeadBobForwardPhase;
    public float PlayerHeadBobForwardPhaseMult;
    public float PlayerHeadBobForwardLerp;
    public float PlayerHeadBobForwardDuration;

    public AudioClip LeftStep;
    public AudioClip RightStep;

    public float MaxCameraAngle = 280.0f;
    public float MinCameraAngle = 80.0f;

    public bool PlayerBlurEnabled;
    public float PlayerBlurAperture;
    public float PlayerBlurDuration;

    public bool PlayerVignetteEnabled;
    public float PlayerVignetteValue;
    public float PlayerVignetteDuration;

    public bool PlayerTonemappingEnabled;
    public float PlayerTonemappingValue;
    public float PlayerTonemappingDuration;

    public bool ObjectiveSucced;

    private bool _initialized;
    private bool _playerInRoom;
    private PlayerController _playerController;

    // Use this for initialization
    void Awake () {
        _initialized = false;
        _playerInRoom = false;

        DeInitialize();
	}

    void Start() {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
        gameObject.SetActive(true);
        //InitTransform(transform);

        _initialized = true;
    }

    public void DeInitialize() {
        gameObject.SetActive(false);
        //DeInitTransform(transform);

        _initialized = false;
    }

    public bool Initialized() {
        return _initialized;
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            _playerInRoom = true;

            _playerController.ApplySettings(this);

            // TODO: White Screen

            var roomCamera = transform.FindChild("MainCamera");
            if (roomCamera != null) {

                var currentCamera = GameObject.FindGameObjectWithTag("MainCamera");

                roomCamera.transform.position = currentCamera.transform.position;
                roomCamera.transform.rotation = currentCamera.transform.rotation;

                Destroy(currentCamera);

                _playerController.SetCamera(roomCamera.GetComponent<CameraController>());
                _playerController.GetComponent<PlayerMovement>().SetCamera(roomCamera.gameObject);
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().SetCamera(roomCamera.gameObject);
                roomCamera.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.transform.tag == "Player") {
            _playerInRoom = false;
        }
    }

    public bool PlayerInRoom() {
        return _playerInRoom;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
