using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
[RequireComponent(typeof(UserControl2D))]
[RequireComponent(typeof(MobileStickController))]
public class PlayerSetup : MonoBehaviour {
	[SerializeField]private UserControl2D userControllder2D;
	[SerializeField]private MobileStickController mobileStickController;
	public void Setup(SmartFox client , SFSUser user){

		if (!user.IsItMe) {
			mobileStickController.enabled = false;
			userControllder2D.enabled = false;

		}
	}
}
