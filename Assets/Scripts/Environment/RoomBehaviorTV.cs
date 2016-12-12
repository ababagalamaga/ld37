using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviorTV : MonoBehaviour
{

    public bool ObjectiveSucced = true;
    public float Period = 2.0f;

    private float _timeLeft = 0f;
    private bool _isWatching = true;
    private Room _room;
    private GameObject _player;
    private GameObject _tv;

    // Use this for initialization
    void Start()
    {
        _tv = GameObject.Find("TV");
        _room = GetComponent<Room>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
         _timeLeft -= Time.deltaTime;

        if (_timeLeft < 0) {
            _isWatching = !_isWatching;
            Material mat = _tv.GetComponent<Renderer>().materials[1];
            if (_isWatching) {
                mat.SetColor("Albedo", Color.white);
            } else {
                mat.SetColor("Albedo", Color.black);
            }
            _timeLeft = Period;
        }
        if (_isWatching) {
            Vector3 raycast_direction = _player.transform.position - _tv.transform.position;
            RaycastHit hit;

            if (Physics.Raycast(_tv.transform.position, raycast_direction, out hit)) {
                if (hit.collider.gameObject.tag == "Player") {
                    print("I SEE YOU");
                    ObjectiveSucced = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (ObjectiveSucced) {
                _room.ObjectiveSucced = true;
                Destroy(this);
            }
        }
    }
}
