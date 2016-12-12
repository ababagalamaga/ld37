using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviorTV : MonoBehaviour
{

    public bool ObjectiveSucced = false;
    public float Period = 2.0f;

    private float _timeLeft = 0f;
    private bool _isWatching = false;
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
        if (ObjectiveSucced)
        {
            _room.ObjectiveSucced = true;
            Destroy(this);
        }
    }
}
