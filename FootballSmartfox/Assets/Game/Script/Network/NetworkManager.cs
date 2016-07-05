using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;

public class NetworkManager : MonoBehaviour {
	static NetworkManager mInstance;
	public static NetworkManager GetInstance{
		get{return mInstance;}
	}
	SmartFox sfs;
	void Awake(){
		mInstance = this;
	}

	void Start() {
		sfs = SmartFoxConnection.Connection;
		if (sfs == null) {
			SceneManager.LoadScene("lobby");
			return;
		}	

		AddSmartFoxEventListener ();

		Debug.Log ("Init NetworkManager ");
		TimeManager.GetInstance.Init ();
		SendSpawnRequest();

		//TimeManager.Instance.Init();

		//running = true;
	}

	void AddSmartFoxEventListener(){
		sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
		sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserLeaveRoom);
		sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
	}

	#region Subscripe Delegate
	void OnExtensionResponse(BaseEvent e){
		string cmd = (string)e.Params["cmd"];
		ISFSObject dt = (SFSObject)e.Params["params"];
		Debug.Log ("cmd : " + cmd);
		try{
			if(cmd == "time"){
				HandleServerTime(dt);
			}
			else if(cmd == "spawnPlayer"){
				HandleInstantiatePlayer(dt);
			}
			else if (cmd == "transform") {
				HandleTransform(dt);
			}
			else if (cmd == "notransform") {
				HandleNoTransform(dt);
			}

		}
		catch(Exception error){
			Debug.Log("Exception handling response: "+error.Message+" >>> "+error.StackTrace);
		}
	}

	void OnUserLeaveRoom(BaseEvent e){

	}

	void OnConnectionLost(BaseEvent e){

	}

	#endregion


	/// <summary>
	/// Request the current server time. Used for time synchronization
	/// </summary>	
	public void TimeSyncRequest() {
		Room room = sfs.LastJoinedRoom;
		ExtensionRequest request = new ExtensionRequest("getTime", new SFSObject(), room);
		sfs.Send(request);
	}

	void FixedUpdate() {
	//	if (!running) return;
	//	smartFox.ProcessEvents();
	}


	#region HandleServer
	private void HandleServerTime(ISFSObject dt) {
		long time = dt.GetLong("t");
		TimeManager.GetInstance.Synchronize(Convert.ToDouble(time));
	}
	// Updating transform of the remote player from server
	private void HandleTransform(ISFSObject dt) {
		int userId = dt.GetInt("id");
		NetworkTransform ntransform = NetworkTransform.FromSFSObject(dt);
		if (userId != sfs.MySelf.Id) {
			// Update transform of the remote user object
			NetworkTransformReceiver recipient = PlayerManager.GetInstance.GetRecipient(userId);

			if (recipient!=null) {
				recipient.ReceiveTransform(ntransform);
			}
		}
	}

	// Server rejected transform message - force the local player object to what server said
	private void HandleNoTransform(ISFSObject dt) {
		int userId = dt.GetInt("id");
		NetworkTransform ntransform = NetworkTransform.FromSFSObject(dt);

		if (userId == sfs.MySelf.Id) {
			// Movement restricted!
			// Update transform of the local object
			ntransform.Update(PlayerManager.GetInstance.GetPlayerObject().transform);
		}
	}
	// Instantiating player (our local FPS model, or remote 3rd person model)
	private void HandleInstantiatePlayer(ISFSObject dt) {
		Debug.Log ("dt : " + dt);
		ISFSObject playerData = dt.GetSFSObject("player");
		int userId = playerData.GetInt("id");
		//int score = playerData.GetInt("score");
		Debug.Log ("userID " + userId);
		//Debug.Log ("score " + score);
		NetworkTransform ntransform = NetworkTransform.FromSFSObject(playerData);

		User user = sfs.UserManager.GetUserById(userId);

		string name = user.Name;
		Debug.Log (name);

		if (userId == sfs.MySelf.Id) {
			PlayerManager.GetInstance.SpawnPlayer(ntransform, name, 0);
		}
		else {
			PlayerManager.GetInstance.SpawnRemotePlayer(userId, ntransform, name, 0);
		}
	}
	#endregion



	#region Request Server
	/// <summary>
	/// Send local transform to the server
	/// </summary>
	/// <param name="ntransform">
	/// A <see cref="NetworkTransform"/>
	/// </param>
	public void SendTransform(NetworkTransform ntransform) {
		Room room = sfs.LastJoinedRoom;
		ISFSObject data = new SFSObject();
		ntransform.ToSFSObject(data);
		ExtensionRequest request = new ExtensionRequest("sendTransform", data, room, true); // True flag = UDP
		sfs.Send(request);
	}

	public void SendSpawnRequest() {
		Room room = sfs.LastJoinedRoom;
		ExtensionRequest request = new ExtensionRequest("spawnMe", new SFSObject(), room);
		sfs.Send(request);
	}

	#endregion
}
