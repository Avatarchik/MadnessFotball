using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Util;
using Sfs2X.Logging;
public class PlayerIdentity : MonoBehaviour {
	SmartFox sfs;

	void Start(){
		sfs = SmartFoxConnection.Connection;
	}
	void OnEnalbe(){
		sfs.AddLogListener(Sfs2X.Logging.LogLevel.DEBUG, OnDebugMessage);
		sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVariableUpdate);
	}

	void OnUserVariableUpdate(BaseEvent e){
		Debug.Log (e.Type);
	}


	public void OnDebugMessage(BaseEvent evt) { string message = (string)evt.Params["message"]; Debug.Log("[SFS DEBUG] " + message);}
}
