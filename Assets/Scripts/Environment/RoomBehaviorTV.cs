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
    private GameObject _tvLight;

    // Use this for initialization
    void Start()
    {
        print("started");
        _tv = GameObject.Find("TV");
        _tvLight = GameObject.Find("TVLight");
        _room = GetComponent<Room>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft < 0) {
            print("swap");
            _isWatching = !_isWatching;
            _timeLeft = Period;
            if (_isWatching) {
                _tv.GetComponent<Renderer>().materials[0].color = Color.white;
                _tv.GetComponent<Renderer>().materials[1].color = Color.white;
                _tv.GetComponent<Renderer>().materials[2].color = Color.white;
                _tvLight.GetComponent<Light>().enabled = _isWatching;
                //mat[0].color = Color.white;
                //mat[0].SetColor("Color", Color.white);
            } else {
                _tv.GetComponent<Renderer>().materials[0].color = Color.black;
                _tv.GetComponent<Renderer>().materials[1].color = Color.black;
                _tv.GetComponent<Renderer>().materials[2].color = Color.black;
                _tvLight.GetComponent<Light>().enabled = _isWatching;
                //mat[0].color = Color.black;
                //0[mat].SetColor("Color", Color.black);
            }
        }
        if (_isWatching) {
            Vector3 raycast_direction = _player.transform.position - _tv.transform.position;
            RaycastHit hit;

            _tvLight.GetComponent<Transform>().rotation = Quaternion.LookRotation(raycast_direction);
            if (Physics.Raycast(_tv.transform.position, raycast_direction, out hit)) {
                if (hit.collider.gameObject.tag == "Player") {
                    print("I SEE YOU");
                    ObjectiveSucced = false;
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (ObjectiveSucced) {
                print("destroy");
                _room.ObjectiveSucced = true;
                Destroy(this);
            }
        }
    }
}
