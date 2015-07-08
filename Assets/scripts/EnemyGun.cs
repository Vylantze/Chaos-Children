using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour {
	Enemy ec; // enemy controller
	bool active = false; // whether the gun is actively firing
	bool trace = false; // whether the gun always locks on player before firing

	float timer = 0f;

	// bullet details
	private BulletScript script;
	public Rigidbody2D bullet;
	public float bullet_speed = 20f;

	// shooting details
	private int numShots = 0; // number of shots currently fired
	public float angle = 0f; // angle to fire shots relative to enemy object (east is 0 degrees)
	public int totalShotsAllowed = 3; // how many bullets to fire before relooping
	public float waitTime = 3f; // amount of time to wait between loops
	public float shotWait = 0.4f; // time between individual shots

	// player location
	public Transform player; // Reference to the player's transform.

	// Use this for initialization
	void Awake () {
		ec = GetComponentInParent<Enemy> ();
		MasterPlayer master = GameObject.Find ("mainChara").GetComponent<MasterPlayer> ();
		if (master.shipMode) {
			player = GameObject.Find ("Starship").transform;
		} else {
			player = GameObject.Find("Platformer").transform;
		}
		timer = Time.time + 1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			check();
		} else if (numShots>0) {
			if (Time.time>timer) {
				numShots = 0;
			}
		}
	}

	void check() {
		float time = Time.time;
		if (numShots < totalShotsAllowed) {
			if (time > timer) {
				if (numShots==0&&!false) {
					calculateAngle();
				}
				fire ();
				timer = time+shotWait;
			}
		} else {
			numShots = 0;
			timer = time + waitTime;
		}
	}

	void calculateAngle() {
		// triangulate player position with enemy position to find angle
		float y = player.position.y - ec.transform.position.y; 
		float x = player.position.x - ec.transform.position.x; 
		float radian = Mathf.Atan (y / x);
		if (x < 0) {
			radian += Mathf.PI;
		}
		angle = radian;
	}

	void fire() {
		if (trace) {
			calculateAngle ();
		}
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
