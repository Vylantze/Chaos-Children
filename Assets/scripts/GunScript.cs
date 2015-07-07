using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {
	public static GunScript gun;
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

	// sound
	public AudioClip chara_change;
	public AudioClip normalShot;
	public AudioClip chargedShot;

	private MasterPlayer master;
	
	// rapid fire mode
	public bool rf_c; // rapid fire or charge mode
	// true is rapid fire
	// false is charge

	// Use this for initialization
	void Start () {
		master = MasterPlayer.mainPlayer;
		rapid = GetComponentInChildren<Rapid> ();
		charge = GetComponentInChildren<Charge> ();
		reset ();
	}

	void activateGun() {
		rapid.enabled = rf_c;
		charge.enabled = !rf_c;
	}

	public void enableGuns(bool value) {
		rapid.enabled = charge.enabled = value;
	}

	public void reset(){
		enableGuns(true);
		charge.reset ();
		rapid.reset ();
		enableGuns(false);
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
	
	// Update is called once per frame
	void Update () {
		if (!master.dead && !SceneManager.manager.isPaused) {
			// switch mdes // debug only
			if (Input.GetButtonDown ("Switch Mode") && STATUS.debug) {
				AudioSource.PlayClipAtPoint (chara_change, transform.position);
				rf_c = !rf_c;
				// true is rapid fire
				// false is charge
			}
			if (Input.GetButtonDown ("SwitchScene") && STATUS.debug) {
				//rapid = GetComponentInChildren<Rapid> ();
				//charge = GetComponentInChildren<Charge> ();
				if (master.shipMode) {
					firingLocation = shipLocation;
				} else {
					firingLocation = platformLocation;
				}
			}//*/

			activateGun ();
		} else if (SceneManager.manager.isPaused) {
			rapid.enabled = false;
			charge.enabled = false;
		}
	}
}
