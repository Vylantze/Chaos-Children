using UnityEngine;
using System.Collections;

public class ShipController : PlayerController {

	public float max = 20f;

	private MasterPlayer master;

	private Rigidbody2D rb2d;
	private Sprite sprite;
	Collider2D[] colliders;
	public Animator charge;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		master = MasterPlayer.mainPlayer;
		colliders = GetComponentsInChildren<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!master.dead) {
			keyboard ();
			movement (master.shipMode);
			movementShip ();
		}
	}

	public void movementShip() { // can access stage directly, but will receive error if not on stage
		// put limiters on speed here
		if (xvel > max) { xvel = (int)max; } 
		else if (xvel < -max) { xvel = (int)-max; }
		
		if (yvel > max) { yvel = (int)max; } 
		else if (yvel < -max) { yvel = (int)-max; }
		
		//if (jump && yvel == 0f) {
		//	rb2d.AddForce (new Vector2 (0f, jumpForce));

		// if statement is true, in air. if false, not
		rb2d.velocity = new Vector2(xvel * 1f, yvel* 1f);
	}

	public void colliderEnabled(bool value) {
		foreach (Collider2D collider in colliders) {
			if (collider!=null) {
				collider.enabled = value;
				collider.isTrigger = !value;
			}
		}
	}

	
	public void reset() {
		transform.localPosition = Vector3.zero;
		colliderEnabled (true);
		rb2d.gravityScale = 0f;
		rb2d.velocity = Vector2.zero;
		xvel = yvel = 0f;
		_up = _down = _left = _right = 0;
		_godown = _goright = false;
		spriteEnabled (true);
	}
	
	
	public void spriteEnabled(bool value) {
		SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sprite in sprites) {
			sprite.enabled = value;
		}
	}
	
	public void disable() {
		reset ();
		colliderEnabled (false);
		spriteEnabled (false);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		master.triggerCheck (collider);
	}
}
