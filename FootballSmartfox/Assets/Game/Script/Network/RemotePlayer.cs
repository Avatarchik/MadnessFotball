using UnityEngine;
using System.Collections;

public class RemotePlayer : MonoBehaviour {

	public RemotePlayerInfo info;
	private bool showingInfo = false;

	private float timeSinceLastRaycast = 0;
	private readonly float showInfoTime = 0.5f;
	public void Init(string name) {
		info.SetName(name);
		info.Hide();
		showingInfo = false;
	}
	public void ShowInfo(){

	}
	public void HideInfo(){

	}
	void RaycastMessage() {
		timeSinceLastRaycast = 0;
	}

	void Update() {
		timeSinceLastRaycast += Time.deltaTime;
		/*if (timeSinceLastRaycast < showInfoTime) {
			ShowInfo();
		}
		else {
			HideInfo();
		}*/
	}
}
