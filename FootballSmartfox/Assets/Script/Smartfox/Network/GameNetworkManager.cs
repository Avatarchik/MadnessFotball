using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;

public class GameNetworkManager : MonoBehaviour {
	public GameObject playerPrefab;
	SmartFox sfs = null;

	Dictionary<SFSUser,GameObject> remotePlayers = new Dictionary<SFSUser, GameObject>();
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
		CreateLocalPlayer (sfs.MySelf);

	}

	void AddSmartfoxListener(){
		sfs.AddEventListener (SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
		sfs.AddEventListener (SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
		sfs.AddEventListener (SFSEvent.USER_VARIABLES_UPDATE, OnUservariableUpdate);
	}

	#region Listener Callback
	void OnUserEnterRoom(BaseEvent e){
		Debug.Log ("User Enter Room");
		SmartfoxUtil.CheckParam (e.Params);
		SFSUser user = (SFSUser)e.Params ["user"];
		CreateRemotePlayer (user,Vector3.zero);
	}
	void OnUserExitRoom(BaseEvent e){
		Debug.Log ("OnuserExitRoom");
		SFSUser user = (SFSUser)e.Params["user"];
		Debug.Log ("User " + user.Name);
		if (remotePlayers.ContainsKey (user)) {
			Destroy (remotePlayers [user] as GameObject);
			remotePlayers.Remove (user);
		}
	}
	void OnUservariableUpdate(BaseEvent e){
		#if UNITY_WSA && !UNITY_EDITOR
		List<string> changedVars = (List<string>)evt.Params["changedVars"];
		#else
		ArrayList changedVars = (ArrayList)e.Params["changedVars"];
		#endif
		SFSUser user = (SFSUser)e.Params["user"];
		if (user == sfs.MySelf) return;

		if (!remotePlayers.ContainsKey(user)) {
			// New client just started transmitting - lets create remote player
			Vector3 pos = new Vector3(0, 1, 0);
			if (user.ContainsVariable("x") && user.ContainsVariable("y")) {
				pos.x = (float)user.GetVariable("x").GetDoubleValue();
				pos.y = (float)user.GetVariable("y").GetDoubleValue();
			}
			CreateRemotePlayer (user,pos);
		}
	}

	#endregion

	#region Private Method
	void CreateLocalPlayer(User user){
		GameObject player = Instantiate (playerPrefab) as GameObject;
		player.GetComponent<PlayerSetup> ().Setup (sfs.UserManager.SmartFoxClient,(SFSUser)user);
		remotePlayers.Add ((SFSUser)user, player);
	}
	void CreateRemotePlayer(SFSUser user,Vector3 vector){
		GameObject player = Instantiate (playerPrefab) as GameObject;
		player.transform.position = vector;
		remotePlayers.Add (user, player);
		player.GetComponent<PlayerSetup> ().Setup (sfs.UserManager.SmartFoxClient,user);
	}
	#endregion


	#region Update
	void FixedUpdate(){
		foreach (KeyValuePair<SFSUser,GameObject> o in remotePlayers) {
			o.Value.GetComponent<PlayerNetwork> ().playerUpdate();
		}
	}
	#endregion
}
