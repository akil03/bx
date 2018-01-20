﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;
using UnityEngine.UI;

public class ChallengeGUIControl : MonoBehaviour 
{
	public UIElement waitingScreen;
	public UIElement challengeScreen;
	public UIElement loading;
	public Text challengeText;
	public ChallengeDataObject challenge;
	public StringObject userId;
	public Text userIdText;
	public InputField playerName;

	public void ShowWaitingScreen()
	{
		waitingScreen.gameObject.SetActive (true);
		waitingScreen.Show (false);
	}

	public void ShowLoadingScreen()
	{
		loading.gameObject.SetActive (true);
		loading.Show (false);
	}

	public void HideLoadingScreen()
	{
		loading.Hide (false);
	}

	public void HideWaitingScreen()
	{
		waitingScreen.Hide(false);
	}

	public void Reconnect()
	{
		PhotonManagerAdvanced.instance.CloseUP ();
		InternetChecker.instance.reconnect = true;
		PhotonNetwork.Disconnect ();
	}

	public void ShowChallengeScreen()
	{
		challengeText.text = challenge.data.challenge.challenged [0].name + " has invited you to a friendly battle!";
		challengeScreen.gameObject.SetActive (true);
		challengeScreen.Show (false);
	}

	public void HideChallengeScreen()
	{
		challengeScreen.Hide(false);
	}

	public void SetPlayerDetails()
	{
		userIdText.text = userId.value;
		playerName.text = AccountDetails.instance.accountDetails.displayName;
		print ("this retarded function gets called");
		print (AccountDetails.instance.accountDetails.displayName);
	}
}
