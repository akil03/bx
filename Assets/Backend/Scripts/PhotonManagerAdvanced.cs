using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using DoozyUI;
using GameSparks.Api.Messages;
using GameSparks.Core;
using System.Linq;

public class PhotonManagerAdvanced : MonoBehaviour 
{

	public static PhotonManagerAdvanced instance;
	public bool connectToMasterOnStart;
	public ConnectionStatus serverStatus;
	public ConnectionStatus roomStatus;
	public ConnectionStatus lobbyStatus;
	public int maxplayers;
	public string ping;
	public bool delete;
	public string selectedServer;
	public TypedLobby matchmakingLobby;
	public List<PhotonPlayer> players;
	bool normal;

	void OnEnable()
	{
//		EventManager.instance.withdrawChallenge += RebootConnection;
//		EventManager.instance.declinedChallenge += RebootConnection;
//		EventManager.instance.challengeStarted += ChallengeAccepted;
	}


	void OnDisable()
	{
		if (delete)
			PlayerPrefs.DeleteAll ();
	}

	void Awake()
	{
		if(instance==null)
		instance = this;
		if (instance != this)
			Destroy (gameObject);
		else
			DontDestroyOnLoad (gameObject);
	}

	IEnumerator Start()
	{
		yield return _ConnectToMaster (Success, Failure);
		yield return GetPing();
	}

//	void ChallengeAccepted(GSMessage msg)
//	{
//		joining = true;
//		StartCoroutine (Timeout ());
//	}


//	IEnumerator Timeout()
//	{
//		yield return new WaitForSeconds (10);
//		if (joining) 
//		{
//			EventManager.instance.OnWithdrawChallenge ();
//			joining = false;
//		}
//	}

	public bool IsInGame()
	{
		return PhotonNetwork.inRoom && roomStatus==ConnectionStatus.connected && PhotonNetwork.room.PlayerCount == maxplayers;
	}

	IEnumerator GetPing()
	{
		while (!GS.Authenticated)
			yield return null;
		AccountDetails.instance.Get ();
		while (string.IsNullOrEmpty (ping))
			yield return null;
		new LogEventRequest ().SetEventKey ("SetPlayerPing").SetEventAttribute ("PING", ping).Send ((response) => {
			if (!response.HasErrors) 
			{
				print ("ping set "+ping+"!");
			}
			else
			{
				print ("ping failed");
			}
		});
	}

	public void Cancel()
	{
		if (PhotonNetwork.room.PlayerCount < maxplayers) {
			normal = false;
			PhotonNetwork.LeaveRoom ();
			GUIManager.instance.ShowMainMenuGUI ();
		}
	}

	void Success()
	{
		print ("woo hoo!!!");
	}

	void Failure()
	{
		print ("oh fuck!!!");
	}
	public IEnumerator _ConnectToMaster(ParameterlessDelegate success=null,ParameterlessDelegate failed=null)
	{
		serverStatus = ConnectionStatus.connecting;
		PhotonNetwork.ConnectUsingSettings (Application.version);
		int i = 0;
		while (serverStatus == ConnectionStatus.connecting && i < 5) 
		{
			yield return new WaitForSeconds(1);
			i++;
		}
		if (serverStatus == ConnectionStatus.connected) {
			print ("Connected to server!!!");
			if(success!=null)
				success ();
		}
		else if(serverStatus == ConnectionStatus.failed || serverStatus == ConnectionStatus.disconnected || serverStatus == ConnectionStatus.connecting)
		{
			print("Failed to connect to server!!!");
			serverStatus = ConnectionStatus.disconnected;
			if(failed!=null)
			failed();			
		}
	}

