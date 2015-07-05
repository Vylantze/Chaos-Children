using UnityEngine;
using System.Collections;

public class Charge : MonoBehaviour {
	GunScript gun;
	MasterPlayer master;
	// bullet stuff
	bool firingDone = true;
	private int numShots = 0;
	public int totalShotsAllowed = 3;
	
	// Interval timer (between every totalShots
	public float waitTime = 3f;
	private float timer = 0f;
	// time between each individual shot
	public float shotWait = 0.4f;
	private float shotWaitTimer = 0f;
	
	// charge mode
	BulletScript script; // script of the charged shot
	Rigidbody2D chargedShot;
	CircleCollider2D chargedCollider;
	private int chargeLevel = 0;
	public int totalChargeLevels = 3; // maximum 3 charges
	public float chargeTime = 1f; 	// charging times before max - *3 of charge time to get max charge
	private float chargeTimer = 0f;
	public Animator charge_anim;
	
	// Use this for initialization
	void Start () {
		gun = GetComponentInParent<GunScript> ();
		master = gun.master;
	}
	
	// Update is called once per frame
	void Update () {
		float time = Time.time; // current time
		// ATTACK
		if (firingDone && Input.GetButtonDown ("FireBullet")) { 
			// if charged mode and valid for firing
			chargeTimer = time + chargeTime; // start a charge timer
		}


		
		// UPDATES REGARDING CHARGED SHOTS
		// if charge mode and holding fire button
		if (Input.GetButton ("FireBullet")) { 
			if (time>=chargeTimer&&chargeLevel<=totalChargeLevels) { // if charged for a certain time
				charge(); // activate charge and increase charge level
				chargeTimer = time + chargeTime; // reset timer
			}

			// if charge level already exists, make sure that the attack keeps following character
			if (chargeLevel>0) {
				charge_anim.SetInteger("charge_level", chargeLevel);
				Vector3 scale = chargedShot.transform.localScale;
				if (master.shipMode) {
					chargedShot.transform.position = master.shipPosition();
				}
				else {
					chargedShot.transform.position = master.platformPosition();
					if (master.flip) { // if facing front yet looking behind
						scale.x = Mathf.Abs(scale.x)*1;
					} else {
						scale.x = Mathf.Abs(scale.x)*-1;
					}
				}
				chargedShot.transform.localScale = scale;
			}
		}

		// if charged shot is fired/released
		if (Input.GetButtonUp ("FireBullet") && chargedShot != null) { // and chargedShot exists
			chargedCollider.enabled = true;

			if (chargeLevel>1) {
				script.charged = true;
				AudioSource.PlayClipAtPoint(gun.chargedShot,transform.position);
			}
			else {AudioSource.PlayClipAtPoint(gun.normalShot,transform.position); }

			chargedShot.GetComponent<SpriteRenderer>().enabled = true;
			if (master.shipMode) { // if it is ship mode, just fire upwards
				chargedShot.velocity = new Vector2 (0f, gun.bullet_speed);
			} else {
				master.SetTrigger ("shoot");
				charge_anim.SetBool("charge", false);
				charge_anim.SetInteger("charge_level", 0); // reset
				master.setCharge(0f); // reset 'charging' layer weight to 0
				chargedShot.velocity = new Vector2 (Mathf.Sign (chargedShot.transform.localScale.x) * gun.bullet_speed, 0f);
			}
			chargeLevel = 0;
			chargedShot = null;
			shotWaitTimer = shotWait + time; // reset timer for normal shots also
		} else if (Input.GetButtonUp ("FireBullet")) {
			fire ();
		}


		//  LIMIT NUMBER OF SHOTS FIRED
		if (numShots >= totalShotsAllowed) {
			firingDone = false;
			numShots = 0;
		}
		
		if (!firingDone) {
			if (time>=timer) { // if waiting time has expired
				firingDone = true;
			}
		}

	}
	
	void charge() {
		numShots = 0;
		if (chargedShot==null) {
			if (!master.shipMode) {
				master.setCharge(1f); // set 'charging' layer to activate
				charge_anim.SetBool("charge", true);
			}
			chargedShot = Instantiate (gun.bullet, gun.firingLocation.position, Quaternion.identity) as Rigidbody2D;
			chargedShot.gameObject.SetActive(true);
			chargedCollider = chargedShot.GetComponent<CircleCollider2D>();
			chargedCollider.enabled = false;// disable the collider first
			chargedShot.GetComponent<SpriteRenderer>().enabled = false;
			script = chargedShot.GetComponent<BulletScript>(); // assign the script
			if (master.shipMode){ 
				chargedShot.rotation = 90; // 270 degrees means facing north
			}
		} else { //if (chargeLevel<=totalCharge) 
			Vector3 size = chargedShot.transform.localScale;
			chargedShot.transform.localScale = new Vector3(size.x*1.5f, size.y*1.5f, 1f);
			script.damage = script.damage*2;
		}
		// make the charged shot stronger
		chargeLevel++;
	}
	
	void fire() {
		numShots++; // increment the number of shots, since it IS fired
		float time = Time.time; // current time

		if (numShots==1) { 
			timer = time + waitTime; // start wait time upon first shot
		}

		if (time >= shotWaitTimer) { // if wait time expired for individual shots
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
}
