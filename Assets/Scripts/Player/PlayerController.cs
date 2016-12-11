using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class PlayerController : MonoBehaviour {

    private PlayerMovement _playerMovement;
    private CameraController _cameraController;

    private DepthOfField _blur;
    private GameObject _bloom;

    private float _currentBlurAperture;
    private float _targetBlurAperture;
    private float _blurApertureDuration;
    private float _blurAperturePassed;
    private bool _blurApertureEnabled;
    // Use this for initialization
    void Awake () {
	    _playerMovement = GetComponent<PlayerMovement>();
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        _blur = _cameraController.GetComponent<DepthOfField>();
        _currentBlurAperture = 0.0f;
        _targetBlurAperture = 0.0f;
        _blurApertureDuration = 0.0f;
        _blurAperturePassed = 0.0f;
        _blurApertureEnabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if (_blurAperturePassed < _blurApertureDuration && _blurApertureDuration > 0.0f) {
	        _blurAperturePassed += Time.deltaTime;
	        var passedNorm = _blurAperturePassed / _blurApertureDuration;
	        _blur.aperture = _currentBlurAperture * (1 - passedNorm) + _targetBlurAperture * passedNorm;
	    } else {
	        _blur.aperture = _targetBlurAperture;
            _blur.enabled = _blurApertureEnabled;
	    }
	}

    public void ApplySettings(Room room) {
        _playerMovement.Speed = room.PlayerSpeed;
        _playerMovement.MaxVelocityChange = room.PlayerMaxVelocityChange;
        _playerMovement.JumpAcceleration = room.PlayerJumpAcceleration;

        _cameraController.Height = room.CameraHeight;
        _cameraController.HeadBobAmount = room.PlayerHeadBobAmount;
        _cameraController.HeadBobDuration = room.PlayerHeadBobDuration;
        _cameraController.HeadBobError = room.PlayerHeadBobError;

        _blurApertureEnabled = room.PlayerBlurEnabled;
        if (_blurApertureEnabled) {
            _blur.enabled = _blurApertureEnabled;
        }
        _blurApertureDuration = room.PlayerBlurDuration;
        _currentBlurAperture = _blur.aperture;
        _targetBlurAperture = room.PlayerBlurAperture;
        _blurAperturePassed = 0.0f;
    }
}
