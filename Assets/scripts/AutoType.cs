using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoType : MonoBehaviour {
	public float letterPause = 0.005f;
	public string message;
	public Text text;
	public bool autoMode = true;

	// Use this for initialization
	void Start () {
		//text = GetComponent<Text>();
		reset ();
		StartCoroutine(TypeText());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void reset() {
		autoMode = true;
		message = text.text;
		text.text = "";
	}

	IEnumerator TypeText () {
		while(true) {
			//foreach (char letter in message.ToCharArray()) {
			if (message.Length>0&&autoMode) {
				text.text += message[0];
				message = message.Substring(1);
			}
			else if (message.Length>0) { // if not auto mode
				// post the text regardless of what happens
				text.text += message;
			}
			yield return 0;
			yield return new WaitForSeconds (letterPause);
		}      
	}
}
