using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoozyUI;
using System.Linq;
using GameSparks.Api.Requests;
using GameSparks.Api.Messages;

public class ChallengeWindowGUI : Singleton<ChallengeWindowGUI> {

	public Text challengeMsg;
	public ChallengeData challenge=null;
	public DeclineChallenge decline;
	public AcceptChallenge accept;
	public UIElement element;
	public UIElement matchLoading;
	public string region;

	void Start()
	{
		challenge = null;
	}

	public void Set(ChallengeData data)
	{
		challenge = data;
		challengeMsg.text = data.challenge.challenger.name + " has challenged you for a game!";
		element.gameObject.SetActive (true);
		element.Show (false);			
	}

	public void DeclineChallenge()
	{
		decline.Decline (challenge.challenge.challengeId);
		Hide ();
		challenge = null;
	}

	void LapseChallenge()
	{
		Hide ();
		challenge = null; 
	}

	public void AcceptChallenge()
	{
		if (!PhotonNetwork.connected) {
			GUIManager.instance.ShowLog ("Please wait! Server not ready.");
			return;
		}
		element.Hide (false);
		matchLoading.gameObject.SetActive (true);
		matchLoading.Show (false);
		InternetChecker.instance.reconnect = false;
		JoinGame (challenge.message);
	}

	public void Hide()
	{
		element.Hide (false);
	}

	public void JoinGame(string region)
	{
		PhotonNetwork.Disconnect ();
		this.region = region;
	}

	void OnDisconnectedFromPhoton()
	{
		if (!InternetChecker.instance.reconnect && challenge!=null) {
			PhotonNetwork.ConnectToRegion (region.ToCloudRegionCode (), Application.version);
		}
	}

	public void OnConnectedToMaster()
	{		
		if (!InternetChecker.instance.reconnect && challenge!=null) {
			print ("Connected to master!");
			print (challenge.challenge.challenger.name);
			PhotonNetwork.JoinRoom (challenge.challenge.challenged[0].name);
		}
	}

	void OnJoinedRoom()
	{
		if (!InternetChecker.instance.reconnect && challenge!=null) {
			print ("Joined room!");
			SnakesSpawner.instance.CreateNetworkSnake (2);
			ObliusGameManager.instance.ResetGame ();
			GUIManager.instance.ShowInGameGUI ();
			accept.Accept (challenge.challenge.challengeId);
			challenge = null;
		}
	}

	void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		if (codeAndMsg [0].ToString ().Contains ("32758")) {
			matchLoading.Hide (false);
			PhotonManagerAdvanced.instance.RebootConnection ();
		}
		print ("Photon room join failed and error code is "+codeAndMsg[0].ToString ());
	}	
}