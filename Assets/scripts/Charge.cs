﻿using UnityEngine;
using System.Collections;

public class Charge : MonoBehaviour {
	GunScript gun;
	MasterPlayer master;
	// bullet stuff
	bool firingDone = true;
	public int numShots = 0;
	public int totalShotsAllowed = 2;
	
	// Interval timer (between every totalShots
	public float waitTime = 1f;
	private float timer = 0f;
	// time between each individual shot
	public float shotWait = 0.15f;
	private float shotWaitTimer = 0f;
	
	// charge mode
	BulletScript script; // script of the charged shot
	Rigidbody2D chargedShot;
	CircleCollider2D chargedCollider;
	private int chargeLevel = 0;
	public int totalChargeLevels = 3; // maximum 3 charges
	public float chargeTime = 1f; 	// charging times before max - *3 of charge time to get max charge
	private float chargeTimer = 0f;

	public Animator ship_charge;
	public Animator platformer_charge;
	private Animator charge_anim;

	private bool reset_mode = false;
	
	// Use this for initialization
	void Start () {
		gun = GetComponentInParent<GunScript> ();
		master = MasterPlayer.mainPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		if (!reset_mode) {
			float time = Time.time; // current time
			// ATTACK
			if (firingDone && Input.GetButtonDown ("FireBullet")) { 
				// if charged mode and valid for firing
				firingDone = false;
				fire ();
				chargeTimer = time + chargeTime; // start a charge timer
			}
			// UPDATES REGARDING CHARGED SHOTS
			// if charge mode and holding fire button
			if (Input.GetButton ("FireBullet")) {
				if (time >= chargeTimer && chargeLevel <= totalChargeLevels) { // if charged for a certain time
					charge (); // activate charge and increase charge level
					chargeTimer = time + chargeTime; // reset timer
				}

				// if charge level already exists, make sure that the attack keeps following character
				if (chargeLevel > 0&&chargedShot!=null) {
					charge_anim.SetInteger ("charge_level", chargeLevel);
					Vector3 scale = chargedShot.transform.localScale;
					if (master.shipMode) {
						chargedShot.transform.position = master.shipPosition ();
					} else {
						chargedShot.transform.position = master.platformPosition ();
						if (master.flip) { // if facing front yet looking behind
							scale.x = Mathf.Abs (scale.x) * 1;
						} else {
							scale.x = Mathf.Abs (scale.x) * -1;
						}
					}
					chargedShot.transform.localScale = scale;
				}
			}// if charged shot is fired/released
			else if (Input.GetButtonUp ("FireBullet") && chargedShot != null) { // and chargedShot exists
				fireChargedShot ();
			} else if (Input.GetButtonUp ("FireBullet")) {
				if (firingDone) {
					fire ();
					firingDone = false;
				}
			} else if (chargedShot != null) { // else if the button not held, fire it away first chance possible
				fireChargedShot ();
			} else { // else basically button not held down
				charge_anim.SetBool ("charge", false);
				charge_anim.SetInteger ("charge_level", 0); // reset
				master.setCharge (0f);
				chargeLevel = 0;
			}

			/*
			//  LIMIT NUMBER OF SHOTS FIRED
			if (numShots >= totalShotsAllowed) {
				firingDone = false;
				numShots = 0;
			}//*/

			if (numShots==1) {
				fire();
			}
		
			if (!firingDone) {
				if (time >= timer) { // if waiting time has expired
					firingDone = true;
					numShots = 0;
				}
			}
		}
	}

