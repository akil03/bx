using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Core;
using System;

public class CreateChallenge : MonoBehaviour {

	public void Create(string id)
	{
		new CreateChallengeRequest()			
			.SetChallengeShortCode("play")
			.SetEndTime(DateTime.Parse(DateTime.Now.AddMinutes (2).ToString("yyyy-MM-ddTHH:mmZ")))
			.SetChallengeMessage (PhotonManagerAdvanced.instance.selectedServer)
			.SetUsersToChallenge(new List<string>(){id})
			.Send((response) => {
				if(response.HasErrors)
				{
					print("Create challenge failed!");
				}
				else
				{
					print("Create challenge success!");
					EventManager.instance.OnCreateChallengeSuccess (JsonUtility.FromJson<CreateChallengeData>(response.JSONString));
				}
				print(response.JSONString);
			});
	}
}