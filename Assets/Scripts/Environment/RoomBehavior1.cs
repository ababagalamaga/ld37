using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior1 : MonoBehaviour {

    public float Stamina;
    public float StaminaDecSpeed;
    public float StaminaIncSpeed;
    public bool ObjectiveSucced = false;

    private Room _room;
    private GameObject _player;
    private float _stamina;

    // Use this for initialization
    void Start () {
	    _room = GetComponent<Room>();
	    _player = GameObject.FindGameObjectWithTag("Player");
        _stamina = Stamina;
    }
	
	// Update is called once per frame
	void Update () {
	    if (_room.PlayerInRoom()) {
            var playerController = _player.GetComponent<PlayerController>();
            var playerMovement = _player.GetComponent<PlayerMovement>();

	        if (playerMovement.Moving()) {
                _stamina -= StaminaDecSpeed * Time.fixedDeltaTime;
	            if (_stamina < 0.0f) {
	                _stamina = 0.0f;
	            }
	        } else {
	            _stamina += StaminaIncSpeed * Time.fixedDeltaTime;
	            if (_stamina > Stamina) {
	                _stamina = Stamina;
	            }
	        }

	        playerMovement.SpeedMultiplier = _stamina / Stamina;
            playerController.ApplyVignetteSettings(true, Time.deltaTime, (1.0f - (_stamina / Stamina)) * 0.5f);
            playerController.ApplyTonemappingSettings(true, Time.deltaTime, Mathf.Pow(_stamina / Stamina, 0.75f) * 0.26f);

            /*if (ObjectiveSucced) {
                _room.ObjectiveSucced = true;
                Destroy(this);
            }*/
        }
	}
}
