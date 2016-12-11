using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float Sensitivity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime, 0), Space.World);
        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime, 0, 0));
    }
}
