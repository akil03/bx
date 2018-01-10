using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Core;

public class GSLoginWithGoogle : MonoBehaviour {

	public GoogleLogin googleLogin;
	public GSLogin gameSparksLogin; 
	public GSRegister gameSparksRegistration;

	void Start()
	{
		
	}


	void OnEnable()
	{
		EventManager.instance.googleLoginSuccess += GoogleLoginSuccess;
		EventManager.instance.googleLoginFailed += GoogleLoginFailed;
		EventManager.instance.gamesparksLoginSuccess += GSLoginSuccess;
		EventManager.instance.gamesparksLoginFailed += GSLoginFailed;
		EventManager.instance.gamesparksRegistrationSuccess += GSRegistrationSuccess;
		EventManager.instance.gamesparksRegistrationFailed += GSRegistrationFailed;
	}

	void OnDisable()
	{
		if (EventManager.instance != null) {
			EventManager.instance.googleLoginSuccess -= GoogleLoginSuccess;
			EventManager.instance.googleLoginFailed -= GoogleLoginFailed;
			EventManager.instance.gamesparksLoginSuccess -= GSLoginSuccess;
			EventManager.instance.gamesparksLoginFailed -= GSLoginFailed;
			EventManager.instance.gamesparksRegistrationSuccess -= GSRegistrationSuccess;
			EventManager.instance.gamesparksRegistrationFailed -= GSRegistrationFailed;
		}
	}

	public void Login()
	{		
		googleLogin.Login ();
	}

	void GoogleLoginSuccess()
	{		
		gameSparksLogin.Login (LCGoogleLoginBridge.GSIEmail (),LCGoogleLoginBridge.GSIEmail ());
	}

	void GoogleLoginFailed()
	{
		print ("Google login failed!");
	}

	void GSLoginSuccess()
	{
		print ("Gamesparks login success!");
	}

	void GSLoginFailed(GSErrorData error)
	{
		print ("Gamesparks login failed!");
		if(error.error==GSErrorCodes.UNRECOGNISED)
			gameSparksRegistration.Register (LCGoogleLoginBridge.GSIUserName (), LCGoogleLoginBridge.GSIEmail (),LCGoogleLoginBridge.GSIEmail ());		
	}

	void GSRegistrationSuccess()
	{
		print ("Gamesparks registration success!");
		GoogleLoginSuccess ();
	}

	void GSRegistrationFailed(GSErrorData error)
	{
		print ("Gamesparks registration failed!");
	}
}
