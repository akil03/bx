using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Messages;

public class ChallengeEvents : MonoBehaviour 
{
	[SerializeField]EventObject accepted;
	[SerializeField]EventObject declined;
	[SerializeField]EventObject withdrawn;
	[SerializeField]EventObject recieved;
	[SerializeField]EventObject started;
	[SerializeField]ChallengeDataObject challenge;
	[SerializeField]ChallengeDataObject challengeOther;
	[SerializeField]BoolObject occupied;

	// Use this for initialization
	void Start () 
	{
		occupied.value = false; 
		ChallengeIssuedMessage.Listener += GotChallenge;
		ChallengeAcceptedMessage.Listener += ChallengeAccepted;
		ChallengeDeclinedMessage.Listener += ChallengeDeclined;
		ChallengeWithdrawnMessage.Listener += ChallengeWithdrawn;
		ChallengeStartedMessage.Listener += ChallengeStarted;
	}
	
	void GotChallenge(GSMessage message)
	{
		if (!occupied.value)
		{
			challenge.data = JsonUtility.FromJson<ChallengeData> (message.JSONString);
			recieved.Fire ();
			EventManager.instance.OnRecievedChallenge (message);
			print ("recived challenged firedddddddddddddddddddddd");
		}
		else
		{
			challengeOther.data = JsonUtility.FromJson<ChallengeData> (message.JSONString);
			recieved.Fire ();
		}
			
	}

	void ChallengeDeclined(GSMessage msg)
	{
		challenge.Reset ();
		declined.Fire ();
		challenge.Reset ();
		EventManager.instance.OnDeclinedChallenge (msg);
		print (msg.JSONString);
	}

	void ChallengeAccepted(GSMessage msg)
	{
		print (msg.JSONString);
		accepted.Fire ();
		EventManager.instance.OnAcceptedChallenge (msg);
	}

	void ChallengeWithdrawn(GSMessage msg)
	{
		challenge.Reset ();
		withdrawn.Fire ();
		challenge.Reset ();
		EventManager.instance.OnWithdrawChallenge ();
		print (msg.JSONString);
	}

	void ChallengeStarted(GSMessage msg)
	{
//		if (!occupied.value) {
			started.Fire ();
			EventManager.instance.OnChallengeStarted (msg);
			print ("Challenge started!");
//		}
	}
}
