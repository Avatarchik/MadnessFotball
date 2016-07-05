using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities;
//NetworkGameConsole
//ใช้คู่กับ UIGameConsole
//ใช้สำหรับ เทสหน้าเกม

// Networkconsole ใช้สำหรับ หน้าเกม 

public class NetworkGameConsole : MonoBehaviour {
	public delegate void LatencyUpdate(int latency);
	public static event LatencyUpdate OnLatencyUpdate;


	public SceneField mLobbyScene;
	SmartFox sfs;
	Room room;

	int latency;
	//Prooerty
	int Latency{
		get{return latency;}
		set{ latency = value;
			OnLatencyUpdate (latency);
			}
	}

	void Start(){
		SmartfoxSetup ();
	}
	void SmartfoxSetup(){
		if (sfs == null) {
			sfs = SmartFoxConnection.Connection;
			AddSmartfoxListener ();
		} else {
			// สำหรับการ หลุดจากหน้าเกม
			AddSmartfoxListener ();
		}
		//sfs.EnableLagMonitor (true, 30);
	}

	void AddSmartfoxListener(){
		sfs.AddEventListener (SFSEvent.LOGOUT, OnLogoutCallback);
		sfs.AddEventListener (SFSEvent.CONNECTION_LOST, OnConnectionLost);
		sfs.AddEventListener (SFSEvent.PING_PONG, OnPing);
	}


	


	#region public Method
	public void Logout(){
		sfs.Send (new LogoutRequest ());
	}
	public void Disconnect(){
		sfs.Disconnect ();
	}
	#endregion
	#region public property
	public string RoomName{
		get{ return "aaa"; }
	}
	#endregion

	#region Smartfox Callback
	void OnLogoutCallback(BaseEvent e){
		Debug.Log ("logoutcomplete");
		Reset ();
		SceneManager.LoadScene (mLobbyScene.SceneName);
	}
	void OnConnectionLost(BaseEvent e){
		Reset ();
		SceneManager.LoadScene (mLobbyScene.SceneName);
	}
	void OnPing(BaseEvent e){
		Latency = (int)e.Params ["lagValue"];
	}
	#endregion


	#region Private Method
	void Reset(){
		sfs.RemoveAllEventListeners ();
		sfs = null;
	}
	#endregion

}
