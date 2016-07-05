using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using System.Collections;
using System.Collections.Generic;
/**
 * Singleton class with static fields to hold a reference to SmartFoxServer connection.
 * It is useful to access the SmartFox class from anywhere in the game.
 */
public class SmartFoxConnection : MonoBehaviour
{
	[SerializeField]public static SmartFoxConnection mInstance; 
	[SerializeField]public static SmartFox sfs;
	public bool isConnect;
	public bool isPause = false;
	public bool isRunning =false;
	public static SmartFox Connection {
		get {
			if (mInstance == null) {
				mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
				DontDestroyOnLoad (mInstance);
				mInstance.CheckInitialize ();
			}
			return sfs;
		}
		set {
			if (mInstance == null) {
				mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
			}
			sfs = value;
		} 
	}
	void CheckInitialize(){
		if (sfs == null || !sfs.IsConnected) {
			sfs = new SmartFox ();
			isRunning = true;
		}
	}
	public static bool IsInitialized {
		get { 
			return (sfs != null); 
		}

	}
	#region Update
	void FixedUpdate(){

		if (!isRunning)
			return;
		if (sfs != null) {
			if (!sfs.IsConnected) {
				Debug.Log ("SFS is Disconnect");
				sfs.Disconnect ();
			}
			sfs.ProcessEvents ();
		} else {
			Debug.Log ("SFS is Null");
		}
	}
	#endregion
	// Handle disconnection automagically
	// ** Important for Windows users - can cause crashes otherwise

	void OnApplicationQuit() { 
		
	}
}