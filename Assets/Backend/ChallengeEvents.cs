using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Messages;

public class ChallengeEvents : MonoBehaviour {



	// Use this for initialization
	void Start () {
		ChallengeIssuedMessage.Listener += GotChallenge;
		ChallengeAcceptedMessage.Listener += ChallengeAccepted;
		ChallengeDeclinedMessage.Listener += ChallengeDeclined;
		ChallengeWithdrawnMessage.Listener += ChallengeWithdrawn;
		ChallengeStartedMessage.Listener += ChallengeStarted;
	}
	
	void GotChallenge(GSMessage message)
	{
		print (message.JSONString);
		EventManager.instance.OnRecievedChallenge (message);
	}

	void ChallengeDeclined(GSMessage msg)
	{
		print (msg.JSONString);
		EventManager.instance.OnDeclinedChallenge (msg);
	}

	void ChallengeAccepted(GSMessage msg)
	{
		print (msg.JSONString);
		EventManager.instance.OnAcceptedChallenge (msg);
	}

	void ChallengeWithdrawn(GSMessage msg)
	{
		print (msg.JSONString);
		EventManager.instance.OnWithdrawChallenge ();
	}

	void ChallengeStarted(GSMessage msg)
	{
		print ("Challenge started!");
		EventManager.instance.OnChallengeStarted (msg);
	}
}
