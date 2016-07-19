using UnityEngine;
using System.Collections;

public class InputDebug : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		for (int i = 0 ; i < 20; i++) {
			if(Input.GetKeyDown("joystick 1 button "+i)){
				print("joystick 1 button "+i);
			}
		}
	}
}
