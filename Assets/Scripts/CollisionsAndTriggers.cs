using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CollisionsAndTriggers : MonoBehaviour {

	public Player Owner;

	void OnCollisionEnter2D(Collision2D other){
		if (other.collider.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			Owner.CancelAttack ();
			Camera.main.gameObject.GetComponent<CameraManager> ().Shake (0.5f);

			if (other.collider.tag == "Player") {
				GameObject.Find ("GameControl").GetComponent<GameControl> ().RespawnPlayer (other.gameObject);
				GameObject.Find ("GameControl").GetComponent<GameControl> ().AddPoints (other.gameObject.name);
				Instantiate (Resources.Load ("DeathSound"));
			}
		}
		if (other.collider.gameObject.layer == LayerMask.NameToLayer ("Shield")) {
			Owner.CancelAttack ();
			Owner.Repulse (other.contacts [0].point, 10.0f);
			Owner.Stun (0.5f);
		}
		if (other.collider.gameObject.layer == LayerMask.NameToLayer ("Weapon")) {
			Owner.CancelAttack ();
			if (other.gameObject.GetComponent<Player> () != null) {
				other.gameObject.GetComponent<Player> ().CancelAttack ();
			}
		}
	}
}