	int fireChargedShot() {
		float time = Time.time; // current time
		if (chargedShot != null) {
			chargedCollider.enabled = true;
		
			if (chargeLevel > 1) {
				script.charged = true;
				AudioSource.PlayClipAtPoint (gun.chargedShot, transform.position);
			} else {
				// else don't fire anything
				Destroy(chargedShot.gameObject);
				if (firingDone) {
					fire ();
					firingDone = false;
				}
				return -1;
			}
		
			if (master.shipMode) { // if it is ship mode, just fire upwards
				chargedShot.velocity = new Vector2 (0f, gun.bullet_speed);
			} else {
				master.SetTrigger ("shoot");
				master.setCharge (0f); // reset 'charging' layer weight to 0
				chargedShot.velocity = new Vector2 (Mathf.Sign (chargedShot.transform.localScale.x) * gun.bullet_speed, 0f);
			}
			chargedShot.GetComponent<SpriteRenderer> ().enabled = true;
		}
		charge_anim.SetBool("charge", false);
		charge_anim.SetInteger("charge_level", 0); // reset
		chargeLevel = 0;
		chargedShot = null;
		shotWaitTimer = shotWait + time; // reset timer for normal shots also
		return 0;
	}
	
	void charge() {
		numShots = 0;
		if (chargedShot==null) {
			charge_anim.SetBool("charge", true);
			chargedShot = Instantiate (gun.bullet, gun.firingLocation.position, Quaternion.identity) as Rigidbody2D;
			chargedShot.gameObject.SetActive(true);
			chargedCollider = chargedShot.GetComponent<CircleCollider2D>();
			chargedCollider.enabled = false;// disable the collider first
			script = chargedShot.GetComponent<BulletScript>(); // assign the script
			if (master.shipMode){ 
				chargedShot.rotation = 90; // 270 degrees means facing north
			}
			else {
				master.setCharge(1f); // set 'charging' layer to activate
			}
			chargedShot.GetComponent<SpriteRenderer>().enabled = false;
		} else { //if (chargeLevel<=totalCharge) 
			Vector3 size = chargedShot.transform.localScale;
			chargedShot.transform.localScale = new Vector3(size.x*1.5f, size.y*1.5f, 1f);
			script.damage = script.damage*2;
		}
		// make the charged shot stronger
		chargeLevel++;
	}
	
	void fire() {
		float time = Time.time; // current time

		if (numShots==1) { 
			timer = time + waitTime; // start wait time upon first shot
		}

		if (time >= shotWaitTimer) { // if wait time expired for individual shots
			numShots++; // increment the number of shots, since it IS fired
			shotWaitTimer = shotWait + time; // reset timer
			// create an instance of the bullet
			Rigidbody2D bulletInstance = Instantiate (gun.bullet, gun.firingLocation.position, Quaternion.identity) as Rigidbody2D;
			bulletInstance.gameObject.SetActive (true);
			// play fire effect
			AudioSource.PlayClipAtPoint(gun.normalShot,transform.position);

			if (master.shipMode) { // if ship mode, shoot upwards
				bulletInstance.rotation = 90; // 90 degrees means facing north
				bulletInstance.velocity = new Vector2 (0f, gun.bullet_speed);
			}
			else {
				master.SetTrigger("shoot"); // change to shoot animation
				// else if platformer mode
				Vector3 scale = bulletInstance.transform.localScale;
				if (master.flip) {
					scale.x *= 1;
				} else {
					scale.x *= -1;
				}
				bulletInstance.velocity = new Vector2 (Mathf.Sign(scale.x)*gun.bullet_speed, 0f);
				bulletInstance.transform.localScale = scale; 
			}
		}
	}

	public void reset() {
		reset_mode = true;
		if (chargedShot != null) {
			Destroy(chargedShot.gameObject);
			chargedShot = null;
		}
		chargeLevel = 0;
		chargeTimer = 0f;
		firingDone = true;
		// reset charge animation
		if (MasterPlayer.mainPlayer.shipMode) {
			charge_anim = ship_charge;
		} else {
			charge_anim = platformer_charge;
		}
		charge_anim.GetComponent<SpriteRenderer> ().enabled = true;
		charge_anim.SetBool ("charge", false);
		charge_anim.SetInteger ("charge_level", 0); // reset
		reset_mode = false;
	}
}