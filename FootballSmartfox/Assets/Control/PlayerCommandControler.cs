using UnityEngine;
using System.Collections;

public class PlayerCommandControler : MonoBehaviour {
	//Keyboard
	void Update(){
		if (Input.GetKey (KeyCode.Space)) {
			Debug.Log ("space");
		}
	}
}
