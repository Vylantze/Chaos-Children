using UnityEngine;
using System.Collections;

public class Rapid : MonoBehaviour {
	GunScript gun;
	MasterPlayer master;

	// time between each individual shot
	public float shotWait = 0.25f;
	private float shotWaitTimer = 0f;
	public int angleInDegreesMax = 10; // in degrees
	private int currentAngle = 0;
	
	// Use this for initialization
	void Start () {
		gun = GetComponentInParent<GunScript> ();
		master = MasterPlayer.mainPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		// ATTACK
		// for rapid fire very easy
		if (Input.GetButton ("FireBullet")) {
			fire ();
		}

		if (!Input.GetButton ("FireBullet")) { //as long as fire button is not held
			currentAngle = 0;
		}
	}

	public void reset() {
		currentAngle = 0;
		shotWaitTimer = 0f;
	}
	
	void fire() {
		float time = Time.time; // current time
		if (time >= shotWaitTimer) { // if wait time expired
			shotWaitTimer = shotWait + time;
			Rigidbody2D bulletInstance = Instantiate (gun.bullet, gun.firingLocation.position, Quaternion.identity) as Rigidbody2D;
			bulletInstance.gameObject.SetActive (true);
			
			AudioSource.PlayClipAtPoint(gun.normalShot,transform.position);
			if (master.shipMode) {
				bulletInstance.rotation = 90; // 90 degrees means facing north
				bulletInstance.velocity = new Vector2 (0f, gun.bullet_speed);
			}
			else {
				// only platform got animation
				master.SetTrigger("shoot"); // change to shoot animation
				Vector3 scale = bulletInstance.transform.localScale;
				if (master.flip) {
					scale.x *= 1;
				} else {
					scale.x *= -1;
				}
				bulletInstance.transform.localScale = scale;

				// give some random angles for rapidfire				
				bulletInstance.rotation = Random.Range(-currentAngle, currentAngle)*1f;
				if (currentAngle<angleInDegreesMax) { currentAngle++; } // increment
				float angle = bulletInstance.rotation*Mathf.PI/180;
				bulletInstance.velocity = new Vector2 (
					gun.bullet_speed*Mathf.Cos(angle)*Mathf.Sign(scale.x),
					gun.bullet_speed*Mathf.Sin(angle));


				//bulletInstance.velocity = new Vector2 (Mathf.Sign(scale.x)*gun.bullet_speed, 0f);


				// if it is the girl, she gets double shots in the air
				if (master.in_air&&master.female) { 
					bulletInstance = Instantiate (gun.bullet, gun.firingLocation2.position, Quaternion.identity) as Rigidbody2D;
					bulletInstance.gameObject.SetActive (true);
					scale.x *= -1;
					bulletInstance.transform.localScale = scale;
					bulletInstance.velocity = new Vector2 (Mathf.Sign(scale.x)*gun.bullet_speed, 0f);
				}
			}
		}
	}
}
