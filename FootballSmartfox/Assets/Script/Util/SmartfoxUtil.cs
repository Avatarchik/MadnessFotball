using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Util;
using Sfs2X.Core;
public class SmartfoxUtil : MonoBehaviour {
	public static void CheckParam(IDictionary param){
		foreach(DictionaryEntry key in param)
		{
			Debug.Log (string.Format ("Key : {0} , Value : {1}", key.Key, key.Value));
		}
	}
}
