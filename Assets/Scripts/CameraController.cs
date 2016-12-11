using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float LookLerpCoeff;
    public float MoveLerpCoeff;
    public float Height;
    public float HeadBobAmount;
    public float HeadBobDuration;
    public float HeadBobLerp;
    public float HeadBobMinSpeed;
    public float HeadBobError;
    public float PlayerSpeed;

    private GameObject _playerController;
    private Vector3 _offset;
    private Vector3 _headBobOffset;
    private float _headBobPassed;
    
    // Use this for initialization
    void Start () {
	    _playerController = GameObject.FindGameObjectWithTag("Player");
        _offset = new Vector3(0, Height, 0);
        _headBobOffset = new Vector3(0, 0, 0);
    }

    void Update() {
        if (PlayerSpeed > HeadBobMinSpeed) {
            _headBobPassed += Time.deltaTime;
            if (_headBobPassed > HeadBobDuration) {
                _headBobPassed -= HeadBobDuration;
            }

            var passedNorm = _headBobPassed / HeadBobDuration;
            var delta = Mathf.Sin(Mathf.Pow(passedNorm, HeadBobError) * Mathf.PI * 2.0f) * HeadBobAmount;
            _headBobOffset = Vector3.Lerp(_headBobOffset, new Vector3(0, delta, 0), Time.deltaTime * HeadBobLerp);
        } else {
            _headBobPassed = 0;
            _headBobOffset = Vector3.Lerp(_headBobOffset, new Vector3(0, 0, 0), Time.deltaTime * HeadBobLerp);
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
	    transform.rotation = Quaternion.Lerp(_playerController.transform.rotation, transform.rotation,
	        Time.deltaTime * LookLerpCoeff);

        transform.position = Vector3.Lerp(_playerController.transform.position + _offset + _headBobOffset, transform.position, Time.deltaTime * MoveLerpCoeff);
	}
}
