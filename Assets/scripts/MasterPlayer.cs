using UnityEngine;
using System.Collections;

public class MasterPlayer : PlayerController {
	// singleton design pattern
	public static MasterPlayer mainPlayer = null;

	public PlatformController platformer;
	public ShipController ship;
	public GunScript gun;
	public bool flip = true; // true = faceright, false = faceleft
	public bool in_air = false;
	bool saved_before = false; // at the start of the game, shouldn't have saved before

	// health
	public bool dead = false;

	//sound
	public AudioClip switch_mode;

	// Use this for initialization
	void Awake() {
		if (mainPlayer == null) {
			DontDestroyOnLoad (gameObject);
			mainPlayer = this;
		} else if (mainPlayer!=this) {
			Destroy (gameObject);
		}
		gun = GetComponentInChildren<GunScript> ();
		ship = GetComponentInChildren<ShipController> ();
		platformer = GetComponentInChildren<PlatformController> ();
	}

	void Start () {
		saved_before = false;
		loadedFromFile = false;
		reset ();
	}

	void loadAnimator() {
		// force the renders to eitehr be in existence or not
		platformer.female_chara.GetComponent<SpriteRenderer>().enabled = female;
		platformer.male_chara.GetComponent<SpriteRenderer>().enabled= !female;
		if (female) {
			anim = platformer.female_chara.GetComponent<Animator>(); // load female animator
		} else { // else if male
			anim = platformer.male_chara.GetComponent<Animator>();; // load male animator
			setCharge (0f); // set 'charging' layer to 0
		}
		platformer.setAnimator(anim);
	}

	void loadShip() {
		ship.female_chara.GetComponent<SpriteRenderer> ().enabled = female;
		ship.male_chara.GetComponent<SpriteRenderer> ().enabled = !female;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Switch Mode")&&STATUS.debug) {
			female = !female;
			// true is female
			// false is male
			if (!shipMode) {
				loadAnimator ();
			}
			else {
				loadShip ();
			}
		}

		if (Input.GetButtonDown ("ColourNext")) {
			colourChange(true);
		}

		if (Input.GetButtonDown ("ColourPrevious")) {
			colourChange (false);
		}

		if (!dead) {
			//colourChange ();
		}
		/*
		if (Input.GetButtonDown("SwitchScene")&&STATUS.debug) {
			shipMode = !shipMode;
			ship.gameObject.SetActive(shipMode);
			platformer.gameObject.SetActive(!shipMode);
		}*/
	}
	/* // old version
	void colourChange() {
		for (int i=1; i<4; i++) { // start from 1, aka fire
			if (Input.GetButtonDown (commands[i]) && // if correct command
			    elements [i]) { // and element is now assessible
				AudioSource.PlayClipAtPoint(switch_mode, transform.position);
				GetComponent<ModeChange> ().currentColour = i;
				break; // once colour changed, can break loop
			}
		}
	}//*/

	void colourChange(bool increase) {
		ModeChange mode = GetComponent<ModeChange> ();
		int nextColour = mode.currentColour;
		for (int i=0; i<4; i++) {
			if (increase) {
				nextColour++;
				if (nextColour>=4) {
					nextColour=1;
				}
			}else {
				nextColour--;
				if (nextColour<=0) {
					nextColour=3;
				}
			}

			if (elements [nextColour]) { // if valid
				AudioSource.PlayClipAtPoint(switch_mode, transform.position);
				mode.currentColour = nextColour;
				break;
			}
		}
	}

	public void Restart() {
		reset();
		Application.LoadLevel (Application.loadedLevel);
		PlatformCamera camera = PlatformCamera.mainCamera;
		if (camera!=null) {
			camera.lockCamera = true;
			camera.enabled = false;
		}
		if (saved_before) {
			loadFromFile ();
			mainPlayer.loadPosition (); 
		}
		reset ();
		if (camera!=null) {
			camera.transform.position = transform.position;
			camera.lockCamera = false;
			camera.enabled = true;
		}
	}

	public void SetTrigger(string value) {
		anim.SetTrigger (value);
	}

	public void reset() {
		dead = false;
		if (!shipMode) {
			platformer.enabled = true;
			loadAnimator();
			gun.reset ();
			platformer.reset();
			ship.disable ();
		} else {
			ship.enabled = true; 
			ship.reset();
			loadShip ();
			gun.reset ();
			platformer.disable ();
		}
	}

	public void enableAll(bool value) {
		platformer.enabled = value;
		ship.enabled = value;
		gun.enableGuns (value);
		gun.enabled = value;
	}

	public void enableGuns(bool value) {
		gun.enableGuns (value);
		gun.enabled = value;
	}

	public void loadPosition() {
		transform.position = loadedPosition;
	}

	public void Death(){
		if (!dead) {
			dead = true;
			PlatformCamera camera = PlatformCamera.mainCamera;
			if (camera != null) {
				camera.lockCamera = true;
				anim.SetBool ("dead", true);
			}
			Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D> ();
			foreach (Collider2D collider in colliders) {
				collider.isTrigger = true;
			}
			Rigidbody2D rb2d;
			if (!shipMode) {
				rb2d = platformer.GetComponent<Rigidbody2D>();
				platformer.enabled = false;
			} else {
				rb2d = ship.GetComponent<Rigidbody2D>();
				ship.enabled = false;
			}
			rb2d.velocity = Vector2.zero;
			rb2d.AddForce (new Vector2 (0f, 500));
			rb2d.gravityScale = 1f;
		}
	}
	public void triggerCheck(Collider2D collider) {
		if (collider.CompareTag ("EnemyBullet")) {
			Death ();
		}
	}

	public Vector3 shipPosition() {
		return ship.gameObject.transform.position;
	}

	public Vector3 platformPosition() {
		return platformer.gameObject.transform.position;
	}

	public void setCharge(float value) {
		if (!female&&!shipMode) { // only if male chara and platformer mode
			anim.SetLayerWeight (1, value);
		}
	}

	public void save() {
		save (transform.position);
	}

	public void save(Vector3 position) {
		saved_before = true;
		saveToFile ("autoSave", position.x, position.y);
	}

	public void cutSceneMode(bool value) {
		if (value) {// if true, reduce velocity to zero 
			Rigidbody2D rb2d;
			if (!shipMode) {
				rb2d = platformer.GetComponent<Rigidbody2D> ();
			} else {
				rb2d = ship.GetComponent<Rigidbody2D> ();
			}
			rb2d.velocity = Vector2.zero;
		}
		enableAll (!value); // opposite of true, if mode is activated
	}
}
