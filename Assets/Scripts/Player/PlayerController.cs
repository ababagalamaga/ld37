using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class PlayerController : MonoBehaviour {

    public LayerMask RaycastMask;
    public float MaxRaycastDist;

    private PlayerMovement _playerMovement;
    private CameraController _cameraController;
    private Room _currentRoom;
    private MouseLook _mouseLook;

    private DepthOfField _blur;
    private VignetteAndChromaticAberration _vignette;
    private ContrastStretch _tonemapping;

    private float _currentBlurAperture;
    private float _targetBlurAperture;
    private float _blurApertureDuration;
    private float _blurAperturePassed;
    private bool _blurApertureEnabled;

    private bool _vignetteEnabled;
    private float _currentVignetteValue;
    private float _targetVignetteValue;
    private float _vignetteDuration;
    private float _vignettePassed;

    private bool _tonemappingEnabled;
    private float _currentTonemappingValue;
    private float _targetTonemappingValue;
    private float _tonemappingDuration;
    private float _tonemappingPassed;

    // Use this for initialization
    void Awake () {
	    _playerMovement = GetComponent<PlayerMovement>();
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        _mouseLook = GetComponent<MouseLook>();

        _blur = _cameraController.GetComponent<DepthOfField>();
        _currentBlurAperture = 0.0f;
        _targetBlurAperture = 0.0f;
        _blurApertureDuration = 0.0f;
        _blurAperturePassed = 0.0f;
        _blurApertureEnabled = false;

        _vignette = _cameraController.GetComponent<VignetteAndChromaticAberration>();

        _vignetteEnabled = false;
        _currentVignetteValue = 0.0f;
        _targetVignetteValue = 0.0f;
        _vignetteDuration = 0.0f;
        _vignettePassed = 0.0f;

        _tonemapping = _cameraController.GetComponent<ContrastStretch>();

        _tonemappingEnabled = false;
        _currentTonemappingValue = 0.0f;
        _targetTonemappingValue = 0.0f;
        _tonemappingDuration = 0.0f;
        _tonemappingPassed = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
	    if (_blurAperturePassed < _blurApertureDuration && _blurApertureDuration > 0.0f) {
	        _blurAperturePassed += Time.deltaTime;
	        var passedNorm = _blurAperturePassed / _blurApertureDuration;
	        _blur.aperture = Mathf.Clamp((_currentBlurAperture * (1 - passedNorm) + _targetBlurAperture * passedNorm), Mathf.Min(_currentBlurAperture, _targetBlurAperture), Mathf.Max(_currentBlurAperture, _targetBlurAperture));
	    } else {
	        _blur.aperture = _targetBlurAperture;
            _blur.enabled = _blurApertureEnabled;
	    }

        if (_vignettePassed < _vignetteDuration && _vignetteDuration > 0.0f) {
            _vignettePassed += Time.deltaTime;
	        var passedNorm = _vignettePassed / _vignetteDuration;
	        _vignette.intensity = Mathf.Clamp((_currentVignetteValue * (1 - passedNorm) + _targetVignetteValue * passedNorm), Mathf.Min(_currentVignetteValue, _targetVignetteValue), Mathf.Max(_currentVignetteValue, _targetVignetteValue));
	    } else {
	        _vignette.intensity = _targetVignetteValue;
            _vignette.enabled = _vignetteEnabled;
	    }

        if (_tonemappingPassed < _tonemappingDuration && _tonemappingDuration > 0.0f) {
            _tonemappingPassed += Time.deltaTime;
	        var passedNorm = _tonemappingPassed / _tonemappingDuration;
            _tonemapping.limitMinimum = Mathf.Clamp(_currentTonemappingValue * (1 - passedNorm) + _targetTonemappingValue * passedNorm, Mathf.Min(_currentTonemappingValue, _targetTonemappingValue), Mathf.Max(_currentTonemappingValue, _targetTonemappingValue));
	    } else {
            _tonemapping.limitMinimum = _targetTonemappingValue;
            _tonemapping.enabled = _tonemappingEnabled;
	    }

        RaycastHit hit;
        Ray ray = _cameraController.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, MaxRaycastDist, RaycastMask)) {
            Transform objectHit = hit.transform;
            if (objectHit.gameObject.GetComponent<Pickup>() != null) {
                var pickup = objectHit.gameObject.GetComponent<Pickup>();
                pickup.Selected = true;
                if (Input.GetButtonDown("Fire1")) {
                    pickup.Pick(this);
                }
            }

            // Do something with the object that was hit by the raycast.
        }
    }

    public void ApplySettings(Room room) {
        _currentRoom = room;

        _playerMovement.Speed = room.PlayerSpeed;
        _playerMovement.MaxVelocityChange = room.PlayerMaxVelocityChange;
        _playerMovement.JumpAcceleration = room.PlayerJumpAcceleration;

        _cameraController.SetHeight(room.CameraHeight);
        _cameraController.HeadBobAmount = room.PlayerHeadBobAmount;
        _cameraController.HeadBobMinSpeed = room.PlayerHeadBobMinSpeed;
        _cameraController.HeadBobDuration = room.PlayerHeadBobDuration;
        _cameraController.HeadBobError = room.PlayerHeadBobError;
        _cameraController.HeadBobSpeedInfluence = room.PlayerHeadBobSpeedInfluence;
        _cameraController.HeadBobRotationAmount = room.PlayerHeadBobRotationAmount;
        _cameraController.HeadBobRotationPhase = room.PlayerHeadBobRotationPhase;
        _cameraController.HeadBobRotationPhaseMult = room.PlayerHeadBobRotationPhaseMult;
        _cameraController.HeadBobRotationLerp = room.PlayerHeadBobRotationLerp;
        _cameraController.HeadBobRotationDuration = room.PlayerHeadBobRotationDuration;
        _cameraController.HeadBobForwardAmount = room.PlayerHeadBobForwardAmount;
        _cameraController.HeadBobForwardPhase = room.PlayerHeadBobForwardPhase;
        _cameraController.HeadBobForwardPhaseMult = room.PlayerHeadBobForwardPhaseMult;
        _cameraController.HeadBobForwardLerp = room.PlayerHeadBobForwardLerp;
        _cameraController.HeadBobForwardDuration = room.PlayerHeadBobForwardDuration;
        _cameraController.LeftStep = room.LeftStep;
        _cameraController.RightStep = room.RightStep;

        _mouseLook.MaxUp = room.MaxCameraAngle;
        _mouseLook.MinDown = room.MinCameraAngle;

        _blurApertureEnabled = room.PlayerBlurEnabled;
        if (_blurApertureEnabled) {
            _blur.enabled = _blurApertureEnabled;
        }
        _blurApertureDuration = room.PlayerBlurDuration;
        _currentBlurAperture = _blur.aperture;
        _targetBlurAperture = room.PlayerBlurAperture;
        _blurAperturePassed = 0.0f;

        _vignetteEnabled = room.PlayerVignetteEnabled;
        if (_vignetteEnabled) {
            _vignette.enabled = _vignetteEnabled;
        }
        _vignetteDuration = room.PlayerVignetteDuration;
        _currentVignetteValue = _vignette.intensity;
        _targetVignetteValue = room.PlayerVignetteValue;
        _vignettePassed = 0.0f;

        _tonemappingEnabled = room.PlayerVignetteEnabled;
        if (_tonemappingEnabled) {
            _tonemapping.enabled = _tonemappingEnabled;
        }
        _tonemappingDuration = room.PlayerTonemappingDuration;
        _currentTonemappingValue = _tonemapping.limitMinimum;
        _targetTonemappingValue = room.PlayerTonemappingValue;
        _tonemappingPassed = 0.0f;
    }

    public void ApplyBlurSettings(bool enabled, float duration, float value) {
        _blurApertureEnabled = enabled;
        if (_blurApertureEnabled) {
            _blur.enabled = _blurApertureEnabled;
        }
        _blurApertureDuration = duration;
        _currentBlurAperture = _blur.aperture;
        _targetBlurAperture = value;
        _blurAperturePassed = 0.0f;
    }

    public void ApplyVignetteSettings(bool enabled, float duration, float value) {
        _vignetteEnabled = enabled;
        if (_vignetteEnabled) {
            _vignette.enabled = _vignetteEnabled;
        }
        _vignetteDuration = duration;
        _currentVignetteValue = _vignette.intensity;
        _targetVignetteValue = value;
        _vignettePassed = 0.0f;
    }

    public void ApplyContrastSettings(bool enabled, float duration, float value) {
        _tonemappingEnabled = enabled;
        if (_tonemappingEnabled) {
            _tonemapping.enabled = _tonemappingEnabled;
        }
        _tonemappingDuration = duration;
        _currentTonemappingValue = _tonemapping.limitMinimum;
        _targetTonemappingValue = value;
        _tonemappingPassed = 0.0f;
    }

    public Room CurrentRoom() {
        return _currentRoom;
    }
}
