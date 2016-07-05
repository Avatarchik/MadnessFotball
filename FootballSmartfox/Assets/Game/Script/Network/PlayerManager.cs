using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {
	public GameObject enemyPrefab;
	public GameObject playerPrefab;

	private GameObject playerObj;

	private static PlayerManager mInstance;
	public static PlayerManager GetInstance {
		get {
			return mInstance;
		}
	}
		
	public GameObject GetPlayerObject(){
		return playerObj;
	}

	private Dictionary<int, NetworkTransformReceiver> recipients = new Dictionary<int, NetworkTransformReceiver>();
	private Dictionary<int, GameObject> items = new Dictionary<int, GameObject>();

	void Awake(){
		mInstance = this;
	}

	public void SpawnPlayer(NetworkTransform ntransform, string name, int score) {
		Debug.Log ("SpawnPlayer");
		if (Camera.main!=null) {
			//Destroy(Camera.main.gameObject);
		}

//		GameHUD.Instance.UpdateHealth(100);
		playerObj = GameObject.Instantiate(playerPrefab) as GameObject;
		playerObj.transform.position = ntransform.Position;
		playerObj.transform.localEulerAngles = ntransform.AngleRotationFPS;
		playerObj.SendMessage("StartSendTransform");

	//	PlayerScore.Instance.SetScore(name, score);
	}
	public void SpawnRemotePlayer(int id, NetworkTransform ntransform, string name, int score) {
		GameObject playerObj = GameObject.Instantiate(enemyPrefab) as GameObject;
		playerObj.transform.position = ntransform.Position;
		playerObj.transform.localEulerAngles = ntransform.AngleRotationFPS;



		//AnimationSynchronizer animator = playerObj.GetComponent<AnimationSynchronizer>();
		//animator.StartReceivingAnimation();

		//PlayerScore.Instance.SetScore(name, score);

		RemotePlayer remotePlayer = playerObj.GetComponent<RemotePlayer>();
		remotePlayer.Init(name);

		recipients[id] = playerObj.GetComponent<NetworkTransformReceiver>();
	}
	public NetworkTransformReceiver GetRecipient(int id){
		if (recipients.ContainsKey(id)) {
			return recipients[id];
		}
		return null;
	}
	public void DestroyRemotePlayer(int id){

	}
}
