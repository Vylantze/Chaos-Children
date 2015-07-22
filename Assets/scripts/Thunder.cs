using UnityEngine;
using System.Collections;

public class Thunder : MonoBehaviour {
	ModeChange colour;
	public int type = ModeChange.THUNDER;
	// Use this for initialization
	void Start () {
		colour = GetComponent<ModeChange>();
		colour.currentColour = type;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionStay2D(Collision2D collision) {
		if (collision.collider.CompareTag ("Player") ){
			MasterPlayer master = MasterPlayer.mainPlayer;
			bool sameColour = colour.compare(collision.collider);
			if (!sameColour) { // if not same colour, kill them
				master.Death();
			}
		}
	}
}
