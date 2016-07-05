using UnityEngine;
using System;
using System.Collections;
using Sfs2X.Entities.Data;
public class NetworkTransform {

	private Vector3 position;
	private Vector3 angleRotation;
	private double timeStamp;
	private Vector3 lastPosition;


	public Vector3 Position {
		get {
			return position;
		}
	}
	public Vector3 AngleRotation {
		get {
			return angleRotation;
		}
	}
	public double TimeStamp {
		get {
			return timeStamp;
		}
		set {
			timeStamp = value;
		}
	}
	public Vector3 AngleRotationFPS {
		get {
			return new Vector3(0, angleRotation.y, 0);
		}
	}

	public Quaternion Rotation {
		get {
			return Quaternion.Euler(AngleRotationFPS);
		}
	}


	// Creating NetworkTransform from Unity transform
	public static NetworkTransform FromTransform(Transform transform) {
		NetworkTransform trans = new NetworkTransform();

		trans.position = transform.position;
		trans.angleRotation = transform.localEulerAngles;

		return trans;
	}

	// Stores the transform values to SFSObject to send them to server
	public void ToSFSObject(ISFSObject data) {
		ISFSObject tr = new SFSObject();

		tr.PutDouble("x", Convert.ToDouble(this.position.x));
		tr.PutDouble("y", Convert.ToDouble(this.position.y));
		tr.PutDouble("z", Convert.ToDouble(this.position.z));

		tr.PutDouble("rx", Convert.ToDouble(this.angleRotation.x));
		tr.PutDouble("ry", Convert.ToDouble(this.angleRotation.y));
		tr.PutDouble("rz", Convert.ToDouble(this.angleRotation.z));

		tr.PutLong("t", Convert.ToInt64(this.timeStamp));

		data.PutSFSObject("transform", tr);
	}

	public static NetworkTransform FromSFSObject(ISFSObject data){
		NetworkTransform trans = new NetworkTransform();
		ISFSObject transformData = data.GetSFSObject("transform");

		float x = Convert.ToSingle(transformData.GetDouble("x"));
		float y = Convert.ToSingle(transformData.GetDouble("y"));
		float z = Convert.ToSingle(transformData.GetDouble("z"));

		float rx = Convert.ToSingle(transformData.GetDouble("rx"));
		float ry = Convert.ToSingle(transformData.GetDouble("ry"));
		float rz = Convert.ToSingle(transformData.GetDouble("rz"));

		trans.position = new Vector3(x, y, z);
		trans.angleRotation = new Vector3(rx, ry, rz);

		if (transformData.ContainsKey("t")) {
			trans.TimeStamp = Convert.ToDouble(transformData.GetLong("t"));
		}
		else {
			trans.TimeStamp = 0;
		}

		return trans;
	}

	// Copies another NetworkTransform to itself
	public void Load(NetworkTransform ntransform) {
		this.position = ntransform.position;
		this.angleRotation = ntransform.angleRotation;
		this.timeStamp = ntransform.timeStamp;
	}

	public void Update(Transform trans) {
		trans.position = this.Position;
		trans.localEulerAngles = this.AngleRotation;
	}
}
