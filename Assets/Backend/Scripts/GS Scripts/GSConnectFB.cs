using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;

public class GSConnectFB : MonoBehaviour {

	public void Connect(string token)
	{
		new FacebookConnectRequest()
			.SetAccessToken(token)
			.Send((response) => {
				if(response.HasErrors)
				{
					print ("Facebook connect failed!");
					EventManager.instance.OnFBConnectFailed (response.JSONString.ToGSErrorData ());
				}
				else
				{
					print ("Facebook connect success!");
					EventManager.instance.OnFBConnectSuccess();
				}
				print (response.JSONString);
			});
	}
}