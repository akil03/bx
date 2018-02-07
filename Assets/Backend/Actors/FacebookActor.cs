using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookActor : MonoBehaviour 
{
	[SerializeField]EventObject fbLoginSuccess;
	[SerializeField]EventObject fbLogout;
	[SerializeField]EventObject gotFBFriends;
	[SerializeField]FbFriendsObject fbFriends;
	string key="fbLoggedIn";

	void OnEnable()
	{
		fbFriends.Reset ();
	}

	void OnDisable()
	{
		fbFriends.Reset ();
	}

	public void CanLogin()
	{
		if (PlayerPrefs.GetInt (key) == 1)
			fbLoginSuccess.Fire ();
	}

	public void Login()
	{
		FB.LogInWithReadPermissions (new List<string> () { "public_profile", "email", "user_friends" }, LoginCallback);
	}

	void LoginCallback(ILoginResult result)
	{
		if (string.IsNullOrEmpty (result.Error)) 
		{
			print (result.RawResult);
			if (!result.RawResult.Contains ("cancelled")) 
			{
				fbLoginSuccess.Fire ();
				PlayerPrefs.SetInt (key,1);
			}
		}
	}

	public void Logout()
	{
		FB.LogOut ();
		fbLogout.Fire ();
		fbFriends.Reset ();
		PlayerPrefs.SetInt (key,0);
		print ("FB logged out!");
	}

	public void GetFriends()
	{
		FB.API ("me/friends",HttpMethod.GET,GetFriendsCallback);
	}

	void GetFriendsCallback(IGraphResult result)
	{
		fbFriends.value = JsonUtility.FromJson<FBFriendsData> (result.RawResult);
		gotFBFriends.Fire ();
	}
}