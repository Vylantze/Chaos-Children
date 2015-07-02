using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	//PlayerController pc;
	public float bullet_speed = 15f;
	public float damage = 0.17f; // damage dealt
	public Rigidbody2D rb2d;
	public bool charged = false;
	public AudioClip normalShot;
	public AudioClip chargedShot;
	// Use this for initialization
	void Awake () {
		rb2d = GetComponent<Rigidbody2D> ();
		//pc = GetComponentInParent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb2d.position == Vector2.zero) {
			Destroy(transform.gameObject);
		}
	}

	/*
	public void fire() {
		Vector3 bullet_pos = transform.position;
		GameObject temp = (GameObject)Instantiate (transform.gameObject,bullet_pos, Quaternion.identity);
		temp.SetActive (true);
		Rigidbody2D rb2d = temp.transform.GetComponent<Rigidbody2D>();
		
		if (pc.facingRight) {
			rb2d.velocity = new Vector2 (bullet_speed, 0f);
		} else {
			rb2d.velocity = new Vector2 (-bullet_speed, 0f);
		}
	}*/
}
