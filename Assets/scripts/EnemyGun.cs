using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour {
	public bool active = false; // whether the gun is actively firing
	public bool homing = true; // whether the gun always locks on player before firing
	public bool traceShot = false;
	public bool spread = false;
	public bool beamSpam = false;

	// for selecting modes and stuff
	private bool[] compiled = new bool[4];
	private bool finished = true;
	private int selected = 0; // selected mode

	// global timer used by everyone
	private float timer = 0f;

	// spread
	public int numOfSpread = 5; // for spread
	public bool randomSpread = false;

	// beam
	public float beamDuration = 2f;
	float beamTimer = 0f;
	
	// bullet details
	private BulletScript script;
	public Rigidbody2D bullet;
	public float bullet_speed = 10f;
	
	// shooting details
	private int numShots = 0; // number of shots currently fired
	public float angle = 0f; // angle to fire shots relative to enemy object (east is 0 degrees)
	public int totalShotsAllowed = 3; // how many bullets to fire before relooping
	public float waitTime = 3f; // amount of time to wait between loops
	public float shotWait = 0.4f; // time between individual shots
	
	// player location
	public Transform player; // Reference to the player's transform.
	public Animator animator;
	// Use this for initialization
	void Start () {
		timer = Time.time + 1f;
		Random.seed = (int)timer;
		MasterPlayer master = MasterPlayer.mainPlayer;
		if (master.shipMode) {
			player = GameObject.Find ("Starship").transform;
		} else {
			player = GameObject.Find("Platformer").transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			float time = Time.time;
			if (finished) { // randomise between selected modes
				compiled[0] = homing; // whether the gun always locks on player before firing
				compiled[1] = traceShot;
				compiled[2] = spread;
				compiled[3] = beamSpam;
				do {
					selected = Random.Range(0,4);
				}while(!compiled[selected]); // as long as it is not valid, keep looping
				finished = false;
			}
			switch(selected) {
			case 1:TraceShot(time); break;
			case 2:SpreadGun(time); break;
			case 3:Beam (time); break;
			default: HomingShot (time); break;
			}
		}
	}

	void activateAnimator() {
		if (animator != null) {
			animator.SetTrigger ("attack");
		}
	}

	void Beam(float time) {
		if (beamSpam && time < beamTimer) {
			if (numShots == 0) {
				activateAnimator();
				angle = 1.5f * Mathf.PI;
				beamTimer = beamDuration + time;
			}
			fire ();
		} else if (numShots>0) {
			timer = time + waitTime;
			numShots = 0;
			finished = true;
		} else if (time>timer) {
			beamTimer = beamDuration + time;
		}
	}

	void SpreadGun(float time) {
		if (numShots>0) {
			numShots = 0;
			timer = time + waitTime;
			finished = true;
		}
		else if (time > timer) {
			activateAnimator();
			int numToDivide;
			if (randomSpread) {
				numToDivide = Random.Range(2,numOfSpread+2); 
				// since min is 2 for 1 bullet
				// and max is not included in range
			} else {
				numToDivide = numOfSpread+1; // to reduce number of times needed to add in loop itself
			}
			//for (int i=0; i<numOfBullets; i++) {
			for (int i=1; i<numToDivide; i++) {
				angle = -Mathf.PI / numToDivide *i;//(i + 1);
				fire ();
			}
		}
	}

	void HomingShot(float time) {
		if (numShots < totalShotsAllowed && time > timer) {
			if (numShots==0){ 
				activateAnimator();
			}
			calculateAngle ();
			fire ();
			timer = time+shotWait;
			// wait time between individual shots inside the bursts themselves
		} else if (numShots>0&&time>timer) {
			timer = time + waitTime - shotWait;
			numShots = 0;
			finished = true;
		}
	}

	void TraceShot(float time) {
		if (numShots < totalShotsAllowed && time > timer) {
			if (numShots==0) {
				activateAnimator();
				calculateAngle ();
			}
			fire ();
			timer = time+shotWait; 
			// wait time between individual shots inside the bursts themselves
		} else if (numShots>0&&time>timer) {
			timer = time + waitTime - shotWait;
			numShots = 0;
			finished = true;
		}
	}
	
	void calculateAngle() {
		// triangulate player position with enemy position to find angle
		float y = player.position.y - transform.position.y; 
		float x = player.position.x - transform.position.x; 
		float radian = Mathf.Atan (y / x);
		if (x < 0) {
			radian += Mathf.PI;
		}
		angle = radian;
	}
	
	void fire() {
		numShots++;
		Rigidbody2D shot = Instantiate (bullet, transform.position, Quaternion.identity) as Rigidbody2D;
		shot.tag = "EnemyBullet";
		//shot.velocity = new Vector2 (bullet_speed, 0f);
		shot.velocity = new Vector2 (bullet_speed*Mathf.Cos(angle),bullet_speed*Mathf.Sin(angle));
		shot.rotation = angle * 180 / Mathf.PI;
	}
	
	void OnTriggerExit2D(Collider2D collider) {
		if (collider.CompareTag ("BulletLimit")) { // if exit the bullet limit, deactivate
			active = false;
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("BulletLimit")) { // if enter the bullet limit, activate
			active = true;
		}
	}
}
