using UnityEngine;
using System.Collections;

public class Stage0 : MonoBehaviour {
	public Transform startPosition;
	public Vector3 spawnPoint;
	// Use this for initialization
	void Start () {
		if (MasterPlayer.mainPlayer.loadedFromFile) {
			MasterPlayer.mainPlayer.loadedFromFile = false;
			spawnPoint = MasterPlayer.mainPlayer.transform.position;
		} else {
			// spawn at start point
			MasterPlayer.mainPlayer.gameObject.transform.position = startPosition.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			// activate save function
			MasterPlayer.mainPlayer.save(MasterPlayer.mainPlayer.platformer.transform.position);
		}
	}
}
