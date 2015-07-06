using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerController : MonoBehaviour {
	// keyboard
	protected bool _godown, _goright;
	protected int _up, _down, _left, _right;
	
	//movement
	protected float xvel, yvel;
	public float acl = 3f;
	protected Animator anim;// Reference to the player's animator component.

	// data to save
	public bool shipMode = false;
	public bool female = true;
	// colour change
	public bool[] elements = {true, false, false, false}; // elements available
	public string[] commands = {"None", "FireMode", "ThunderMode", "IceMode"};
	// 0 = NONE = true;
	// others are in order

	// Stage stuff
	public string stage_name = "";
	public bool loadedFromFile = false;
	protected Vector3 loadedPosition;

	// Use this for initialization
	void Start () {
		_up = _down = _left = _right = 0;
		_godown = _goright = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void keyboard() {
		// Down inputs
		// Vertical
		if (Input.GetButtonDown("Up")) {
			_up = 1;
			_godown = false;
		}
		else if (Input.GetButtonDown("Down")) { 
			_down = -1;
			_godown = true;
		}
		// Horizontal
		if (Input.GetButtonDown ("Right")) {
			_right = 1;
			_goright = true;
		}
		else if (Input.GetButtonDown ("Left")) { 
			_left = -1;
			_goright = false;
		}
		
		// Release inputs
		// Vertical
		if (Input.GetButtonUp("Up")) {
			_up = 0;
			_godown = true;
		}
		else if (Input.GetButtonUp("Down")) { 
			_down = 0;
			_godown = false;
		}
		// Horizontal
		if (Input.GetButtonUp("Right")) {
			_right = 0;
			_goright = false;
		}
		else if (Input.GetButtonUp("Left")) { 
			_left = 0;
			_goright = true;
		}
	}
	
	protected void movement(bool _shipMode) {
		int x_dir, y_dir;
		//y_dir = up_dir + down_dir;
		if (_goright) { // priority is right
			x_dir = _right;
		}
		else {
			x_dir = _left;
		}
		
		if (_godown) {
			y_dir = _down;
		}
		else {
			y_dir = _up;
		}
		
		// X VELOCITY
		if (x_dir == 0) { 
			if (xvel > 0) { xvel--; } else if (xvel < 0) { xvel++; }
			else { 
				
			}
		}
		else { 
			if ( (xvel > 0 &&x_dir < 0)||(xvel < 0 && x_dir > 0) )  {
				xvel = 0;
			}
			xvel += x_dir * acl;
			//xvel = x_dir*maxHoriSpeed;
		}
		// Y VELOCITY
		//if (shipMode) {
			if (y_dir == 0) { 
				if (yvel > 0) {
					yvel--;
				} else if (yvel < 0) {
					yvel++;
				}
			} else { 
				if ((yvel > 0 && y_dir < 0) || (yvel < 0 && y_dir > 0)) {
					yvel = 0;
				}
				yvel += y_dir * acl;
			}
			
		//}//*/
	}
	
	protected void saveToFile(float x_coor, float y_coor) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create(Application.dataPath + "/saveData.dat");
		SaveData data = new SaveData (this);
		data.x_coor = x_coor;
		data.y_coor = y_coor;
		bf.Serialize (file, data);
		file.Close ();
	}

	protected void saveToFile() {
		Vector3 location = transform.position;
		float x_coor = location.x;
		float y_coor = location.y;
		saveToFile(x_coor, y_coor);
	}

	protected void loadFromFile() {
		if (File.Exists (Application.dataPath + "/saveData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.dataPath + "/saveData.dat", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			PlayerController player = this;
			load (data);
			player.loadedFromFile = true;
			file.Close ();
		}
	}

	protected void load(SaveData data) {
		shipMode = data.shipMode;
		female = data.female;
		elements = data.elements;
		stage_name = data.stage_name;
		loadedPosition = new Vector3(data.x_coor, data.y_coor, 0f);
	}
}

[Serializable]
public class SaveData {
	public bool shipMode;
	public bool female;
	// colour change
	public bool[] elements; // elements available
	public string stage_name;
	public float x_coor;
	public float y_coor;

	public SaveData(PlayerController player) {
		save (player);
	}

	public void save(PlayerController player){
		shipMode = player.shipMode;
		female = player.female;
		elements = player.elements;
		stage_name = Application.loadedLevelName;
		Vector3 location = player.transform.position;
		x_coor = location.x;
		y_coor = location.y;
	}

	public void load(ref PlayerController player) {
		player.shipMode = shipMode;
		player.female = female;
		player.elements = elements;
		player.stage_name = stage_name;
		player.transform.position = new Vector3(x_coor, y_coor, 0f);
	}
}
