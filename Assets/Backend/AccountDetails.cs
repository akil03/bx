﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;

public class AccountDetails : Singleton<AccountDetails> {

	public AccountDetailsData accountDetails;

	public void Get()
	{
		new AccountDetailsRequest ().Send ((response)=>{
			if(response.HasErrors)
			{
				print ("Getting account details failed!");
			}
			else
			{
				print ("Getting account details success!");
				accountDetails = JsonUtility.FromJson<AccountDetailsData>(response.JSONString);
			}
			print (response.JSONString);
		});
	}
}
