using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private PlayerMovement _playerMovement;
    private CameraController _cameraController;
    // Use this for initialization
    void Awake () {
	    _playerMovement = GetComponent<PlayerMovement>();
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ApplySettings(Room room) {
        _playerMovement.Speed = room.PlayerSpeed;
        _playerMovement.MaxVelocityChange = room.PlayerMaxVelocityChange;
        _playerMovement.JumpAcceleration = room.PlayerJumpAcceleration;

        _cameraController.Height = room.CameraHeight;
        _cameraController.HeadBobAmount = room.PlayerHeadBobAmount;
        _cameraController.HeadBobDuration = room.PlayerHeadBobDuration;
        _cameraController.HeadBobError = room.PlayerHeadBobError;
    }
}
