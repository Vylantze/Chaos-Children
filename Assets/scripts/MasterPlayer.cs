using UnityEngine;
using System.Collections;

public class MasterPlayer : MonoBehaviour {
	public bool shipMode = false;
	public bool female = true;
	
	protected Animator anim;// Reference to the player's animator component.
	private PlayerController platformer;
	private ShipController ship;
	public bool flip = true; // true = faceright, false = faceleft
	public bool in_air = false;

	// health
	public bool dead = false;
	public Debug debug;

	// keyboard
	protected bool _godown, _goright;
	protected int _up, _down, _left, _right;

	//movement
	protected float xvel, yvel;
	public float acl = 3f;

	// Use this for initialization
	void Awake () {
		_up = _down = _left = _right = 0;
		_godown = _goright = false;
		if (shipMode) {
			ship = GetComponentInChildren<ShipController> ();
		} else {
			platformer = GetComponentInChildren<PlayerController> ();
			loadAnimator ();
		}
	}

	void loadAnimator() {
		// force the renders to eitehr be in existence or not
		platformer.female_chara.GetComponent<SpriteRenderer>().enabled = female;
		platformer.male_chara.GetComponent<SpriteRenderer>().enabled= !female;
		if (female) {
			anim = platformer.female_chara.GetComponent<Animator>(); // load female animator
			platformer.female_chara.GetComponent<SpriteRenderer>().enabled=true;
			platformer.male_chara.GetComponent<SpriteRenderer>().enabled=false;
		} else { // else if male
			anim = platformer.male_chara.GetComponent<Animator>();; // load male animator
		}
		platformer.anim = anim;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Switch Mode")&&debug.debug) {
			female = !female;
			// true is female
			// false is male
			loadAnimator ();
		}
		if (Input.GetButtonDown("SwitchScene")&&debug.debug) {
			shipMode = !shipMode;
			ship.gameObject.SetActive(shipMode);
			platformer.gameObject.SetActive(!shipMode);
		}
	}

	void Restart() {
		Application.LoadLevel(Application.loadedLevel);
	}

	public void SetTrigger(string value) {
		anim.SetTrigger (value);
	}

	protected void keyboard() {
		// Down inputs
		// Vertical
		if (Input.GetButtonDown("Up")) {
			_up = 1;
			_godown = false;
		}
		else if (Input.GetButtonDown("Down")) { 
			_down = -1;
			_godown = true;
		}
		// Horizontal
		if (Input.GetButtonDown ("Right")) {
			_right = 1;
			_goright = true;
		}
		else if (Input.GetButtonDown ("Left")) { 
			_left = -1;
			_goright = false;
		}
		
		// Release inputs
		// Vertical
		if (Input.GetButtonUp("Up")) {
			_up = 0;
			_godown = true;
		}
		else if (Input.GetButtonUp("Down")) { 
			_down = 0;
			_godown = false;
		}
		// Horizontal
		if (Input.GetButtonUp("Right")) {
			_right = 0;
			_goright = false;
		}
		else if (Input.GetButtonUp("Left")) { 
			_left = 0;
			_goright = true;
		}
	}

	protected void movement(bool _shipMode) {
		int x_dir, y_dir;
		//y_dir = up_dir + down_dir;
		if (_goright) { // priority is right
			x_dir = _right;
		}
		else {
			x_dir = _left;
		}
		
		if (_godown) {
			y_dir = _down;
		}
		else {
			y_dir = _up;
		}

		// X VELOCITY
		if (x_dir == 0) { 
			if (xvel > 0) { xvel--; } else if (xvel < 0) { xvel++; }
			else { 
				
			}
		}
		else { 
			if ( (xvel > 0 &&x_dir < 0)||(xvel < 0 && x_dir > 0) )  {
				xvel = 0;
			}
			xvel += x_dir * acl;
			//xvel = x_dir*maxHoriSpeed;
		}
		// Y VELOCITY
		if (shipMode) {
			if (y_dir == 0) { 
				if (yvel > 0) {
					yvel--;
				} else if (yvel < 0) {
					yvel++;
				}
			} else { 
				if ((yvel > 0 && y_dir < 0) || (yvel < 0 && y_dir > 0)) {
					yvel = 0;
				}
				yvel += y_dir * acl;
			}
			
		}//*/
	}
	
	void Death(){
		dead = true;
		if (!shipMode) {
			PlatformCamera camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PlatformCamera> ();
			camera.lockCamera = true;
			anim.SetBool ("dead",true);
		}
		Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D>();
		foreach (Collider2D collider in colliders) {
			collider.isTrigger = true;
		}
		Rigidbody2D rb2d = transform.GetComponentInChildren<Rigidbody2D> ();
		rb2d.AddForce (new Vector2 (0f, 500));
		rb2d.velocity = new Vector2 (0f, 0f);
		rb2d.gravityScale = 1f;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("EnemyBullet")&&!dead) {
			Death ();
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (dead&&collider.CompareTag ("MapLimit")) {
			Restart ();
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
}