	void Update()
	{
//		if (PhotonNetwork.insideLobby) 
//		{
//			print ("No of available rooms is "+PhotonNetwork.GetRoomList().Length);
//			print (PhotonNetwork.countOfPlayersOnMaster);
//		}
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Cancel ();
		}
	}

	void OnConnectedToMaster()
	{	
		serverStatus = ConnectionStatus.connected;
		if (matchmakingPhase) 
		{
			if (isServer) 
			{	
				print ("create room");
				PhotonNetwork.CreateRoom (null);	
			}
			else
			{
				print ("join room");
				PhotonNetwork.JoinRandomRoom ();
			}
			matchmakingPhase = false;
		}
	}

	void OnDisconnectedFromPhoton()
	{
		print ("disconnected manager");
		serverStatus = ConnectionStatus.disconnected;
		PowerUpManager.instance.dontSpawn = true;
	}

	void OnConnectionFail(DisconnectCause cause)
	{
		serverStatus = ConnectionStatus.failed;
		roomStatus = ConnectionStatus.failed;
	}

	IEnumerator CloseFindingMatchScren()
	{
		roomStatus = ConnectionStatus.disconnected;
		GUIManager.instance.OpenPage (3);
		yield break;
	}

	public IEnumerator _CreateRoom()
	{
		if (!PhotonNetwork.connected) 
		{
			GUIManager.instance.ShowLog ("Not connected to multiplayer server");
			yield break;
		}
		matchmakingPhase = true;
		normal = true;
		PhotonNetwork.JoinOrCreateRoom ("matchmaking",null,null);
	}

	public bool matchmakingPhase;

	void OnJoinedRoom()
	{
		if (matchmakingPhase) 
		{
			if (PhotonNetwork.room.PlayerCount >= 2)
			{
				print ("Begin a game!");
				if (PhotonNetwork.player.ID % 2 != 0) 
				{
					isServer = true;
				}
				else
				{
					isServer = false;
				}
				PhotonNetwork.LeaveRoom ();
				return;
			}
			else
			{
				print ("Added to queue!");
				return;
			}
		}
		print ("Joined room");
		roomStatus = ConnectionStatus.connected;
		new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 1).Send((response)=> {});
		ObliusGameManager.instance._ShowFindingMatchScreen (PhotonNetwork.player.ID);
		GSUpdateMMR.instance.loading.Hide (false);
		print ("Max players reached!");
		isServer = false;	
		if(PhotonNetwork.room.PlayerCount == maxplayers)
		PhotonNetwork.room.IsVisible = false;
	}

	void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (PhotonNetwork.player.ID != player.ID) 
		{
			if (matchmakingPhase) 
			{
				if (PhotonNetwork.room.PlayerCount >= 2)
				{
					print ("Begin a game!");
					if (PhotonNetwork.player.ID % 2 != 0) 
					{
						isServer = true;
					}
					else
					{
						isServer = false;
					}
					PhotonNetwork.LeaveRoom ();
					return;
				}
				else
				{
					print ("Added to queue!");
					return;
				}
			}	
		}
	}


	void OnLeftLobby()
	{
		lobbyStatus = ConnectionStatus.disconnected;
	}

	void OnCreatedRoom()
	{
		roomStatus = ConnectionStatus.connected;
	}

	void OnPhotonCreateRoomFailed()
	{
		roomStatus = ConnectionStatus.failed;
	}



	public IEnumerator _JoinRandomRoom(ParameterlessDelegate success=null,ParameterlessDelegate failed=null)
	{
//		if (serverStatus == ConnectionStatus.disconnected) 
//		{
//			if(failed!=null)
//			failed ();
//			print ("Not connected to host!");
//		}
//		int j = 0;
//		while (serverStatus != ConnectionStatus.connected && j<5){
//			yield return new WaitForSeconds (1);
//			j++;
//		}
//		if (serverStatus != ConnectionStatus.connected)
//			yield break;
//		else
//		yield return new WaitForSeconds(0.5f);
//		if (roomStatus == ConnectionStatus.connected || roomStatus == ConnectionStatus.connecting) {
//			CloseFindingMatchScren ();
//		}
//		if (PhotonNetwork.connectedAndReady)
			PhotonNetwork.JoinRandomRoom ();
//		else 
//		{
//			roomStatus = ConnectionStatus.disconnected;
//			GUIManager.instance.OpenPage (3);
//			yield break;
//		}
		roomStatus = ConnectionStatus.connecting;
		int i = 0;
		while (roomStatus == ConnectionStatus.connecting && i<5){
			yield return new WaitForSeconds (1);
			i++;
		}
		if (roomStatus == ConnectionStatus.connected) {
			print ("Joined random room!!!");
			if(success!=null)
				success ();
		}
		else if(roomStatus == ConnectionStatus.failed || roomStatus == ConnectionStatus.disconnected)
		{
			print("Failed to join a random room!!!");
			if(failed!=null)
				failed();
		}
	}

	public bool isServer;


	void OnPhotonRandomJoinFailed()
	{
		roomStatus = ConnectionStatus.failed;
		StartCoroutine (Retry());
		print ("failed to random join");
	}

	IEnumerator Retry()
	{
		if (!normal)
			yield break;
		yield return new WaitForSeconds(1);
		PhotonNetwork.JoinRandomRoom();
	}

	void  JoinRandomRoomFailure()
	{
		StartCoroutine (_CreateRoom());
	}

	void OnLeftRoom()
	{
		roomStatus = ConnectionStatus.disconnected;
		new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 0).Send((response)=> {});
		PowerUpManager.instance.dontSpawn = true;
	}


	public void CloseUP()
	{
		SnakesSpawner.instance.KillAllNetworkSnakes ();
		PowerUpManager.instance.ClearPowerUps ();
		GroundSpawner.instance.ClearGround ();
	}

	public void RebootConnection()
	{
		if (GSUpdateMMR.instance.loading.isActiveAndEnabled)
			GSUpdateMMR.instance.loading.Hide (false);
		CloseUP ();
		PhotonNetwork.LeaveRoom ();
		PhotonNetwork.Disconnect ();
		GUIManager.instance.inGameGUI.GetComponent<UIElement> ().Hide (false);
		InternetChecker.instance.reconnect = true;
	}

	void RebootConnection(GSMessage msg)
	{
		CloseUP ();
		PhotonNetwork.LeaveRoom ();
		PhotonNetwork.Disconnect ();
		GUIManager.instance.inGameGUI.GetComponent<UIElement> ().Hide (false);
		InternetChecker.instance.reconnect = true;
	}
}

public enum ConnectionStatus
{
	none,
	connecting,
	connected,
	failed,
	disconnected
}

public enum Actions
{
	createRoom,
	joinRoom,
	createAndJoin
}



//public delegate void ParameterlessDelegate();