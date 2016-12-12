using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float LookLerpCoeff;
    public float MoveLerpCoeff;

    public float HeadBobAmount;
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

    private GameObject _playerController;
    private float _targetHeight;
    private float _currentHeight;
    private Vector3 _headBobOffset;
    private Quaternion _headBobRotation;
    private float _headBobPassed;
    private float _headBobRotationPassed;
    private float _playerSpeed;
    private Quaternion _realRotation;

    // Use this for initialization
    void Start () {
	    _playerController = GameObject.FindGameObjectWithTag("Player");
        _headBobOffset = new Vector3(0, 0, 0);
        _headBobRotation = Quaternion.identity;
        _realRotation = transform.rotation;
    }

    public void SetPlayerSpeed(float speed) {
        _playerSpeed = speed;
    }

    public void SetHeight(float height) {
        _targetHeight = height;
    }

    void Update() {
        _currentHeight = Mathf.Lerp(_currentHeight, _targetHeight, Time.deltaTime * 10.0f);

        if (_playerSpeed > HeadBobMinSpeed && HeadBobDuration > 0.0f) {
            _headBobPassed += Time.deltaTime;
            if (_headBobPassed > HeadBobDuration) {
                _headBobPassed -= HeadBobDuration;
            }

            _headBobRotationPassed += Time.deltaTime;
            if (_headBobRotationPassed > HeadBobRotationDuration) {
                _headBobRotationPassed -= HeadBobRotationDuration;
            }

            var passedNorm = _headBobPassed / HeadBobDuration;
            var passedRotationNorm = _headBobRotationPassed / HeadBobRotationDuration;
            passedNorm = Mathf.Pow(passedNorm, HeadBobError > 0.0f ? HeadBobError : 1.0f);
            passedRotationNorm = Mathf.Pow(passedRotationNorm, HeadBobError > 0.0f ? HeadBobError : 1.0f);

            var posDelta = Mathf.Sin(passedNorm * Mathf.PI * 2.0f) * HeadBobAmount;
            var rotDelta = Mathf.Sin(((passedRotationNorm * Mathf.PI * 2.0f) + HeadBobRotationPhase) * HeadBobRotationPhaseMult) * HeadBobRotationAmount;

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

        transform.rotation =  _realRotation * _headBobRotation;

        transform.position = Vector3.Lerp(_playerController.transform.position + Vector3.up * _currentHeight + _headBobOffset, transform.position, Time.deltaTime * MoveLerpCoeff);
	}
}
