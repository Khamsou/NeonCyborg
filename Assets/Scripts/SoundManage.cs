using UnityEngine;
using System.Collections;

public class SoundManage : MonoBehaviour {
	void Update () {
		if (!GetComponent<AudioSource> ().isPlaying) {
			Destroy (gameObject);
		}
	}
}
