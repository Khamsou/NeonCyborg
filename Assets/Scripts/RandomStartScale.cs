using UnityEngine;
using System.Collections;

public class RandomStartScale : MonoBehaviour {

	private float _scale;

	void Start () {
		_scale = Random.Range (0.2f, 1.2f);
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3 (_scale, _scale, _scale), Time.deltaTime * 2.0f);
	}
}
