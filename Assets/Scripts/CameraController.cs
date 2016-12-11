using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float LerpCoeff;
    public float Height;

    private GameObject _playerController;
    private Vector3 _offset;

	// Use this for initialization
	void Start () {
	    _playerController = GameObject.FindGameObjectWithTag("Player");
        _offset = new Vector3(0, Height, 0);
	}
	
	// Update is called once per frame
	void Update () {
	    transform.rotation = Quaternion.Lerp(_playerController.transform.rotation, transform.rotation,
	        Time.deltaTime * LerpCoeff);
	    transform.position = _playerController.transform.position + _offset;
	}
}
