using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Core;

public class AcceptChallenge : MonoBehaviour {

	public void Accept(string id)
	{
		new AcceptChallengeRequest ().SetChallengeInstanceId (id)
			.Send ((response)=>{
			if(response.HasErrors)
			{					
				print("Accept challenge failed!");
			}
			else
			{
				print("Accept challenge success!");
			}
			print(response.JSONString);
		});
	} 
}
