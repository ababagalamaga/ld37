using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float LerpCoeff;

    private GameObject _playerController;

	// Use this for initialization
	void Start () {
	    _playerController = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	    transform.rotation = Quaternion.Lerp(_playerController.transform.rotation, transform.rotation,
	        Time.deltaTime * LerpCoeff);
	    transform.position = _playerController.transform.position;
	}
}
