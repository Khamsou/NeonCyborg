using UnityEngine;
using System.Collections;

public class AlphaEffect : MonoBehaviour {

	public float AlphaValue;

	private float _alphaValue;

	void Start () {
		_alphaValue = GetComponent<SpriteRenderer> ().color.a;
	}

	void Update () {
		_alphaValue += AlphaValue * Time.deltaTime;

		GetComponent<SpriteRenderer> ().color = new Color (GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, _alphaValue);
	}
}
