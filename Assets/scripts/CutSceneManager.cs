using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutSceneManager : MonoBehaviour {
	public Image image;
	public AutoType _text;
	public Image background;
	public Text _name;
	string[] script = {};
	int currentLine = 0;
	public TextAsset[] assets;

	public bool cutSceneMode = false;
	// Use this for initialization
	void Start () {
		currentLine = 0;
	}

	void enableAll(bool value) {
		image.enabled = _text.enabled = _name.enabled = background.enabled = value;
	}

	void loadScript(int num) {
		string[] temp = { System.Environment.NewLine };
		script = assets[num].text.Split (temp,System.StringSplitOptions.RemoveEmptyEntries);
	}

	public void nextLine() {
		if (script [currentLine] == "END") {
			SceneManager.manager.cutSceneMode = false;
		} else {
			nextLine(script[currentLine]);
		}
		currentLine++;
	}

	public void nextLine(string line) {
		int location = line.IndexOf (":");
		if (location != -1) {
			nextLine (line.Substring (0, location), line.Substring (location + 1));
		} else {
			SceneManager.manager.cutSceneMode = false;
		}
	}

	public void nextLine(string name, string line) {
		_name.text = name;
		_text.message = line;
	}

	// Update is called once per frame
	void Update () {
		if (cutSceneMode != SceneManager.manager.cutSceneMode) {
			cutSceneMode = SceneManager.manager.cutSceneMode;
			MasterPlayer.mainPlayer.cutSceneMode (cutSceneMode);
			enableAll (cutSceneMode);
			//currentLine = 0;
		}

		if (cutSceneMode) {
			if (Input.GetButtonDown("Pause")) {

			}
			else if (Input.anyKeyDown) { // get any other button
				nextLine();
			}
		}
	}
}
