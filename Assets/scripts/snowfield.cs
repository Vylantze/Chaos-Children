using UnityEngine;
using System.Collections;

public class snowfield : MonoBehaviour {
	//private Transform player;		// Reference to the player's transform.
	public GameObject ground;
	[HideInInspector] private BoxCollider2D[] colliders; //array
	public int maxTiles = 5; // in either direction
	// Use this for initialization
	void Start() {
		//player = GameObject.Find ("mainChara").transform;
		//ground = GameObject.Find ("winterground_0");
		Spawn ();
		//GattaiAllColliders ();
	}

	private void GattaiAllColliders() {
	}

	/*
	public Bounds master_bound;
	public Bounds holder_bound;
	public Vector2 master_max;
	public Vector2 holder_max;
	public Vector2 master_min;
	public Vector2 holder_min;*/
	// master is the one to be kept. holder is the one to be added
	public void recalibrateCollider(BoxCollider2D master, BoxCollider2D holder) {
		Bounds boundary = master.bounds; // quicker access
		Vector2 min = boundary.min;
		Vector2 max = boundary.max;



		boundary = holder.bounds;// same
		if ( (boundary.min.x < min.x)||(boundary.min.y < min.y) ) { // if min is not smaller
			min = boundary.min;
		} 
		if ((boundary.max.x > max.x)||(boundary.max.y > max.y)) { // then the max must be bigger
			max = boundary.max;
		} else { // something weird happened
		}

		boundary.SetMinMax (min, max);
		master.size = new Vector2 (boundary.extents.x * 2f, boundary.extents.y * 2f);
		master.offset = new Vector2 (boundary.center.x - master.transform.position.x, 
		                             boundary.center.y - master.transform.position.y);
		Destroy(holder);
	}
	
	private void Spawn() {
		GameObject temp;
		GameObject instantiator = (GameObject)Instantiate (ground, ground.transform.position, Quaternion.identity);
		BoxCollider2D ground_collider = ground.GetComponent<BoxCollider2D>();
		float y = ground.transform.position.y;
		Vector2 origin;

		for (int i=1; i <maxTiles; i++) {
			origin = new Vector2(i *1f, y);
			temp = (GameObject)Instantiate(instantiator, origin, Quaternion.identity);
			temp.transform.parent = transform;
			recalibrateCollider(ground_collider, temp.GetComponent<BoxCollider2D>());

			origin = new Vector2(-i *1f, y);
			temp = (GameObject)Instantiate(instantiator, origin, Quaternion.identity);
			temp.transform.parent = transform;
			recalibrateCollider(ground_collider, temp.GetComponent<BoxCollider2D>());//*/
		}
		Destroy (instantiator);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
