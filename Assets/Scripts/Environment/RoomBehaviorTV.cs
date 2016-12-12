using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviorTV : MonoBehaviour
{

    public bool ObjectiveSucced = true;
    public float Period = 2.0f;

    private float _timeLeft = 0f;
    private bool _isWatching = true;
    private bool _isDetected = false;
    private Room _room;
    private GameObject _player = null;
    public GameObject _tv;
    public GameObject _tvLight;

    // Use this for initialization
    void Awake()
    {
        _room = GetComponent<Room>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft < 0 && !_isDetected) {
            _isWatching = !_isWatching;
            _timeLeft = Period;
            if (_isWatching) {
                _tv.GetComponent<Renderer>().materials[0].color = Color.white;
                _tv.GetComponent<Renderer>().materials[1].color = Color.white;
                _tv.GetComponent<Renderer>().materials[2].color = Color.white;
                _tvLight.GetComponent<Light>().enabled = _isWatching;
            } else {
                _tv.GetComponent<Renderer>().materials[0].color = Color.black;
                _tv.GetComponent<Renderer>().materials[1].color = Color.black;
                _tv.GetComponent<Renderer>().materials[2].color = Color.black;
                _tvLight.GetComponent<Light>().enabled = _isWatching;
            }
        }
        Vector3 raycast_direction = _player.transform.position - _tv.transform.position;
        if (_isDetected) {
            _tvLight.GetComponent<Transform>().rotation = Quaternion.LookRotation(raycast_direction);
        }
        if (_isWatching && !_isDetected) {
            RaycastHit hit;

            _tvLight.GetComponent<Transform>().rotation = Quaternion.LookRotation(raycast_direction);
            if (Physics.Raycast(_tv.transform.position, raycast_direction, out hit)) {
                if (hit.collider.gameObject.tag == "Player") {
                    _room.ObjectiveSucced = false;
                    _isDetected = true;
                }
            }
        }
    }
}
