using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(TimeManager))]
public class PingEditor : Editor {

	public override void OnInspectorGUI ()
	{
		TimeManager t = (TimeManager)target;
		EditorGUILayout.DoubleField ("Ping",t.AveragePing);
	}
}
