using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float Duration;
    public float Angle;

    private Collider _trigger;

	// Use this for initialization
	void Start () {
	    _trigger = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other) {
        StartCoroutine(Open());
    }

    IEnumerator Open() {
        var initialRot = transform.rotation.eulerAngles;

        for (float passed = 0f; passed <= Duration; passed += Time.deltaTime) {
            var targetRot = initialRot;
            targetRot.y -= passed / Duration * Angle;
            transform.rotation = Quaternion.Euler(targetRot);
            yield return null;
        }

        Destroy(_trigger);
        _trigger = null;
    }
}
