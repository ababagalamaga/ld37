using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Speed;
    public float JumpHeight;
    public float MaxVelocityChange;
    public bool Grounded;

    private GameObject _camera;
    private Rigidbody _rigidbody;

	// Use this for initialization
	void Start () {
		_camera = GameObject.FindGameObjectWithTag("MainCamera");
	    _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate() {
        if (Grounded) {
            var targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = _camera.transform.TransformDirection(targetVelocity);
            targetVelocity *= Speed;
            var v = _rigidbody.velocity;
            var velocityChange = (targetVelocity - v);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -MaxVelocityChange, MaxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -MaxVelocityChange, MaxVelocityChange);
            velocityChange.y = 0;

            _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        if (Input.GetButtonDown("Jump") && Grounded) {
            _rigidbody.AddForce(transform.up * JumpHeight);
        }

        Grounded = false;
    }

    void OnCollisionStay(Collision collision) {
        if (collision.transform.tag != "Not Ground") {
            Grounded = true;
        }
    }
}
