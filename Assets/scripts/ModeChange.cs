using UnityEngine;
using System.Collections;

public class ModeChange : MonoBehaviour {
	public const int NONE = 0;
	public const int RED = 1;
	public const int YELLOW = 2;
	public const int BLUE = 3;
	public const int FIRE = RED;
	public const int THUNDER = YELLOW;
	public const int ICE = BLUE;
	public const int YEL = YELLOW;
	public int currentColour = NONE;

	private MasterPlayer master;
	
	public bool isObject = false; // is this attached to an interactable object?
	public SpriteRenderer[] activePower;

	// Use this for initialization
	void Start () {
		master = MasterPlayer.mainPlayer;
		if (isObject) { // if it is an object, it has the 4 colours
			activePower = GetComponentsInChildren<SpriteRenderer> ();
			selectSprite(); // and thus needs to have a sprite colour
		}
	}

	// compares the current colour to the collider's colour
	// general tool
	public bool compare(Collider2D collider) {
		ModeChange mode = collider.GetComponentInChildren<ModeChange> ();
		if (mode == null) {
			mode = collider.GetComponentInParent<ModeChange> ();
			return compare (mode);
		}
		else {
			int colour = mode.currentColour;
			return currentColour == colour; // if they are the same, return true
		}
	}

	// overloaded version
	public bool compare(ModeChange mode) {
		if (mode == null) {
			print("Error with mode");
			return false;
		}
		else {
			int colour = mode.currentColour;
			return currentColour == colour; // if they are the same, return true
		}
	}

	// METHODS FOR WHEN MODECHANGE IS ATTACHED TO AN OBJECT NOT THE PLAYER
	
	void selectSprite() {
		for (int i=0; i<4; i++) {
			activePower [i].enabled = i == currentColour;
		}
	}

	// for interaction if it is an object
	void OnTriggerEnter2D(Collider2D collider) {
		if (isObject&&collider.CompareTag ("Player")) {
			master = collider.GetComponentInParent<MasterPlayer>();
			master.elements[currentColour] = true;
			collider.GetComponentInParent<ModeChange>().currentColour = currentColour;

			// get the master player
			// get the current colour this sphere is possessing
			// set the element to true so that the player can access it
			Destroy(transform.gameObject); // destroy this object after using it
		}
	}
}
