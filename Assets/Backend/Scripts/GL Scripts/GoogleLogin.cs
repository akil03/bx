using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoogleLogin : MonoBehaviour {

	public void Login(bool status=false)
	{
		Action<bool> logInCallBack = (Action<bool>)((loggedIn)=> {
			if(loggedIn){
				print("Google Login Success> " + LCGoogleLoginBridge.GSIUserName()); 
				EventManager.instance.OnGoogleLoginSuccess ();
				PlayerPrefs.SetInt ("isGoogle",1);
			}

			else{
				print("Google Login Failed");
				EventManager.instance.OnGoogleLoginFailed();
			}	
		});


		LCGoogleLoginBridge.LoginUser (logInCallBack, status);
	}

}


