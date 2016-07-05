using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Util;
using Sfs2X.Bitswarm;
public class NetworkLobbyConsole : MonoBehaviour {
	//[SerializeField]GameObject playerPrefab;
	private const string EXTENSION_ID = "Football";
	private const string EXTENSION_CLASS = "com.football.FootballExtension";
	SmartFox sfs = null;

	//public
	public SceneField myScene;
	//private
	private RoomSettings roomSetting;

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
	}
	void AddSmartfoxListener(){
		sfs.AddEventListener (SFSEvent.CONNECTION, OnConnection);
		sfs.AddEventListener (SFSEvent.CONNECTION_LOST, OnConnectionLost);
		sfs.AddEventListener (SFSEvent.CONNECTION_RETRY,OnConnectionRetry);
		sfs.AddEventListener (SFSEvent.CONNECTION_RESUME,OnConnectionResume);
		sfs.AddEventListener (SFSEvent.CONFIG_LOAD_SUCCESS, OnConfigLoad);
		sfs.AddEventListener (SFSEvent.UDP_INIT, OnUDPInit);
		sfs.AddEventListener (SFSEvent.LOGIN,OnLogin);
		sfs.AddEventListener (SFSEvent.ROOM_ADD, OnRoomAdd);
		sfs.AddEventListener (SFSEvent.ROOM_JOIN, OnRoomJoin);
	}

	#region public Method
	public void Connect(ConfigData data){
		Debug.Log ("now connection Zone");
		sfs.ThreadSafeMode = true;
		Debug.Log ("IP : " + data.Host);
		Debug.Log ("Port : " + data.Port);
		sfs.Connect(data);
	}
	#endregion 

	#region public Request Method
	public void Request_CreateRoom(){
		sfs.Send (new CreateRoomRequest (GetRoomSetting(),true));
	}
	public void Login(string name){
		sfs.Send (new LoginRequest (name));
	}
	public void Disconnect(){
		sfs.Disconnect ();
	}
	public void KillConnection(){
		sfs.KillConnection ();
	}
	public void QiuckMatch(){
		Debug.Log ("QuickMatch");
		List<Room> rooms = sfs.RoomList;
		Debug.Log (rooms.Count);
		if (rooms.Count > 0) {
			sfs.Send (new JoinRoomRequest (rooms[0].Id));
		}
	}

	#endregion

	#region Smartfox Listener Callback
	void OnConnection(BaseEvent e){
		sfs.RemoveEventListener (SFSEvent.CONNECTION, OnConnection);
		//AddSmartfoxListener ();
		Debug.Log ("Connection complete");
	}
	void OnUDPInit(BaseEvent e){
		Debug.Log ("UDP INIT");
	}
	void OnConnectionLost(BaseEvent e){
		Debug.Log ("Connection lost");
	}
	void OnConnectionRetry(BaseEvent e){
		Debug.Log ("OnConnectionRetry ");
		SmartfoxUtil.CheckParam (e.Params);
	}
	void OnConnectionResume(BaseEvent e){
		Debug.Log ("OnConnectionResume");
	}
	void OnConfigLoad(BaseEvent e){
		Debug.Log ("Config Load Complete");
	}
	void OnLogin(BaseEvent e){
		Debug.Log ("Login");
		Debug.Log ("init DUP NOW");
		sfs.InitUDP ();
		//SmartfoxUtil.CheckParam(e.Params);
	}
	void OnRoomAdd(BaseEvent e){
	}
	void OnRoomJoin(BaseEvent e){
		Debug.Log ("Onroomjoin");
		Reset ();
		SceneManager.LoadScene (myScene.SceneName);
	}
	#endregion


	#region Private Method
	void Reset(){
		sfs.RemoveAllEventListeners ();
	}
	#endregion


	#region return Method
	RoomSettings GetRoomSetting(){
		roomSetting = new RoomSettings ("TestRoom");
		roomSetting.Extension = new RoomExtension (EXTENSION_ID, EXTENSION_CLASS);
		roomSetting.IsGame = true;
		Debug.Log (roomSetting.Extension);
		return roomSetting;
	}

	public bool IsConnect{
		get{ return sfs.IsConnected;}
	}
	#endregion

}
