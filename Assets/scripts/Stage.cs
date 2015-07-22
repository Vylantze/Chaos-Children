using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour {
	public Transform startPosition;
	public Vector3 spawnPoint;
	public bool platformStage = true;
	// Use this for initialization
	void Start () {
		MasterPlayer master = MasterPlayer.mainPlayer;
		if (master.loadedFromFile) {
			master.loadedFromFile = false;
			spawnPoint = MasterPlayer.mainPlayer.transform.position;
		} else {
			// spawn at start point
			master.gameObject.transform.position = startPosition.position;
		}
		if (master.shipMode == platformStage) {
			master.shipMode = !platformStage;
		}
		master.reset ();
		if (!platformStage) {
			save ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

	void save() {
		MasterPlayer.mainPlayer.save(MasterPlayer.mainPlayer.platformer.transform.position);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			// activate save function
			save ();
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.tag == "Player") {
			SceneManager.manager.nextLevel ();
		}
	}
}
