using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPMonitor : MonoBehaviour {
	public Enemy enemy;
	public float HP;
	Image bar;
	// Use this for initialization
	void Start () {
		HP = enemy.HP;
		bar = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		bar.transform.localScale = new Vector3 (1f, enemy.HP / HP);
	}
}
