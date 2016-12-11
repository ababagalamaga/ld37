using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float jumpPressure = 0f;
    public float minJumpPower = 0.1f;
    public float maxJumpPower = 1f;
    public float pressureCoef = 1f;
    public bool onGround = false;

    private Rigidbody _rigidbody;

	// Use this for initialization
	void Start () {
	    _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        if (onGround){
            if (Input.GetButton("Jump")) {
                if (jumpPressure < maxJumpPower)
                {
                    jumpPressure += Time.deltaTime * pressureCoef;
                }
                else
                {
                    jumpPressure = maxJumpPower;
                }
            }
            else {
                if (jumpPressure > 0f) {
                    jumpPressure += minJumpPower;
                    _rigidbody.AddForce(0, jumpPressure / Time.deltaTime * _rigidbody.mass, 0);
                    jumpPressure = 0;
                    onGround = false;
                }
            }
           
        }
    }
    void OnCollisionEnter(Collision oter) {
        onGround = true;
        //if (oter.gameObject.CompareTag("ground")) {
        //    onGround = true;
        //}
    }
}
