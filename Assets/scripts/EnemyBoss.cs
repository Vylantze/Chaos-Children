using UnityEngine;
using System.Collections;

public class EnemyBoss : Enemy {
	public bool defeated = false;
	public bool leftRightMovement = true;
	public float xSpeed = 1f;
	//public float ySpeed = 1f;
	int x_dir = 1; // 1 indicates movement to right, -1 is left

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
		if (defeated) {
			SceneManager.manager.nextLevel();
		}

		if (leftRightMovement) {
			LateralMovement();
		}

		if (rb2d.position == Vector2.zero) {
			Destroy(transform.gameObject);
		}
		
		if(dead) {
			defeated = true;
			dead = false; // only need to do this once
			if (gun!=null) {
				gun.enabled = false;
			}
			transform.tag = "ToBeDestroyed";
			if (master) {
				Scatter();
			}
			else {
				Death();
			}
		}
	}

	void LateralMovement() {
		rb2d.velocity = new Vector2 (xSpeed * x_dir, 0f);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.CompareTag ("BulletLimit")) {
			x_dir *= -1; // if it hits a bullet limit wall, invert direction
		}
	}
}
