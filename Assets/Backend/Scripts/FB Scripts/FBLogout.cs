using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using GameSparks.Core;

public class FBLogout : MonoBehaviour {

	public void Logout()
	{
		FB.LogOut ();
		EventManager.instance.OnFbLogout ();
		print ("FB logged out!");
		PlayerPrefs.SetInt ("isFB", 0);
	}
}