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

    private GameObject _playerController;
    private Vector3 _offset;
    private Vector3 _headBobOffset;
    private float _headBobPassed;
    private bool _headBobMoving;

    // Use this for initialization
    void Start () {
	    _playerController = GameObject.FindGameObjectWithTag("Player");
        _offset = new Vector3(0, Height, 0);
        _headBobOffset = new Vector3(0, 0, 0);
	}

    void Update() {
        if (_headBobMoving) {
            _headBobPassed += Time.deltaTime;
            if (_headBobPassed > HeadBobDuration) {
                _headBobPassed = 0;
            }

            var delta = Mathf.Sin(_headBobPassed) * HeadBobAmount;
            _headBobOffset = Vector3.Lerp(_headBobOffset, new Vector3(0, delta, 0), Time.deltaTime * HeadBobLerp);
        } else {
            _headBobOffset = Vector3.Lerp(_headBobOffset, new Vector3(0, 0, 0), Time.deltaTime * HeadBobLerp);
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
	    transform.rotation = Quaternion.Lerp(_playerController.transform.rotation, transform.rotation,
	        Time.deltaTime * LookLerpCoeff);

	    var delta = _playerController.transform.position - transform.position;
	    _headBobMoving = delta.magnitude / Time.deltaTime > HeadBobMinSpeed;

        transform.position = Vector3.Lerp(_playerController.transform.position + _offset, transform.position, Time.deltaTime * MoveLerpCoeff);
	}
}
