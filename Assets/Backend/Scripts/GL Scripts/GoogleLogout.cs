using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Core;

public class GoogleLogout : MonoBehaviour {

	public void Logout(){
		LCGoogleLoginBridge.LogoutUser ();
		print("Google logged out!");
		GS.Reset ();
		GS.Disconnect ();
		EventManager.instance.OnGoogleLogout ();
		PlayerPrefs.SetInt ("isGoogle",0);
	}
}