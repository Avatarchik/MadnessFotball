using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


// file ที่เกี่ยวข้อง InputEvents
public class MobileStickController : MonoBehaviour {


	// Create a class that listens to the list's change event.


	// Add and remove items from the list.

	//public static event InputEvent();

	// Get ค่าปุ่มที่บันทึกเอาไว้ใน CrossplatformManager
	void Update(){
		
		if (CrossPlatformInputManager.GetButtonDown ("Kick")) {
		//	Debug.Log ("kick");
			//Game.Events.InputEvent.InputFire ();
		}
		if (CrossPlatformInputManager.GetButtonUp ("Kick")) {
		//	Debug.Log ("Kickup");
		}

	}

	void Start(){
		//Test EventList
		/*ListWithChangedEvent list = new ListWithChangedEvent();
		EventListener listener = new EventListener(list);
		list.Add("item 1");
		list.Clear();
		listener.Detach();*/
	}

}
