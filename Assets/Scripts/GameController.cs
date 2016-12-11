using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<GameObject> Rooms;

    private int _currentRoomId;
    private GameObject _previous;
    private GameObject _current;
    private GameObject _nextCurrent;
    private GameObject _nextNext;

    private bool _nextUnlocked;
    private bool _nextDoorOpened;

    void Awake() {
        _currentRoomId = 0;
        _nextUnlocked = true;
    }

    void Start() {
        _current = Instantiate(Rooms[_currentRoomId]);
        SpawnNext(true);
    }

    // Update is called once per frame
    void Update() {
        if (_nextUnlocked && !_nextDoorOpened) {
            Destroy(_nextCurrent);
            _nextCurrent = null;
            _nextNext.GetComponent<Room>().Initialize();
            _nextUnlocked = false;
        }
    }

    public void SetDoorOpened(bool opened) {
        _nextDoorOpened = opened;
    }

    public void UnlockNext() {
        _nextUnlocked = true;
    }

    public void MoveToNext() {
        Debug.Log("MoveToNext");

        _previous = _current;
        if (_nextCurrent == null) {
            _current = _nextNext;
            SpawnNext(true);
        }
        else {
            Destroy(_nextNext);
            _nextNext = null;
            _current = _nextCurrent;
            SpawnNext(false);
        }
        MoveBack();
    }

    private void MoveBack() {
        Debug.Log("MoveBack");

        var currentEnterTransform = _current.transform.FindChild("EnterTransform") as Transform;
        var previousEnterTransform = _previous.transform.FindChild("EnterTransform") as Transform;

        var offset = currentEnterTransform.position - previousEnterTransform.position;

        _previous.transform.position -= offset;
        _current.transform.position -= offset;
        _nextCurrent.transform.position -= offset;
        _nextNext.transform.position -= offset;
    }

    private void SpawnNext(bool increment) {
        Debug.Log("SpawnNext");
        _nextCurrent = Instantiate(Rooms[_currentRoomId]);
        _nextNext = Instantiate(Rooms[_currentRoomId + 1]);

        if (increment)
            ++_currentRoomId;

        _nextCurrent.GetComponent<Room>().Initialize();

        MoveToEnd();
    }

    private void MoveToEnd() {
        Debug.Log("MoveToEnd");

        var currentExitTransform = _current.transform.FindChild("ExitTransform") as Transform;
        var nextCurrentEnterTransform = _nextCurrent.transform.FindChild("EnterTransform") as Transform;
        var nextNextEnterTransform = _nextNext.transform.FindChild("EnterTransform") as Transform;

        var nextCurrentPos = currentExitTransform.position - nextCurrentEnterTransform.localPosition;
        var nextNextPos = currentExitTransform.position - nextNextEnterTransform.localPosition;

        _nextCurrent.transform.position = nextCurrentPos;
        _nextNext.transform.position = nextNextPos;
    }
}
