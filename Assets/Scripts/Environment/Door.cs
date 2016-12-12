using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float OpeningDuration;
    public float ClosingDuration;
    public float Angle;

    private Quaternion _initialRotation;
    private Quaternion _targetRotation;
    private Coroutine _coroutine;
    private Transform _doorRoot;

    private enum State {
        CLOSED,
        OPENING,
        OPENED,
        CLOSING
    };

    private State _state;
    private bool _canBeClosed;

    public bool Opened() {
        return _state != State.CLOSED;
    }

    // Use this for initialization
    void Start () {
        _doorRoot = transform.FindChild("DoorRoot");
        _initialRotation = _doorRoot.rotation;
	    _targetRotation = _initialRotation * Quaternion.AngleAxis(-Angle, Vector3.up);
        _state = State.CLOSED;
        _canBeClosed = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if (_canBeClosed && _state == State.OPENED) {
            _canBeClosed = false;
            _coroutine = StartCoroutine(Close());
	    }
	}

    void OnTriggerEnter(Collider other) {
        _canBeClosed = false;

        if (_state == State.CLOSED || _state == State.CLOSING) {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(Open());
        }
    }

    void OnTriggerExit(Collider other) {
        _canBeClosed = true;
    }

    IEnumerator Open() {
        _state = State.OPENING;

        var initialRotation = _doorRoot.rotation;
        for (float passed = 0f; passed <= OpeningDuration; passed += Time.deltaTime) {
            _doorRoot.rotation = Quaternion.Lerp(initialRotation, _targetRotation, passed / OpeningDuration);
            yield return null;
        }

        _state = State.OPENED;
    }

    IEnumerator Close() {
        _state = State.CLOSING;

        var targetRotation = _doorRoot.rotation;
        for (float passed = 0f; passed <= ClosingDuration; passed += Time.deltaTime) {
            _doorRoot.rotation = Quaternion.Lerp(targetRotation, _initialRotation, passed / ClosingDuration);
            yield return null;
        }

        _state = State.CLOSED;

        // TODO: destroy prev room
    }
}
