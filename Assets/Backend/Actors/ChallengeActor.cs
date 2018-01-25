using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using System;

public class ChallengeActor : MonoBehaviour {

	public StringObject server;
	public StringObject opponentId;
	public StringObject userId;
	public ChallengeDataObject challenge;
	public ChallengeDataObject challengeOther;
	public EventObject created;
	public EventObject withdrawn;
	public EventObject accepted;
	public EventObject declined;
	public BoolObject occupied;
	string cid;

	void Start()
	{
		challenge.Reset ();
		challengeOther.Reset ();
	}

	void OnDisable()
	{
		challenge.Reset ();
		challengeOther.Reset ();
	}

	public void Create()
	{
		print ("creating room");
		new CreateChallengeRequest()			
			.SetChallengeShortCode("play")
			.SetEndTime(DateTime.Parse(DateTime.UtcNow.AddMinutes (2).ToString("yyyy-MM-ddTHH:mmZ")))
			.SetChallengeMessage (server.value)
			.SetUsersToChallenge(new List<string>(){opponentId.value})
			.Send((response) => {
				if(response.HasErrors)
				{
					print("Create challenge failed!");
				}
				else
				{
					cid = response.ChallengeInstanceId;
					created.Fire();
					print("Create challenge success!");
				}
				print(response.JSONString);
			});
	}

	public void Withdraw()
	{
		new WithdrawChallengeRequest()
			.SetChallengeInstanceId(cid)
			.Send((response) => {
				if(response.HasErrors)
				{
					print ("Withdraw challenge failed!");
				}
				else
				{
					challenge.Reset();
					withdrawn.Fire();
					InternetChecker.instance.reconnect = true;
					print ("Withdraw challenge success!");
				}
			});
	}

	public void Accept()
	{
		new AcceptChallengeRequest ().SetChallengeInstanceId (challenge.data.challenge.challengeId)
			.Send ((response)=>{
				if(response.HasErrors)
				{					
					print("Accept challenge failed!");
				}
				else
				{
					accepted.Fire();
					print("Accept challenge success!");
				}
				print(response.JSONString);
			});
	}

	public void Decline()
	{
		new DeclineChallengeRequest ().SetChallengeInstanceId (challenge.data.challenge.challengeId).Send ((response)=>{
			if(response.HasErrors)
			{
				print("Decline challenge failed!");
			}
			else
			{
				challenge.Reset();
				declined.Fire();
				InternetChecker.instance.reconnect = true;
				print("Decline challenge success!");
			}
			print(response.JSONString);
		});
	}

	public void DeclineOther()
	{
		new DeclineChallengeRequest ().SetChallengeInstanceId (challengeOther.data.challenge.challengeId).Send ((response)=>{
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


	public void StartChallenge()
	{
		InternetChecker.instance.reconnect = false;
		PhotonNetwork.Disconnect ();
	}

	void OnDisconnectedFromPhoton()
	{

		if (!InternetChecker.instance.reconnect) 
		{
			if(challenge.data.challenge != null)
			{
				PhotonNetwork.ConnectToRegion (challenge.data.message.ToCloudRegionCode (), Application.version);
			}
			else
			{
				PhotonNetwork.ConnectToRegion (server.value.ToCloudRegionCode(), Application.version);
			}
		}
	}

	public void OnConnectedToMaster()
	{		

		if (!InternetChecker.instance.reconnect)
		{
			if(challenge.data.challenge != null)
			{
				PhotonNetwork.JoinRoom (challenge.data.challenge.challenged[0].id);
				print ("Connected to master!");
			}
			else
			{
				PhotonNetwork.CreateRoom (opponentId.value,new RoomOptions(){MaxPlayers = (byte)2,IsVisible = false},TypedLobby.Default);
			}
		}
	}

	void OnJoinedRoom()
	{
		if (!InternetChecker.instance.reconnect)
		{	
			if(challenge.data.challenge != null)
			{
				ObliusGameManager.instance.ResetGame ();
				SnakesSpawner.instance.CreateNetworkSnake (2);
				GUIManager.instance.ShowInGameGUI ();
				challenge.Reset ();
				print ("Joined room!");

			}
			else
			{
				ObliusGameManager.instance.ResetGame ();
				SnakesSpawner.instance.CreateNetworkSnake (1);
				Create ();
				challenge.Reset ();
				print ("Joined room!");

			}
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (!InternetChecker.instance.reconnect && PhotonNetwork.room.PlayerCount == 2) 
		{
			accepted.Fire ();
			challenge.Reset ();
			occupied.value = false;
			GUIManager.instance.ShowInGameGUI ();
			print (player.ID+" has connected.");
		}
	}	

	public void Reconnect()
	{
		challenge.data = new ChallengeData ();
	}

	public void CanAcceptChallenge()
	{
		print (occupied.value+" can accept challenge!");
		if (occupied.value)
			DeclineOther ();	
	}

	public void SetOccupiedStatus(bool val)
	{
		occupied.value = val;
	}


}