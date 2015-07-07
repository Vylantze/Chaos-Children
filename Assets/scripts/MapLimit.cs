using UnityEngine;
using System.Collections;

public class MapLimit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.CompareTag ("Player")) {
			//GetComponentInParent<Stage0>().Restart();
			MasterPlayer.mainPlayer.Restart();
		}
	}
}
