using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Core;


[RequireComponent(typeof(GoogleLogin))]
[RequireComponent(typeof(GoogleLogout))]
[RequireComponent(typeof(GSLogin))]
[RequireComponent(typeof(GSLogout))]
[RequireComponent(typeof(GSRegister))]
[RequireComponent(typeof(ChangeTexture))]
public class GoogleLoginController : Singleton<GoogleLoginController> {


	GoogleLogin googleLogin;
	GoogleLogout googleLogout;
	public GSLogin gameSparksLogin;
	GSLogout gameSparksLogout;
	GSRegister gameSparksRegistration;
	[SerializeField]ChangeTexture gameSparks;
	[SerializeField]ChangeTexture google;

	void Awake()
	{
		googleLogin = GetComponent<GoogleLogin> ();
		googleLogout = GetComponent<GoogleLogout> ();
		gameSparksLogin = GetComponent<GSLogin> ();
		gameSparksLogout = GetComponent<GSLogout> ();
		gameSparksRegistration = GetComponent<GSRegister> ();
		//gameSparks = GetComponent<ChangeTexture> ();
	}


	void Start()
	{
	//	if(PlayerPrefs.GetInt ("isGoogle")!=1)
			googleLogin.Login ();
	}

	void OnEnable()
	{
		EventManager.instance.googleLoginSuccess += GoogleLoginSuccess;
		EventManager.instance.googleLoginFailed += GoogleLoginFailed;
		EventManager.instance.gamesparksLoginSuccess += GSLoginSuccess;
		EventManager.instance.gamesparksLoginFailed += GSLoginFailed;
		EventManager.instance.gamesparksRegistrationSuccess += GSRegistrationSuccess;
		EventManager.instance.gamesparksRegistrationFailed += GSRegistrationFailed;
		EventManager.instance.gamesparksLogoutSuccess += GSLogoutSuccess;
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
			EventManager.instance.gamesparksLogoutSuccess -= GSLogoutSuccess;
		}
	}

	public void Login()
	{	
		#if UNITY_EDITOR
		gameSparksLogin.Login ("akil.hotshot@gmail.com", "akil.hotshot@gmail.com");
		#else
		googleLogin.Login ();
		#endif				
	}


	public void Logout()
	{
		#if UNITY_EDITOR
		gameSparksLogout.Logout();
		#else
		googleLogout.Logout ();
		#endif
	}

	void GoogleLoginSuccess()
	{		
		gameSparksLogin.Login (LCGoogleLoginBridge.GSIEmail (),LCGoogleLoginBridge.GSIEmail ());
		ObliusGameManager.instance.StartTutorial ();
		google.ShowType1 ();
	}

	void GoogleLoginFailed()
	{
		print ("Google login failed!");
		GUIManager.instance.ShowLog ("Google login failed!");
		Application.LoadLevel (0);
		google.ShowType2 ();
	}

	void GSLoginSuccess()
	{
		//GUIManager.instance.ShowLog ("Login Success !!");
		FacebookLoginController.instance.GetGameSparksFriends ();
		google.ShowType1 ();
		gameSparks.ShowType1 ();
		ObliusGameManager.instance.StartTutorial ();
		print ("Gamesparks login success!");
	}

	void GSLogoutSuccess()
	{
		print ("Gamesparks logout success!");
		google.ShowType2 ();
		gameSparks.ShowType2 ();
	}

	void GSLoginFailed(GSErrorData error)
	{
		print ("Gamesparks login failed!");
		if(error.error!=GSErrorCodes.timeout)
			gameSparksRegistration.Register (LCGoogleLoginBridge.GSIUserName (), LCGoogleLoginBridge.GSIEmail (), LCGoogleLoginBridge.GSIEmail ());
		else
			GUIManager.instance.ShowLog (error.error);
		gameSparks.ShowType2 ();
	}

	void GSRegistrationSuccess()
	{
		print ("Gamesparks registration success!");
		GoogleLoginSuccess ();
	}

	void GSRegistrationFailed(GSErrorData error)
	{
		print ("Gamesparks registration failed!");
		GUIManager.instance.ShowLog ("User registration failed!");
	}
}
