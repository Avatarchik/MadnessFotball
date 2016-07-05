using UnityEngine;
using System.Collections;

public class NetworkTransformReceiver : MonoBehaviour {
	Transform thisTransform;
	private NetworkTransformInterpolation interpolater;
	void Awake(){
		thisTransform = this.transform;
	
		interpolater = GetComponent<NetworkTransformInterpolation>();
		if (interpolater!=null) {
			interpolater.StartReceiving();
		}
	}
	public void ReceiveTransform(NetworkTransform nTransform){
		if (interpolater != null) {
			interpolater.ReceivedTransform (nTransform);
		} else {
			//No interpolation - updating transform directly
			thisTransform.position = nTransform.Position;
			// Ignoring x and z rotation angles
			thisTransform.localEulerAngles = nTransform.AngleRotationFPS;
		}

	}

}
