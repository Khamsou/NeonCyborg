using UnityEngine;
using System.Collections;

public class ScaleEffect : MonoBehaviour {

	public float ScaleValue;

	private float _scaleValue;

	void Start () {
		_scaleValue = transform.localScale.x;
	}

	void Update () {
		_scaleValue += ScaleValue * Time.deltaTime;

		transform.localScale = new Vector3 (_scaleValue, _scaleValue, _scaleValue);
	}
}
