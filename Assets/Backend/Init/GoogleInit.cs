using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleInit : MonoBehaviour {

	public string key = "931749422625-lh84dbo0adnq18ht01qmm5mfjn88jdqf.apps.googleusercontent.com";

	void Awake()
	{
		Initialize ();
	}

	void Initialize()
	{
		LCGoogleLoginBridge.ChangeLoggingLevel(true);
		LCGoogleLoginBridge.InitWithClientID (key);
		print("Google Login Initialized");
	}
}