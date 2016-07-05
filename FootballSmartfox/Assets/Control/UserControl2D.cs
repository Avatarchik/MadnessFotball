using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
[RequireComponent(typeof (PersonCharacter2D))]
public class UserControl2D : MonoBehaviour {
	private PersonCharacter2D m_Character;
	private Transform m_Cam;
	private Vector3 m_CamForward;             // The current forward direction of the camera
	private Vector2 m_Move;
	private bool m_Jump;    
	// Use this for initialization

	private void Start()
	{
		// get the transform of the main camera
		if (Camera.main != null)
		{
			m_Cam = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning(
				"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}

		// get the third person character ( this should never be null due to require component )
		m_Character = GetComponent<PersonCharacter2D>();
	}
	// Update is called once per frame
	void Update () {
	
	}
	void FixedUpdate(){
				float h = CrossPlatformInputManager.GetAxis("Horizontal");
				float v = CrossPlatformInputManager.GetAxis("Vertical");

				//bool crouch = Input.GetKey(KeyCode.C);

		 
				// calculate move direction to pass to character
				if (m_Cam != null)
				{
					// calculate camera relative direction to move:
					//m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
					//m_Move = v*m_CamForward + h*m_Cam.right;
					//Debug.Log ("H "+h);
						//Debug.Log ("V "+v);
					m_Move = new Vector2(h,v);
				}
				else
				{
					// we use world-relative directions in the case of no main camera
					m_Move = new Vector2(h,v);
				}
				#if !MOBILE_INPUT
				// walk speed multiplier
				if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
				#endif
				m_Character.Move (m_Move);
				// pass all parameters to the character control script
				//m_Character.Move(m_Move, crouch, m_Jump);
				//m_Jump = false;
	}
}
