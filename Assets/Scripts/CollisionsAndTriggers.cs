using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CollisionsAndTriggers : MonoBehaviour {

	public Player Owner;

	/*
	 * This script is executed when a weapon collides with something
	 * */

	//When the weapon collides with
	void OnCollisionEnter2D(Collision2D other){
		//ANOTHER Weapon
		if (other.collider.gameObject.layer == LayerMask.NameToLayer ("Weapon")) {
			//GAMEPLAY EFFECTS
			Owner.CancelAttack ();
			Owner.Repulse (other.contacts [0].point, 10.0f);
			Owner.Stun (0.5f);
			Owner.Vibrate (0.8f);
			if (other.gameObject.GetComponent<Player> () != null) {
				other.gameObject.GetComponent<Player> ().CancelAttack ();
				other.gameObject.GetComponent<Player> ().Repulse (other.contacts [0].point, 10.0f);
				other.gameObject.GetComponent<Player> ().Stun (0.5f);
				other.gameObject.GetComponent<Player> ().Vibrate (0.8f);
			}

			//VISUAL EFFECTS
			Instantiate(Resources.Load("Effects/SwordHit"), other.contacts[0].point, Quaternion.identity);
			Instantiate(Resources.Load("Sounds/SwordHit" + Random.Range(1, 4) + "Sound"));

		//A Shield
		} else if (other.collider.gameObject.layer == LayerMask.NameToLayer ("Shield")) {
			//GAMEPLAY EFFECTS
			Owner.CancelAttack ();
			Owner.Repulse (other.contacts [0].point, 10.0f);
			Owner.Stun (0.5f);
			Owner.Vibrate (0.8f);
			if (other.gameObject.GetComponent<Player> () != null) {
				other.gameObject.GetComponent<Player> ().ConsumeStamina (15f);
				other.gameObject.GetComponent<Player> ().Vibrate (0.8f);
			}

			//VISUAL EFFECTS
			Camera.main.gameObject.GetComponent<CameraManager> ().Shake (0.1f);
			Instantiate(Resources.Load("Sounds/ParrySound"));
			Instantiate(Resources.Load("Sounds/SwordHit" + Random.Range(1, 4) + "Sound"));
			Instantiate(Resources.Load("Effects/SwordHit"), other.contacts[0].point, Quaternion.identity);
		//FLESH
		} else if (other.collider.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			Camera.main.gameObject.GetComponent<CameraManager> ().Shake (0.5f);
			Owner.Vibrate (0.5f);
			if (other.collider.tag == "Player") {
				Instantiate (Resources.Load ("Sounds/DeathSound"));
				Instantiate (Resources.Load ("Sounds/DieSound"));
				GameObject head = (GameObject)Instantiate (Resources.Load ("Effects/Head"), other.transform.position, Quaternion.identity);
				head.GetComponent<Rigidbody2D>().AddForce((head.transform.position - Owner.transform.position) * 500);
				//Instantiate (Resources.Load ("Effects/PlayerDeathEffect"), other.transform.position, Quaternion.identity);
				GameObject splat = (GameObject)Instantiate (Resources.Load ("Effects/BloodSplat" + Random.Range(1, 3)), other.transform.position, Quaternion.identity);
				splat.transform.Translate (0, 0, 1);
				GameObject.Find ("GameControl").GetComponent<GameControl> ().RespawnPlayer (other.gameObject);
				GameObject.Find ("GameControl").GetComponent<GameControl> ().AddPoints (Owner.name);

				if (other.gameObject.GetComponent<Player> () != null) {
					other.gameObject.GetComponent<Player> ().Vibrate (0);
				}
			}
		} 
	}
}
