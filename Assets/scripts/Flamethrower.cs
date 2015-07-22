using UnityEngine;
using System.Collections;

public class Flamethrower : MonoBehaviour {
	public float force;
	ModeChange colour;
	public int type = ModeChange.FIRE;
	private bool allowBoost = true;
	// Use this for initialization
	void Start () {
		colour = GetComponent<ModeChange>();
		colour.currentColour = type;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.CompareTag ("Player")) {
			allowBoost = true;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Player")&&allowBoost ){
			allowBoost = false;
			MasterPlayer master = MasterPlayer.mainPlayer;
			bool sameColour = colour.compare(collider);
			if (sameColour) { // if same colour, give a boost
				if (!master.dead) { // and if chara not dead
					master.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0, force));
				}
			}
			else if (!sameColour) {
				master.Death();
			}
		}
	}
}
