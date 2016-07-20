using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private Vector3 _startPosition;

	private float _range;
	private float _shakeTime;
	private float _shakeTimer;

	void Start () {
		_startPosition = transform.position;

		_range = 2;
		_shakeTime = 0;
		_shakeTimer = 0;
	}

	void Update () {
		_shakeTimer += Time.deltaTime;
		if (_shakeTimer < _shakeTime) {
			Vector3 newPos = new Vector3 (_startPosition.x + Random.Range (-_range, _range), _startPosition.y + Random.Range (-_range, _range), _startPosition.z);

			transform.position = Vector3.Lerp (transform.position, newPos, Time.deltaTime * 8);
		} else {
			transform.position = Vector3.Lerp(transform.position, _startPosition, Time.deltaTime*8);
		}
	}

	public void Shake(float duration){
		_shakeTime = duration;
		_shakeTimer = 0;
	}
}
