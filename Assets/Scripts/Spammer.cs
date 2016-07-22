using UnityEngine;
using System.Collections;

public class Spammer : MonoBehaviour {

	public string[] ObjectsToCreate;
	public float Step;
	public float Lifetime;
	public bool Special;

	private float _timer;
	private float _globalTimer;
	// Use this for initialization
	void Start () {
		_timer = 0f;
		_globalTimer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		_timer += Time.deltaTime;
		_globalTimer += Time.deltaTime;

		if (_timer > Step) {
			_timer = 0;
			GameObject test = (GameObject)Instantiate (Resources.Load (ObjectsToCreate[Random.Range(0, ObjectsToCreate.Length)]), transform.position, Quaternion.identity);
			if(Special) test.transform.Translate (0, 0, 1);
		}

		if (_globalTimer > Lifetime && Lifetime != 0) {
			Destroy (this);
		}
	}
}
