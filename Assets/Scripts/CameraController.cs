using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float LookLerpCoeff;
    public float MoveLerpCoeff;

    public float HeadBobAmount;
    public float HeadBobStandingMult;
    public float HeadBobDuration;
    public float HeadBobLerp;
    public float HeadBobMinSpeed;
    public float HeadBobError;
    public float HeadBobSpeedInfluence;
    public float HeadBobRotationDuration;
    public float HeadBobRotationAmount;
    public float HeadBobRotationPhase;
    public float HeadBobRotationPhaseMult;
    public float HeadBobRotationLerp;
    public float HeadBobForwardAmount;
    public float HeadBobForwardPhase;
    public float HeadBobForwardPhaseMult;
    public float HeadBobForwardLerp;
    public float HeadBobForwardDuration;
    public AudioClip RightStep;
    public AudioClip LeftStep;

    private GameObject _playerController;
    private float _targetHeight;
    private float _currentHeight;
    private Vector3 _headBobOffset;
    private Quaternion _headBobRotation;
    private Quaternion _headBobForward;
    private float _headBobPassed;
    private float _headBobRotationPassed;
    private float _headBobForwardPassed;
    private float _playerSpeed;
    private Quaternion _realRotation;
    private AudioSource _audioSource;
    private float step = 1;

    // Use this for initialization
    void Start () {
	    _playerController = GameObject.FindGameObjectWithTag("Player");
        _currentHeight = 1.6f;
        _headBobOffset = new Vector3(0, 0, 0);
        _headBobRotation = Quaternion.identity;
        _headBobForward = Quaternion.identity;
        _realRotation = transform.rotation;
        _audioSource = transform.FindChild("Audio Source").GetComponent<AudioSource>();
    }

    public void SetPlayerSpeed(float speed) {
        _playerSpeed = speed;
    }

    public void SetHeight(float height) {
        _targetHeight = height;
    }

    void Update() {
        _currentHeight = Mathf.Lerp(_currentHeight, _targetHeight, Time.deltaTime * 2.0f);

        if ((_playerSpeed > HeadBobMinSpeed || HeadBobStandingMult > 0.0f) && HeadBobDuration > 0.0f && HeadBobRotationDuration > 0.0f) {
            _headBobPassed += Time.deltaTime;
            if (_headBobPassed > HeadBobDuration) {
                _headBobPassed -= HeadBobDuration;
            }

            _headBobRotationPassed += Time.deltaTime;
            if (_headBobRotationPassed > HeadBobRotationDuration) {
                _headBobRotationPassed -= HeadBobRotationDuration;
            }

            _headBobForwardPassed += Time.deltaTime;
            if (_headBobForwardPassed > HeadBobForwardDuration) {
                _headBobForwardPassed -= HeadBobForwardDuration;
            }

            if (HeadBobForwardDuration > 0.0f) {
                var passedForwardNorm = _headBobForwardPassed / HeadBobForwardDuration;
                passedForwardNorm = Mathf.Pow(passedForwardNorm, HeadBobError > 0.0f ? HeadBobError : 1.0f);
                var forwardDeltaX = Mathf.Sin(((passedForwardNorm * Mathf.PI * 2.0f) + HeadBobForwardPhase) * HeadBobForwardPhaseMult) * HeadBobForwardAmount;
                var forwardDeltaY = Mathf.Cos(((passedForwardNorm * Mathf.PI * 2.0f) + HeadBobForwardPhase + Mathf.PI * 1.0f) * HeadBobForwardPhaseMult) * HeadBobForwardAmount;

                if (_playerSpeed <= HeadBobMinSpeed) {
                    forwardDeltaX *= HeadBobStandingMult;
                    forwardDeltaY *= HeadBobStandingMult;
                }

                _headBobForward = Quaternion.Lerp(_headBobForward, Quaternion.LookRotation(new Vector3(forwardDeltaX, forwardDeltaY, HeadBobForwardAmount).normalized), Time.deltaTime * HeadBobForwardLerp);
            }

            var passedNorm = _headBobPassed / HeadBobDuration;
            var passedRotationNorm = _headBobRotationPassed / HeadBobRotationDuration;
            passedNorm = Mathf.Pow(passedNorm, HeadBobError > 0.0f ? HeadBobError : 1.0f);
            passedRotationNorm = Mathf.Pow(passedRotationNorm, HeadBobError > 0.0f ? HeadBobError : 1.0f);

            var posDelta = Mathf.Sin(passedNorm * Mathf.PI * 2.0f) * HeadBobAmount;
            var rotDelta = Mathf.Sin(((passedRotationNorm * Mathf.PI * 2.0f) + HeadBobRotationPhase) * HeadBobRotationPhaseMult) * HeadBobRotationAmount;

            if (_playerSpeed <= HeadBobMinSpeed) {
                posDelta *= HeadBobStandingMult;
                rotDelta *= HeadBobStandingMult;
            } else {
                if (posDelta > 0.7 * HeadBobAmount && step > 0)
                {
                    step = -1;
                    _audioSource.PlayOneShot(RightStep, 0.025f);
                }
                if (posDelta < -0.7 * HeadBobAmount && step < 0)
                {
                    step = 1;
                    _audioSource.PlayOneShot(LeftStep, 0.025f);
                }
            }

            _headBobOffset = Vector3.Lerp(_headBobOffset, new Vector3(0, posDelta, 0), Time.deltaTime * HeadBobLerp);
            _headBobRotation = Quaternion.Lerp(_headBobRotation, Quaternion.LookRotation(Vector3.forward, new Vector3(rotDelta, 1, 0).normalized), Time.deltaTime * HeadBobRotationLerp);

            var speedMult = 1.0f;
            if (HeadBobAmount > 0.0f) {
                speedMult = (_headBobOffset.y + HeadBobAmount) * 0.5f / HeadBobAmount;
            }

            _playerController.GetComponent<PlayerMovement>().HeadBobSpeedMultiplier = 1.0f - HeadBobSpeedInfluence + HeadBobSpeedInfluence * speedMult;
        } else {
            _headBobPassed = 0;
            _headBobRotationPassed = 0;
            _headBobOffset = Vector3.Lerp(_headBobOffset, new Vector3(0, 0, 0), Time.deltaTime * HeadBobLerp);
            _headBobRotation = Quaternion.Lerp(_headBobRotation, Quaternion.identity, Time.deltaTime * HeadBobRotationLerp);
            _headBobForward = Quaternion.Lerp(_headBobForward, Quaternion.identity, Time.deltaTime * HeadBobRotationLerp);
            var speedMult = 1.0f;
            if (HeadBobAmount > 0.0f) {
                speedMult = (_headBobOffset.y + HeadBobAmount) * 0.5f / HeadBobAmount;
            }
            _playerController.GetComponent<PlayerMovement>().HeadBobSpeedMultiplier = 1.0f - HeadBobSpeedInfluence + HeadBobSpeedInfluence * speedMult;
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
        _realRotation = Quaternion.Lerp(_playerController.GetComponent<MouseLook>().Rotation(), _realRotation,
            Time.deltaTime * LookLerpCoeff);
	    _realRotation = Quaternion.LookRotation(_realRotation * Vector3.forward);

	    transform.rotation = _realRotation * _headBobRotation * _headBobForward;

        transform.position = Vector3.Lerp(_playerController.transform.position + Vector3.up * _currentHeight + _headBobOffset, transform.position, Time.deltaTime * MoveLerpCoeff);
	}
}
