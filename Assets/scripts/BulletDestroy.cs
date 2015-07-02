using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.CompareTag ("Bullet")||
		    collider.CompareTag ("EnemyBullet")||
		    collider.CompareTag("ToBeDestroyed")) {
			Destroy(collider.gameObject);
		}
	}
}
