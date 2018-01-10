using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;

public class WithdrawChallenge : MonoBehaviour {

	public void Withdraw(string id)
	{
		new WithdrawChallengeRequest()
			.SetChallengeInstanceId(id)
			.Send((response) => {
				if(response.HasErrors)
				{
					print ("Withdraw challenge failed!");
				}
				else
				{
					print ("Withdraw challenge success!");
					ChallegeWaitGUI.instance.isBusy = false;
					EventManager.instance.OnWithdrawChallenge ();
				}
			});
	}
	 
}