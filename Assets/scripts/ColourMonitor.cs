using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColourMonitor : MonoBehaviour {
	public int currentColour = -1;
	public Image[] activePower;
	public ModeChange player;
	// Use this for initialization
	void Start () {
		activePower = GetComponentsInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.currentColour!=currentColour) {
			currentColour = player.currentColour;
			selectSprite ();
		}
	}

	void selectSprite() {
		for (int i=0; i<4; i++) {
			activePower [i].enabled = i == currentColour;
		}
	}
}
