using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
public class PlayerNetwork : MonoBehaviour {
	public bool isLocal = false;
	SmartFox sfs;
	SFSUser user;
	public List<UserVariable> userVariables = new List<UserVariable>();
	public bool useHistroialLerping = false;
	public int lerpRate = 16;
	public int minLerpRate = 16;
	public int maxLerpRate = 27;
	public float threshold = 0.1f;
	public float closeEnough = 0.1f;


	float lastTime;
	float delayTime;
	private Vector2 lastPoint;
	private Vector2 syncPosition;
	public List<Vector2> moveMentList = new List<Vector2>();


	void Start(){
		lastTime = Time.time;
	}

	#region Setup
	public void SetSmartFoxClient(SmartFox smartfoxClient){
		sfs = smartfoxClient;
		AddSmartFoxListener ();
	}
	void AddSmartFoxListener(){
		sfs.AddEventListener (SFSEvent.USER_VARIABLES_UPDATE, OnUserVariableUpdate);
	}
	public void SetRemoteUser(SFSUser sfsUser){
		user = sfsUser;
	}
	#endregion

	#region Smartfox Listener Callback
	void OnUserVariableUpdate(BaseEvent e){
		SFSUser sfsUser = (SFSUser)e.Params ["user"];
		if (user.IsItMe)
			return;
		if (user == sfsUser) {
			if (sfsUser.ContainsVariable ("x") && sfsUser.ContainsVariable ("y")) {
				syncPosition = new Vector2((float)user.GetVariable("x").GetDoubleValue(),
									  (float)user.GetVariable("y").GetDoubleValue());
				delayTime = Time.time - (float)user.GetVariable("time").GetDoubleValue();
				moveMentList.Add (syncPosition);
				//transform.position = Vector2.Lerp (transform.position, newPosition, Time.deltaTime * 0.5f);
			}
		}
	}
	#endregion
	#region Movement Method
	void RemoteMovement(){
		if (useHistroialLerping) {
			HistorialMovement ();
		} else {
			//ChangeRotation (syncPosition);

			transform.position = Vector2.Lerp (transform.position, syncPosition, Time.deltaTime * delayTime);
			Vector2 vec = new Vector2(transform.position.x,transform.position.y) - syncPosition;
			ChangeRotation (vec,-1);
			lastTime = Time.time;
		}
	}
	void HistorialMovement(){
		if (moveMentList.Count > 0) {
			transform.position = Vector2.Lerp (transform.position, moveMentList[0], Time.deltaTime * delayTime);
			if (Vector2.Distance (transform.position, moveMentList [0]) <= closeEnough) {
				moveMentList.RemoveAt (0);
			}

			if (moveMentList.Count > 10) {
				lerpRate = maxLerpRate;
			} else {
				lerpRate = minLerpRate;
			}
		}
	}
	void LocalMovement(){
		if (Vector2.Distance (transform.position, lastPoint) >= threshold) {
			userVariables.Clear ();
			userVariables.Add(new SFSUserVariable("x", (double)transform.position.x));
			userVariables.Add(new SFSUserVariable("y", (double)transform.position.y));
			userVariables.Add (new SFSUserVariable ("time", (double)lastTime));
			sfs.Send(new SetUserVariablesRequest(userVariables));
			lastPoint = transform.position;
		}
	}
	public void ChangeRotation(Vector2 move,int rotate = 1){
		if (transform == null)
			return;
		if (move.magnitude > 1f)
			move.Normalize ();
		move = transform.InverseTransformVector (move);
		if (move != Vector2.zero) {
			float angle = Mathf.Atan2 (move.y, move.x) * Mathf.Rad2Deg +(90*rotate);
			transform.Rotate (Vector3.forward, angle);
		}
	}
	#endregion



	#region Update
	public void playerUpdate(){
		if (sfs != null && user.IsItMe) {
			LocalMovement ();
		} else {
			RemoteMovement ();
		}
	}
	#endregion 

	#region Return Method

	#endregion
}
