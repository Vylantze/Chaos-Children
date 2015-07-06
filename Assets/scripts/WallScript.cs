using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag ("Bullet")&&transform.tag!="ToBeDestroyed") {
			BulletScript bullet = collider.GetComponent<BulletScript>();
			
			if (!bullet.charged) { // if shot invalid
				AudioSource.PlayClipAtPoint(bullet.deflect, transform.position);
				Destroy (collider.gameObject);
			}
		}
	}
}
