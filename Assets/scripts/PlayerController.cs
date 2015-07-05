/*
 * 
 * 
 * Uses direct velocity manipulation
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
*/

using UnityEngine;
using System.Collections;

public class PlayerController : MasterPlayer {
	public bool jump = false;
	public bool facingRight = true;
	public float moveForce = 365f;
	public float maxHoriSpeed = 10f;
	public float maxVerticalSpeed = 20f;
	public float jumpForce = 1000f;
	public GameObject female_chara;
	public GameObject male_chara;

	public MasterPlayer master;
	private Rigidbody2D rb2d;
	Collider2D[] colliders;

	// Use this for initialization
	void Start () {
		colliders = GetComponentsInChildren<Collider2D>();
		rb2d = GetComponentInParent<Rigidbody2D> ();
	}
	// Update is called once per frame
	void Update () {
		if (!master.dead) {
			// Jump
			keyboard();
			if (Input.GetButtonDown("Jump")) {
				jump = true;
			}
			movement (master.shipMode);
			movementPlatform ();
			Flip ();
		}
	}

	void Flip() {
		if (master.flip!= facingRight) {
			master.flip = facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

	public void movementPlatform() { // can access stage directly, but will receive error if not on stage
		// put limiters on speed here
		if (xvel > maxHoriSpeed) { xvel = (int)maxHoriSpeed; } 
		else if (xvel < -maxHoriSpeed) { xvel = (int)-maxHoriSpeed; }
		yvel = rb2d.velocity.y;

		//if (jump && yvel == 0f) {
		//	rb2d.AddForce (new Vector2 (0f, jumpForce));
		/*// Old code
		if (jump&&!master.shipMode) {
			foreach(Collider2D collider in colliders) {
				if (collider.IsTouchingLayers(LayerMask.GetMask("Ground"))&&Mathf.Abs(yvel)<=0.3f) {
					rb2d.AddForce (new Vector2 (0f, jumpForce));
					master.SetTrigger("jump");
					break;
				}
			}
		} else {
		}//*/
		
		anim.SetBool ("in_air", true);
		master.in_air = true;
		foreach(Collider2D collider in colliders) {
			if (collider.IsTouchingLayers(LayerMask.GetMask("Ground"))&&Mathf.Abs(yvel)<=0.3f) {
				anim.SetBool ("in_air", false);
				master.in_air = false;
				break;
			}
		}

		if (jump && !master.in_air) {
			rb2d.AddForce (new Vector2 (0f, jumpForce));
			master.SetTrigger("jump");
		}
		
		jump = false;

		if (xvel > 0) {
			facingRight = true;
		} else if (xvel < 0) {
			facingRight = false;
		}
		// if statement is true, in air. if false, not
		rb2d.velocity = new Vector2(xvel * 1f, yvel);

		anim.SetFloat ("speed", Mathf.Abs (xvel));
	}
}
