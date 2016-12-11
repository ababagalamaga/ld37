using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float LookLerpCoeff;
    public float MoveLerpCoeff;
    public float Height;

    private GameObject _playerController;
    private Vector3 _offset;

	// Use this for initialization
	void Start () {
	    _playerController = GameObject.FindGameObjectWithTag("Player");
        _offset = new Vector3(0, Height, 0);
	}
	
	// Update is called once per frame
	void LateUpdate () {
	    transform.rotation = Quaternion.Lerp(_playerController.transform.rotation, transform.rotation,
	        Time.deltaTime * LookLerpCoeff);
	    transform.position = Vector3.Lerp(_playerController.transform.position + _offset, transform.position, Time.deltaTime * MoveLerpCoeff);
	}
}
