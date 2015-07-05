using UnityEngine;
using System.Collections;

public class Stage0 : MonoBehaviour {
	public Transform player;
	public Transform startPosition;
	// Use this for initialization
	void Start () {
		Restart ();
	}

	public void Restart() {
		player.position = startPosition.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
