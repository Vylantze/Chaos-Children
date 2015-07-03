﻿using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {
	// bullet stuff
	public Rigidbody2D bullet;
	public Transform firingLocation;
	public float bullet_speed = 30f;
	private float platformer_speed = 30f;
	private float ship_speed = 15f;

	public Transform firingLocation2;
	public Transform shipLocation;
	public Transform platformLocation;
	private Rapid rapid;
	private Charge charge;

	public AudioClip mode_change;
	public MasterPlayer master;
	
	// rapid fire mode
	public bool rf_c; // rapid fire or charge mode
	// true is rapid fire
	// false is charge

	// Use this for initialization
	void Start () {
		rapid = GetComponentInChildren<Rapid> ();
		charge = GetComponentInChildren<Charge> ();
		if (master.shipMode) {
			firingLocation = shipLocation;
			bullet_speed = ship_speed;
		}
		else {
			firingLocation = platformLocation;
			bullet_speed = platformer_speed;
		}

		rf_c = master.female;
		// true is rapid fire/female
		// false is charged mode/male
		activateGun ();
	}

	void activateGun() {
		rapid.enabled = rf_c;
		charge.enabled = !rf_c;
	}
	
	// Update is called once per frame
	void Update () {
		// switch mdes // debug only
		if (Input.GetButtonDown("Switch Mode")&&master.debug.debug) {
			AudioSource.PlayClipAtPoint(mode_change, transform.position);
			rf_c = !rf_c;
			// true is rapid fire
			// false is charge
			activateGun();
		}
		if (Input.GetButtonDown ("SwitchScene") && master.debug.debug) {
			//rapid = GetComponentInChildren<Rapid> ();
			//charge = GetComponentInChildren<Charge> ();
			if (master.shipMode) {
				firingLocation = shipLocation;
			}
			else {
				firingLocation = platformLocation;
			}
		}//*/
	}
}