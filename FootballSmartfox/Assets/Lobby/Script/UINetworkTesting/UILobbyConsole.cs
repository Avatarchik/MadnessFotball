using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Util;
using Sfs2X.Requests;
using UnityEngine.SceneManagement;
public class UILobbyConsole : MonoBehaviour {

	[SerializeField]private Button b_connect;
	[SerializeField]private Button b_create;
	[SerializeField]private Button b_join;
	[SerializeField]private Button b_login;
	[SerializeField]private Button b_disconnect;
	[SerializeField]private Button b_killConnection;
	[SerializeField]private Button b_quickMatch;

	[SerializeField]InputField input_ipAddress;
	[SerializeField]InputField input_userName;
	Smartfox_NetworkSetting sfsSetting;
	NetworkLobbyConsole m_NetworkLobby;
	Sprite sp;
	void Start () {
		m_NetworkLobby = GameObject.Find ("NetworkLobby").GetComponent<NetworkLobbyConsole> ();
		sfsSetting = GameObject.Find ("NetworkSetting").GetComponent<Smartfox_NetworkSetting> ();

		input_ipAddress.text = sfsSetting.m_IpAddress;
		b_connect.onClick.AddListener (Connect);
		b_login.onClick.AddListener (Click_Login);
		b_create.onClick.AddListener (OnCreateRoom);
		b_disconnect.onClick.AddListener (OnDisconnect);
		b_killConnection.onClick.AddListener (OnKillConnection);
		b_quickMatch.onClick.AddListener (OnQuickMatch);

	}


	void OnSmartfoxInit(){

	}
	void OnConnectionLost(BaseEvent e){
		Debug.Log ("OnConnectionLost");
	}


	void OnCreateRoom(){
		m_NetworkLobby.Request_CreateRoom ();
	}

	void Connect(){
		
		ConfigData cfg = new ConfigData();
		cfg.Host = input_ipAddress.text;
		cfg.Port = System.Convert.ToInt32(sfsSetting.m_Port);
		cfg.Zone = sfsSetting.m_zone;
		cfg.Debug = true;

		m_NetworkLobby.Connect (cfg);
	
	}
	void OnConnection(BaseEvent e){
		
	}
	void OnDisconnect(){
		m_NetworkLobby.Disconnect ();
	}
	void OnKillConnection(){
		m_NetworkLobby.KillConnection ();
	}
	void Click_Login(){
		if (m_NetworkLobby.IsConnect) {
			m_NetworkLobby.Login (input_userName.text);
		}
	}
	void OnQuickMatch(){
		if (m_NetworkLobby.IsConnect) {
			m_NetworkLobby.QiuckMatch ();
		}
	}

}
