using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent(typeof(NetworkGameConsole))]
public class UIGameConsole : MonoBehaviour {
	

	[SerializeField]private Button b_logout;
	[SerializeField]private Button b_disconnect;
	[SerializeField]private Text room_txt;
	[SerializeField]private Text latency_txt;
	NetworkGameConsole mNetworkGameconsole;
	void Start(){
		mNetworkGameconsole = GameObject.Find ("NetworkGameConsole").GetComponent<NetworkGameConsole> ();

		b_logout.onClick.AddListener (OnLogout);
		b_disconnect.onClick.AddListener (OnDisconnect);
	}

	void OnEnable(){
		NetworkGameConsole.OnLatencyUpdate += NetworkGameConsole_OnLatencyUpdate;
	}

	void NetworkGameConsole_OnLatencyUpdate (int latency)
	{
		latency_txt.text = "Ping : " + latency;
	}




	void OnLogout(){
		mNetworkGameconsole.Logout();
	}
	void OnDisconnect(){
		mNetworkGameconsole.Disconnect ();
	}


}
