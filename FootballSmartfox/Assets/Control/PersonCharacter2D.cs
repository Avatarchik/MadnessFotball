using UnityEngine;
using System.Collections;

public class PersonCharacter2D : MonoBehaviour {

	// Use this for initialization
	private float speed = 5f;
//	Vector2 lastPoint = Vector2.zero;
	Transform myTransform;
	void Start () {
		myTransform = gameObject.transform.parent;
	}
	
	public void Move(Vector2 move,int rotate = 1){
		if (myTransform == null)
			return;
		Vector2 mm = move;
		if (move.magnitude > 1f)
		move.Normalize ();
		move = myTransform.InverseTransformVector (move);
		if (move != Vector2.zero) {
			float angle = Mathf.Atan2 (move.y, move.x) * Mathf.Rad2Deg +(90*rotate);
			myTransform.Rotate (Vector3.forward, angle);
		}
		myTransform.position = myTransform.position + new Vector3 (mm.x*speed*Time.deltaTime, mm.y*speed*Time.deltaTime, 0);

	}

}
