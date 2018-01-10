using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Core;
using System;

public class DeclineChallenge : MonoBehaviour {

	public void Decline(string id)
	{
		new DeclineChallengeRequest ().SetChallengeInstanceId (id).Send ((response)=>{
			if(response.HasErrors)
			{
				print("Decline challenge failed!");
			}
			else
			{
				print("Decline challenge success!");
				ChallegeWaitGUI.instance.isBusy = false;
			}
			print(response.JSONString);
		});
	}

	public void Decline(string id,string msg)
	{
		new DeclineChallengeRequest ().SetMessage (msg).SetChallengeInstanceId (id).Send ((response)=>{
			if(response.HasErrors)
			{
				print("Decline challenge failed!");
			}
			else
			{
				print("Decline challenge success!");
			}
			print(response.JSONString);
		});
	}
}