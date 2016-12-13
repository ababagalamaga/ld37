using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float Speed;
    public float JumpAcceleration;
    public float JumpCooldown;
    public float MaxVelocityChange;
    public bool Grounded;
    public float SpeedMultiplier;
    public float HeadBobSpeedMultiplier;

    private GameObject _camera;
    private Rigidbody _rigidbody;
    
    private bool _moving;
    private bool _jumpNeeded;
    private float _jumpCooldown;
    private Vector3 _prevPosition;

    // Use this for initialization
    void Start () {
		_camera = GameObject.FindGameObjectWithTag("MainCamera");
	    _rigidbody = GetComponent<Rigidbody>();
        _moving = false;
    }

    public void SetCamera(GameObject cameraObject) {
        _camera = cameraObject;
    }
	
	// Update is called once per frame
	void Update () {
	    if (_jumpCooldown > 0.0f) {
	        _jumpCooldown -= Time.deltaTime;
        }
	    if (Input.GetButtonDown("Jump") && _jumpCooldown <= 0.0f) {
	        _jumpNeeded = true;
	    }
	}

    void FixedUpdate() {
        if (Vector3.Distance(transform.position, _prevPosition) < 0.001f)
            Grounded = true;

        if (Grounded) {
            var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (targetVelocity.magnitude > 1.0f) {
                targetVelocity = targetVelocity.normalized;
            }

            targetVelocity = _camera.transform.TransformDirection(targetVelocity);
            targetVelocity *= Speed * SpeedMultiplier * HeadBobSpeedMultiplier;
            var v = _rigidbody.velocity;
            var velocityChange = (targetVelocity - v);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -MaxVelocityChange, MaxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -MaxVelocityChange, MaxVelocityChange);
            velocityChange.y = 0;

            _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        if (_jumpNeeded && Grounded) {
            _rigidbody.AddForce(transform.up * JumpAcceleration / _rigidbody.mass);
            _jumpCooldown = JumpCooldown;
            _jumpNeeded = false;
        }

        var vel = _rigidbody.velocity.magnitude;
        _moving = vel > 0.01f && Grounded;

        Grounded = false;
        _camera.GetComponent<CameraController>().SetPlayerSpeed(_moving ? vel : 0.0f);
        _prevPosition = transform.position;
    }

    public bool Moving() {
        return _moving;
    }

    void OnCollisionStay(Collision collision) {
        if (collision.transform.tag != "Not Ground") {
            Grounded = true;
        }
    }
}
