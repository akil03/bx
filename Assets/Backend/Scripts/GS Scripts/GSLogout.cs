using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Core;

public class GSLogout : MonoBehaviour {

	public void Logout()
	{
		GS.Reset ();
		print ("GS logged out!");
		EventManager.instance.OnGSLogoutSuccess ();
	}
}
