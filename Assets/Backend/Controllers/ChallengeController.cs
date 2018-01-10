using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Messages;
using DoozyUI;
public class ChallengeController : Singleton<ChallengeController> 
{
	public ChallengeWindowGUI gotChallenge;
	public ChallegeWaitGUI challengeWait;
	public UIElement loadingMatch;
	public ChallengeStartData challengeStart;
	public DeclineChallenge decline;

	void OnEnable()
	{
		EventManager.instance.recievedChallenge += GotChallenge;
		EventManager.instance.createChallengeSuccess += CreateChallengeSuccess;
		EventManager.instance.declinedChallenge += ChallengeDeclined;
		EventManager.instance.withdrawChallenge += ChallengeWithdrawn;
		EventManager.instance.challengeStarted += ChallengeStarted;
	}

	void OnDisable()
	{
		if (EventManager.instance != null) {
			EventManager.instance.recievedChallenge -= GotChallenge;
			EventManager.instance.createChallengeSuccess -= CreateChallengeSuccess;
			EventManager.instance.declinedChallenge -= ChallengeDeclined;
			EventManager.instance.withdrawChallenge -= ChallengeWithdrawn;
			EventManager.instance.challengeStarted -= ChallengeStarted;
		}
	}


	void GotChallenge(GSMessage msg)
	{
		ChallengeData data = JsonUtility.FromJson<ChallengeData> (msg.JSONString);

		if (!ChallegeWaitGUI.instance.isBusy) {
			if (AccountDetails.instance.accountDetails.userId == data.challenge.challenged [0].id)
				gotChallenge.Set (data);
			ChallegeWaitGUI.instance.isBusy = true;
		}
		else
		{
			decline.Decline (data.challenge.challengeId);
		}
	}

	void CreateChallengeSuccess(CreateChallengeData data)
	{
//		if(data.)
		challengeWait.Set (data);
	}

	void ChallengeDeclined(GSMessage msg)
	{
		challengeWait.Hide ();
		ChallegeWaitGUI.instance.isBusy = false;
	}

	void ChallengeWithdrawn()
	{
		ChallegeWaitGUI.instance.isBusy = false;
		gotChallenge.Hide ();
	}

	void ChallengeStarted(GSMessage msg)
	{
		challengeWait.Hide ();
		print (msg.JSONString);
		challengeStart = JsonUtility.FromJson<ChallengeStartData> (msg.JSONString);
		print ("Create a server in "+challengeStart.challenge.challengeMessage);
	}
}