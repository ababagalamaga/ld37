using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class Game : MonoBehaviour
{
    public List<GameObject> Rooms;

    private int _currentRoomId;
    private GameObject _current;
    private GameObject _nextCurrent;
    private GameObject _nextNext;

    void Awake() {
        _currentRoomId = 0;
    }

	void Start () {
        _current = Instantiate(Rooms[_currentRoomId]);
	    SpawnNext();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SpawnNext() {
        _nextCurrent = Instantiate(Rooms[_currentRoomId++]);
        _nextNext = Instantiate(Rooms[_currentRoomId]);

        MoveToEnd();
    }

    private void MoveToEnd() {
        var currentExitTransform = _current.transform.FindChild("ExitTransform") as Transform;
        var nextCurrentEnterTransform = _nextCurrent.transform.FindChild("EnterTransform") as Transform;
        var nextNextEnterTransform = _nextNext.transform.FindChild("EnterTransform") as Transform;

        var nextCurrentOffset = currentExitTransform.position - nextCurrentEnterTransform.localPosition;
        var nextNextOffset = currentExitTransform.position - nextNextEnterTransform.localPosition;

        _nextCurrent.transform.position = nextCurrentOffset;
        _nextNext.transform.position = nextNextOffset;
    }
}
