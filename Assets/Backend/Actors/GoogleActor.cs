using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameSparks.Core;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using EasyMobile;

public class GoogleActor : MonoBehaviour 
{
	[SerializeField]EventObject googleLoginSuccess;
	[SerializeField]EventObject googleLogout;
	[SerializeField]StringObject email;
	[SerializeField]StringObject userName;
	string key="googleLoggedIn";

	void Awake()
	{

        if (!Application.isEditor)
        {
            if (Application.platform == RuntimePlatform.Android)
                StartCoroutine(GooglePlayLogin());
            else
                SocialLogin();
        }

    }

    IEnumerator GooglePlayLogin()
    {
        if (!GameServices.IsInitialized())
        {
            GameServices.Init();
        }
        while (!GameServices.IsInitialized())
            yield return null;

        SocialLogin();
    }

    void GameCenterLogin()
    {


    }


	void SocialLogin()
	{
		Social.localUser.Authenticate (success => 
		{
			if (success)
			{
				PlayerPrefs.SetString ("SocialID",Social.localUser.id);
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
			   // email.value = LCGoogleLoginBridge.GSIEmail();                
			   // userName.value = LCGoogleLoginBridge.GSIUserName();
				PlayerPrefs.SetInt(key, 1);
				googleLoginSuccess.Fire();               
			}

			else
			{
				print("Google Login Failed");
				Application.Quit ();
			}	
		});
		//LCGoogleLoginBridge.LoginUser (logInCallBack, status);
	}
   

	public void Logout()
	{
		//LCGoogleLoginBridge.LogoutUser ();
		googleLogout.Fire ();
		PlayerPrefs.SetInt (key,0);
	}
}