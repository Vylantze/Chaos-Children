using UnityEngine;
using System.Collections;

public class MasterPlayer : PlayerController {
	// singleton design pattern
	public static MasterPlayer mainPlayer = null;

	public PlatformController platformer;
	public ShipController ship;
	public bool flip = true; // true = faceright, false = faceleft
	public bool in_air = false;

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
	}

	void Start () {
		ship = GetComponentInChildren<ShipController> ();
		platformer = GetComponentInChildren<PlatformController> ();
		switchMode (shipMode);
	}

	void switchMode(bool isShipOn) {
		triggerPlatform(!isShipOn);
		triggerShip (isShipOn);
	}

	void loadAnimator() {
		// force the renders to eitehr be in existence or not
		platformer.female_chara.GetComponent<SpriteRenderer>().enabled = female;
		platformer.male_chara.GetComponent<SpriteRenderer>().enabled= !female;
		if (female) {
			anim = platformer.female_chara.GetComponent<Animator>(); // load female animator
		} else { // else if male
			anim = platformer.male_chara.GetComponent<Animator>();; // load male animator
		}
		platformer.setAnimator(anim);
	}

	
	void triggerPlatform(bool activated) {
		//platformer.colliderEnabled(activated); // enable/disable collider
		loadAnimator ();
		platformer.enabled = activated; // disable the controller
	}

	void triggerShip(bool activated) {
		ship.colliderEnabled(activated); // enable/disable collider
		ship.GetComponentInChildren<SpriteRenderer>().enabled = activated;
		ship.enabled = activated;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Switch Mode")&&Debug.debug) {
			female = !female;
			// true is female
			// false is male
			loadAnimator ();
		}
		if (!dead) {
			colourChange ();
		}
		/*
		if (Input.GetButtonDown("SwitchScene")&&debug.debug) {
			shipMode = !shipMode;
			ship.gameObject.SetActive(shipMode);
			platformer.gameObject.SetActive(!shipMode);
		}*/
	}

	void colourChange() {
		for (int i=1; i<4; i++) { // start from 1, aka fire
			if (Input.GetButtonDown (commands[i]) && // if correct command
			    elements [i]) { // and element is now assessible
				AudioSource.PlayClipAtPoint(switch_mode, transform.position);
				GetComponent<ModeChange> ().currentColour = i;
				break; // once colour changed, can break loop
			}
		}
	}

	public void Restart() {
		Reanimate ();
		loadFromFile ();
		PlatformCamera camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PlatformCamera> ();
		camera.lockCamera = false;
		//camera.transform.position = transform.position;
		if (shipMode) {
			ship.gameObject.transform.localPosition = Vector3.zero;
		} else {
			platformer.gameObject.transform.localPosition = Vector3.zero;
		}
	}

	public void SetTrigger(string value) {
		anim.SetTrigger (value);
	}

	void Reanimate() {
		dead = false;
		Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D> ();
		foreach (Collider2D collider in colliders) {
			collider.isTrigger = false;
		}
		if (!shipMode) {
			platformer.enabled = true;
			anim.SetBool ("dead", false);
			platformer.reset();
		} else {
			ship.enabled = true; 
			// and reset the gravity for rb2d
			ship.reset();
		}
	}

	public void Death(){
		if (!dead) {
			dead = true;
			if (!shipMode) {
				PlatformCamera camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PlatformCamera> ();
				camera.lockCamera = true;
				anim.SetBool ("dead", true);
			}
			Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D> ();
			foreach (Collider2D collider in colliders) {
				collider.isTrigger = true;
			}
			Rigidbody2D rb2d = transform.GetComponentInChildren<Rigidbody2D> ();
			if (!shipMode) {
				platformer.enabled = false;
			} else {
				ship.enabled = false;
			}
			rb2d.velocity = Vector2.zero;
			rb2d.AddForce (new Vector2 (0f, 500));
			rb2d.gravityScale = 1f;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("EnemyBullet")&&!dead) {
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
		anim.SetLayerWeight(1, value);
	}

	public void save() {
		saveToFile ();
	}

	public void save(Vector3 position) {
		saveToFile (position.x, position.y);
	}
}
