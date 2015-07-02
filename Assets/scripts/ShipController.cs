using UnityEngine;
using System.Collections;

public class ShipController : MasterPlayer {

	public float max = 20f;

	public MasterPlayer master;

	private Rigidbody2D rb2d;
	private Sprite sprite;
	// Use this for initialization
	void Awake () {
		rb2d = GetComponent<Rigidbody2D> ();
		master = GetComponentInParent<MasterPlayer> ();
		master.shipMode = true;
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
}
