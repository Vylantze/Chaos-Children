using UnityEngine;
using System.Collections;

public class Stage0 : MonoBehaviour {
	public PlatformCamera cam;

	public Transform startPosition;
	public Vector3 spawnPoint;
	// Use this for initialization
	void Start () {
		if (MasterPlayer.mainPlayer.loadedFromFile) {
			MasterPlayer.mainPlayer.loadedFromFile = false;
			spawnPoint = MasterPlayer.mainPlayer.transform.position;
		} else {
			spawnPoint = startPosition.position;
			Restart ();
		}

	}

	public void Restart() {
		MasterPlayer.mainPlayer.gameObject.transform.position = spawnPoint;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			MasterPlayer.mainPlayer.save(MasterPlayer.mainPlayer.platformer.transform.position);
			spawnPoint = MasterPlayer.mainPlayer.transform.position;
		}
	}
}
