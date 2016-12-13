using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

    public List<GameObject> Rooms;

    private GameObject _playerController;
    private GameObject _cameraController;
    private GameObject _menu;

    private int _currentRoomId;
    private GameObject _previous;
    private GameObject _current;
    private GameObject _nextCurrent;
    private GameObject _nextNext;

    private bool _nextUnlocked;

    void Awake() {
        _currentRoomId = 0;
        _nextUnlocked = true;
    }

    void Start() {
        _playerController = GameObject.FindGameObjectWithTag("Player");
        _menu = GameObject.FindGameObjectWithTag("Menu");
        _menu.SetActive(false);
        _cameraController = GameObject.FindGameObjectWithTag("MainCamera");

        _current = Instantiate(Rooms[_currentRoomId]);
        _playerController.GetComponent<PlayerController>().ApplySettings(_current.GetComponent<Room>());
        _current.GetComponent<Room>().Initialize();
        SpawnNext();

        Cursor.lockState = CursorLockMode.Locked;
        _playerController.GetComponent<MouseLook>().RotationEnabled(true);
    }

    public void SetCamera(GameObject camera) {
        _cameraController = camera;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("escape")) {
            Cursor.lockState = CursorLockMode.None;
            _playerController.GetComponent<MouseLook>().RotationEnabled(false);
        }

        if (_current.GetComponent<Room>().ObjectiveSucced) {
            _nextUnlocked = true;
            _current.GetComponent<Room>().ObjectiveSucced = false;
        }

        var nextDoorObject = _current.transform.FindChild("Door");
        if (nextDoorObject != null) {
            var nextDoor = nextDoorObject.GetComponent<Door>();

            if (_nextUnlocked && !nextDoor.Opened()) {
                Destroy(_nextCurrent);
                _nextCurrent = null;
                _nextUnlocked = false;
                ++_currentRoomId;
            }

            if (nextDoor.Opened()) {
                if (_nextCurrent != null) {
                    if (!_nextCurrent.GetComponent<Room>().Initialized()) {
                        _nextCurrent.GetComponent<Room>().Initialize();
                        //Debug.Log(" _nextCurrent.GetComponent<Room>().Initialize();");
                    }
                } else {
                    if (!_nextNext.GetComponent<Room>().Initialized()) {
                        _nextNext.GetComponent<Room>().Initialize();
                        //Debug.Log(" _nextNext.GetComponent<Room>().Initialize()");
                    }
                }
            } else {
                if (_nextCurrent != null) {
                    if (_nextCurrent.GetComponent<Room>().Initialized()) {
                        if (_nextCurrent.GetComponent<Room>().PlayerInRoom()) {
                            MoveToNext();
                            nextDoor.transform.GetComponent<BoxCollider>().enabled = false;
                            if (_nextCurrent != null) {
                                if (!_nextCurrent.GetComponent<Room>().Initialized()) {
                                    _nextCurrent.GetComponent<Room>().Initialize();
                                    //Debug.Log(" _nextCurrent.GetComponent<Room>().Initialize();");
                                }
                            } else {
                                if (!_nextNext.GetComponent<Room>().Initialized()) {
                                    _nextNext.GetComponent<Room>().Initialize();
                                    //Debug.Log(" _nextNext.GetComponent<Room>().Initialize()");
                                }
                            }
                        } else {
                            _nextCurrent.GetComponent<Room>().DeInitialize();
                            //Debug.Log(" _nextCurrent.GetComponent<Room>().DeInitialize()");
                        }
                    }
                } else {
                    if (_nextNext.GetComponent<Room>().Initialized()) {
                        if (_nextNext.GetComponent<Room>().PlayerInRoom()) {
                            MoveToNext();
                            nextDoor.transform.GetComponent<BoxCollider>().enabled = false;
                            if (_nextCurrent != null) {
                                if (!_nextCurrent.GetComponent<Room>().Initialized()) {
                                    _nextCurrent.GetComponent<Room>().Initialize();
                                    //Debug.Log(" _nextCurrent.GetComponent<Room>().Initialize();");
                                }
                            } else {
                                if (!_nextNext.GetComponent<Room>().Initialized()) {
                                    _nextNext.GetComponent<Room>().Initialize();
                                    //Debug.Log(" _nextNext.GetComponent<Room>().Initialize()");
                                }
                            }
                        } else {
                            _nextNext.GetComponent<Room>().DeInitialize();
                            //Debug.Log(" _nextNext.GetComponent<Room>().DeInitialize()");
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                _menu.SetActive(!_menu.activeSelf);
                Time.timeScale = _menu.activeSelf?Time.timeScale = 0:Time.timeScale = 1;
            }
        }
    }

    public void Continue() {
        Time.timeScale = 1;
        _menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        _playerController.GetComponent<MouseLook>().RotationEnabled(true);
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        _playerController.GetComponent<MouseLook>().RotationEnabled(true);
    }

    public void Quit() {
        Application.Quit();
    }

    public void MoveToNext() {
        if (_previous != null) {
            Destroy(_previous);
        }

        _previous = _current;
        Destroy(_previous.GetComponent<RoomBehavior0>());
        Destroy(_previous.GetComponent<RoomBehavior1>());
        Destroy(_previous.GetComponent<RoomBehavior2>());
        Destroy(_previous.GetComponent<RoomBehavior6>());
        Destroy(_previous.GetComponent<RoomBehavior4>());
        Destroy(_previous.GetComponent<RoomBehaviorLava>());

        if (_nextCurrent == null) {
            _current = _nextNext;
            SpawnNext();
        } else {
            Destroy(_nextNext);
            _nextNext = null;
            _current = _nextCurrent;
            SpawnNext();
        }
        MoveBack();
    }

    private void MoveBack() {
        var currentEnterTransform = _current.transform.FindChild("EnterTransform") as Transform;
        var previousEnterTransform = _previous.transform.FindChild("EnterTransform") as Transform;

        var offset = currentEnterTransform.position - previousEnterTransform.position;

        if (_currentRoomId > 0)
            if (_cameraController.transform.parent == null) {
                _cameraController.transform.position -= offset;
            }
        _playerController.transform.position -= offset;
        _previous.transform.position -= offset;
        _current.transform.position -= offset;
        _nextCurrent.transform.position -= offset;
        _nextNext.transform.position -= offset;
    }

    private void SpawnNext() {
        _nextCurrent = Instantiate(Rooms[_currentRoomId]);
        _nextNext = Instantiate(Rooms[_currentRoomId + 1]);
        MoveToEnd();
    }

    private void MoveToEnd() {
        var currentExitTransform = _current.transform.FindChild("ExitTransform") as Transform;
        var nextCurrentEnterTransform = _nextCurrent.transform.FindChild("EnterTransform") as Transform;
        var nextNextEnterTransform = _nextNext.transform.FindChild("EnterTransform") as Transform;

        var nextCurrentPos = currentExitTransform.position - nextCurrentEnterTransform.localPosition;
        var nextNextPos = currentExitTransform.position - nextNextEnterTransform.localPosition;

        _nextCurrent.transform.position = nextCurrentPos;
        _nextNext.transform.position = nextNextPos;
    }
}
