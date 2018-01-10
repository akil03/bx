using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using GameSparks.Core;

public class Events : MonoBehaviour {

	IEnumerator Start()
	{
		print ("fb login "+FB.IsLoggedIn+". GS login "+GS.Authenticated);
		if (FB.IsLoggedIn)
			EventManager.instance.OnFbLoginSuccess ();
		if (GS.Authenticated)
			EventManager.instance.OnGSLoginSuccess ();
		#if UNITY_EDITOR
		else
			GoogleLoginController.instance.gameSparksLogin.Login ("akil.hotshot@gmail.com", "akil.hotshot@gmail.com");
		#endif
		yield return null;
	}
}