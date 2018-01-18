using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameSparks.Core;

public class GoogleActor : MonoBehaviour 
{
	[SerializeField]EventObject googleLoginSuccess;
	[SerializeField]EventObject googleLogout;
	[SerializeField]StringObject email;
	[SerializeField]StringObject userName;
	string key="googleLoggedIn";

	void Start()
	{
		if(PlayerPrefs.GetInt(key)==0)
		Login (false);
		if(PlayerPrefs.GetInt(key)==1)
		Login (true);	
			
	}

	public void Login(bool status=false)
	{
		Action<bool> logInCallBack = (Action<bool>)((loggedIn)=> {
			if(loggedIn)
			{
				email.value = LCGoogleLoginBridge.GSIEmail();
				userName.value = LCGoogleLoginBridge.GSIUserName();
				print("Google Login Success> " + LCGoogleLoginBridge.GSIUserName()); 
				PlayerPrefs.SetInt (key,1);
				googleLoginSuccess.Fire();
			}

			else
			{
				print("Google Login Failed");
			}	
		});
		LCGoogleLoginBridge.LoginUser (logInCallBack, status);
	}

	public void Logout()
	{
		LCGoogleLoginBridge.LogoutUser ();
		googleLogout.Fire ();
		PlayerPrefs.SetInt (key,0);
	}
}