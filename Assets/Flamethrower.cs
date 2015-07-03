using UnityEngine;
using System.Collections;

public class Flamethrower : MonoBehaviour {
	public float force;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Player")) {
			MasterPlayer master = collider.GetComponentInParent<MasterPlayer>();
			if (!master.dead) { // if chara not dead
				master.GetComponentInChildren<Rigidbody2D>().AddForce(new Vector2(0, force));
			}
		}
	}
}
