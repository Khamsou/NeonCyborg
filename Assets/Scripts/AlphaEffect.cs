using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlphaEffect : MonoBehaviour {

	public float AlphaValue;

	private float _alphaValue;

	void Start () {
		if (GetComponent<Text> () != null) {
			_alphaValue = GetComponent<Text> ().color.a;
		} else {
			_alphaValue = GetComponent<SpriteRenderer> ().color.a;
		}
	}

	void Update () {
		_alphaValue += AlphaValue * Time.deltaTime;

		if (GetComponent<Text> () != null) {
			GetComponent<Text> ().color = new Color (GetComponent<Text> ().color.r, GetComponent<Text> ().color.g, GetComponent<Text> ().color.b, _alphaValue);
		} else {
			GetComponent<SpriteRenderer> ().color = new Color (GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, _alphaValue);
		}
	}
}
