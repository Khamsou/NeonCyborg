using UnityEngine;
using System.Collections;

public class Trainer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, 200.0f * Time.deltaTime);
	}
}
