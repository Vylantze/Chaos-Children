﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float HP = 1f;
	private Rigidbody2D rb2d;
	public bool dead = false;
	public float deathForceMax = 250f;
	public float deathForceMin = 50f;
	public bool master = false; // is it a master enemy
	public bool onlyChargeShots = false;
	public float damage_dealt = 1f;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		if (master) {
			Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D>();
			foreach (Collider2D collider in colliders) {
				collider.enabled = false;
			}
			colliders = transform.GetComponents<Collider2D>();
			foreach (Collider2D collider in colliders) {
				collider.enabled = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (rb2d.position == Vector2.zero) {
			Destroy(transform.gameObject);
		}

		if(dead) {
			dead = false; // only need to do this once
			transform.tag = "ToBeDestroyed";
			if (master) {
				Scatter();
			}
			else {
				Death();
			}
		}
	}

	public Rigidbody2D rigid() {
		return rb2d;
	}

	void Scatter() {
		Enemy[] enemySections = GetComponentsInChildren<Enemy> (); // get all sub components
		foreach (Enemy sub in enemySections) {
			sub.dead = true;
		}

		Collider2D[] colliders = GetComponentsInChildren<Collider2D> ();
		foreach (Collider2D collider in colliders) {
			collider.enabled = true;
			collider.isTrigger = true;
		}

		colliders = GetComponents<Collider2D> (); // destroy all current colliders
		foreach (Collider2D collider in colliders) {
			Destroy(collider);
		}
	}

	void Death() {
		Collider2D[] colliders = GetComponents<Collider2D>();
		foreach (Collider2D collider in colliders) {
			collider.isTrigger = true;
		}

		// animation section
		rb2d.isKinematic = false; // now can be subject to gravity
		rb2d.constraints = RigidbodyConstraints2D.None;
		// now add torque, but spin away from player
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		float direction = 1f; // by default, fly towards right
		if (player.transform.position.x > rb2d.position.x) { 
			// if player is on right side of object
			direction = -1f; // fly left instead
		}
		Random.seed = (int)Time.time;
		float dir_factor = Random.Range (0f, 1f);
		float force = Random.Range (deathForceMin, deathForceMax);
		rb2d.AddTorque (force * dir_factor); 
		rb2d.AddForce (new Vector2(force * direction, force * dir_factor)); 
		// fly towards direction
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Bullet")&&transform.tag!="ToBeDestroyed") {
			BulletScript bullet = collider.GetComponent<BulletScript>();
			if (!bullet.charged) { // if it is not a charged bullet
				AudioSource.PlayClipAtPoint(bullet.normalShot, transform.position);
				Destroy(bullet.gameObject);
			}
			else {
				AudioSource.PlayClipAtPoint(bullet.chargedShot, transform.position);
			}

			if (!onlyChargeShots|| // means if normal shots can deal damage also
			    (bullet.charged&&onlyChargeShots)) {
				HP -= bullet.damage;
			}
		}

		if (HP <= 0f) {
			dead = true;
		}
	}


}