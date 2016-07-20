using UnityEngine;
using System.Collections;

public class DeathTimer : MonoBehaviour {

	public float LifeTime;

	private float _timer;

	void Start () {
		_timer = 0;
	}

	void Update () {
		_timer += Time.deltaTime;

		if (_timer > LifeTime) {
			Destroy (gameObject);
		}
	}
}
