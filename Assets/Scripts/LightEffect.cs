using UnityEngine;
using System.Collections;

public class LightEffect : MonoBehaviour {

	public float LightValue;

	private float _lightValue;

	void Start () {
		_lightValue = GetComponent<Light> ().intensity;
	}

	void Update () {
		_lightValue += LightValue * Time.deltaTime;

		GetComponent<Light> ().intensity = _lightValue;
	}
}
