using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameSparks.Core;
using UnityEngine.SocialPlatforms;
public class GoogleActor : MonoBehaviour 
{
	[SerializeField]EventObject googleLoginSuccess;
	[SerializeField]EventObject googleLogout;
	[SerializeField]StringObject email;
	[SerializeField]StringObject userName;
	string key="googleLoggedIn";

	void Awake()
	{
		//userName.Reset ();
		//email.Reset ();
		#if UNITY_IOS
		//GameCenterLogin ();

		if (PlayerPrefs.HasKey ("GameCenterID")) {
		email.value = PlayerPrefs.GetString ("GameCenterID");
		googleLoginSuccess.Fire();
		}
		else
		GameCenterLogin ();


		#else
		if(PlayerPrefs.GetInt(key)==0)
			Login (false);
		if (PlayerPrefs.GetInt(key) == 1)
			Login(true);
		#endif

	
		

	}

	void GameCenterLogin()
    {
		Social.localUser.Authenticate (success => 
        {
			if (success)
			{
				PlayerPrefs.SetString ("GameCenterID",Social.localUser.id);
				email.value = Social.localUser.id;
				userName.value = Social.localUser.userName;
				googleLoginSuccess.Fire();
            }
			else
				Debug.Log("Failed to authenticate");
		});


	}

	public void Login(bool status)
	{
		Action<bool> logInCallBack = (Action<bool>)((loggedIn)=> {
			if(loggedIn)
            {
                email.value = LCGoogleLoginBridge.GSIEmail();                
                userName.value = LCGoogleLoginBridge.GSIUserName();
                PlayerPrefs.SetInt(key, 1);
                googleLoginSuccess.Fire();               
            }

            else
			{
				print("Google Login Failed");
				Application.Quit ();
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