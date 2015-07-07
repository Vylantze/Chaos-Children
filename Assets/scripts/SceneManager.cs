using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneManager : MonoBehaviour {
	public static SceneManager manager;
	public PlatformCamera cam;
	public string[] scenes = { "tutorial", "tutorialBoss" };
	public int currentScene = -1;

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

	public void loadScene() {
		
		Application.LoadLevel (scenes [currentScene]);
	}

	public void loadScene(string sceneName) {
		Application.LoadLevel (sceneName);
	}

	public void StartGame() {
		currentScene = 0;
		MasterPlayer.mainPlayer.loadPosition();
		loadScene ();
	}

	public void ContinueGame() {
		MasterPlayer master = MasterPlayer.mainPlayer;
		master.loadFromFile ();
		master.loadedFromFile = false; // to prevent auto save functions from triggering
		for (int i=0; i<scenes.Length;i++) {
			if (scenes[i]==master.stage_name) {
				currentScene = i;
				break;
			}
		}
		// if failed to find the name, start new game
		currentScene = 0;
		master.loadPosition();
		loadScene ();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			MasterPlayer.mainPlayer.Restart();
			MasterPlayer.mainPlayer.loadedFromFile = false;
			Application.LoadLevel(nextScene ());
		}
	}
}
