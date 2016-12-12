using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

    public float Sensitivity;
    public float MaxUp;
    public float MinDown;

    private Transform _transform;
    private bool _rotationEnabled;

	// Use this for initialization
	void Start () {
	    _transform = new GameObject().transform;
	    _transform.rotation = transform.rotation;
	    _rotationEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (_rotationEnabled) {
            _transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * Sensitivity, 0), Space.World);
            _transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * Sensitivity, 0, 0));

	        var rotation = _transform.rotation;
	        var angles = rotation.eulerAngles;
	        if (angles.x > 0.0f && angles.x < 90.0f) {
	            angles.x = Mathf.Clamp(angles.x, 0.0f, MinDown);
            } else if (angles.x > 270.0f && angles.x < 360.0f) {
	            angles.x = Mathf.Clamp(angles.x, MaxUp, 360.0f);
            }

            _transform.rotation = Quaternion.Euler(angles);
	    }
	}

    public Quaternion Rotation() {
        return _transform.rotation;
    }

    public void RotationEnabled(bool enabled) {
        _rotationEnabled = enabled;
    }
}
