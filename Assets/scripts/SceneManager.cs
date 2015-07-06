using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public static SceneManager manager;
	public PlatformCamera cam;
	public string[] scenes = { "tutorial" };
	public int currentScene = 0;

	// Use this for initialization
	void Awake () {
		if (manager == null) {
			DontDestroyOnLoad (gameObject);
			manager = this;
		} else if (manager!=this) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string nextScene() {
		currentScene++;
		if (currentScene >= scenes.Length) {
			currentScene = 0;
		}
		return scenes [currentScene];
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			MasterPlayer.mainPlayer.Restart();
			MasterPlayer.mainPlayer.loadedFromFile = false;
			Application.LoadLevel(nextScene ());
		}
	}
}
