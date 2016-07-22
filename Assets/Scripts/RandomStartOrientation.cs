using UnityEngine;
using System.Collections;

public class RandomStartOrientation : MonoBehaviour {

	public bool xRot;
	public bool yRot;
	public bool zRot;

	private float _xRot = 0;
	private float _yRot = 0;
	private float _zRot = 0;

	void Start () {
		if(xRot) _xRot = Random.Range (0.0f, 360.0f);
		if(yRot) _yRot = Random.Range (0.0f, 360.0f);
		if(zRot) _zRot = Random.Range (0.0f, 360.0f);

		transform.localEulerAngles = new Vector3 (_xRot, _yRot, _zRot);
	}
}
