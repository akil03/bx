using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;


public class GSRegister : MonoBehaviour {

	public void Register(string values)
	{
		string[] data = values.Split ('.');
		Register (data[0],data[1],data[2]);
	}

	public void Register(string nickName,string password,string userName)
	{
		new RegistrationRequest()
			.SetDisplayName(nickName)
			.SetPassword(password)
			.SetUserName(userName)
			.Send((response) => {
				if(response.HasErrors)
				{
					print ("GS Registration failed!");
					EventManager.instance.OnGSRegistrationFailed (response.JSONString.ToGSErrorData ());
				}
				else
				{
					print ("GS Registration success!");
					EventManager.instance.OnGSRegistrationSuccess();
				}
				print (response.JSONString);
			});
	}
}
