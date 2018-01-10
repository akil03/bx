using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FBLogin : MonoBehaviour {


	public FbLoginData data;

	public void Login()
	{		
		FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" },LoginCallback);
	}

	void LoginCallback(ILoginResult result)
	{
		if (string.IsNullOrEmpty (result.Error)) 
		{
			print ("Facebook login success!");
			print (result.RawResult);
			data = JsonUtility.FromJson<FbLoginData> (result.RawResult);
			EventManager.instance.OnFbLoginSuccess ();
		}
		else
		{
			print (result.Error);
			GUIManager.instance.ShowLog (result.Error);
		}
	}
}
