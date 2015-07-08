using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour {
	public Transform startPosition;
	public Vector3 spawnPoint;
	public bool platformStage = true;
	public Enemy boss;
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
		if (!platformStage) {
			if (boss.HP<0||boss==null) {
				SceneManager.manager.nextLevel();
			}
		}
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
}
