using UnityEngine;
using System.Collections;

public class NetworkTransformSender : MonoBehaviour {
	// We will send transform each 0.1 second. To make transform synchronization smoother consider writing interpolation algorithm instead of making smaller period.
	public static readonly float sendingPeriod = 0.001f; 
	private float timeLastSending = 0.0f;

	private bool send = false;
	private NetworkTransform lastState;

	private Transform thisTransform;

	void Start(){
		thisTransform = this.transform;
		lastState = NetworkTransform.FromTransform (thisTransform);
	}
	void StartSendTransform() {
		send = true;
	}

	void FixedUpdate() { 
		if (send) {
			SendTransform();
		}
	}
	void SendTransform() {
		//if (lastState.IsDifferent(thisTransform, accuracy)) {
		if (timeLastSending >= sendingPeriod) {
			lastState = NetworkTransform.FromTransform(thisTransform);
			NetworkManager.GetInstance.SendTransform(lastState);
			timeLastSending = 0;
			return;
		}
		//}
		timeLastSending += Time.deltaTime;
	}
}
