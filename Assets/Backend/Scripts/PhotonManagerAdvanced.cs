using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using DoozyUI;
using GameSparks.Api.Messages;
using GameSparks.Core;

public class PhotonManagerAdvanced : MonoBehaviour 
{

	public static PhotonManagerAdvanced instance;
	public bool connectToMasterOnStart;
	public ConnectionStatus serverStatus;
	public ConnectionStatus roomStatus;
	public int maxplayers;
	public string ping;
	public bool delete;
	public string selectedServer;
	bool joining;

	void OnEnable()
	{
		EventManager.instance.withdrawChallenge += RebootConnection;
		EventManager.instance.declinedChallenge += RebootConnection;
//		EventManager.instance.challengeStarted += ChallengeAccepted;
	}


	void OnDisable()
	{
		if (EventManager.instance != null) {
			EventManager.instance.withdrawChallenge -= RebootConnection;
			EventManager.instance.declinedChallenge -= RebootConnection;
//			EventManager.instance.challengeStarted -= ChallengeAccepted;
		}
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
		while (serverStatus == ConnectionStatus.connecting)
			yield return null;
		if (serverStatus == ConnectionStatus.connected) {
			print ("Connected to server!!!");
			if(success!=null)
				success ();
		}
		else if(serverStatus == ConnectionStatus.failed || serverStatus == ConnectionStatus.disconnected)
		{
			print("Failed to connect to server!!!");
			if(failed!=null)
			failed();
		}
	}

	void OnConnectedToMaster()
	{	
		serverStatus = ConnectionStatus.connected;
	}

	void OnDisconnectedFromPhoton()
	{
		print ("disconnected manager");
		serverStatus = ConnectionStatus.disconnected;
		PowerUpManager.instance.dontSpawn = true;

//		if (ChallegeWaitGUI.instance.isBusy || ChallengeWindowGUI.isChallenged)
//			return;
		
//		if (!InternetChecker.instance.reconnect&&!ChallengeWindowGUI.isChallenged&&!ChallegeWaitGUI.instance.isBusy) 
//		{
//			print ("creating server");
//			PhotonNetwork.ConnectUsingSettings (Application.version);
//		}
	}

	void OnConnectionFail(DisconnectCause cause)
	{
		serverStatus = ConnectionStatus.failed;
//		GUIManager.instance.OpenPage (3);
//		print (cause.ToString());
	}

	public IEnumerator _CreateRoom(ParameterlessDelegate success=null,ParameterlessDelegate failed=null,ParameterlessDelegate playersFilled=null,ParameterlessDelegate noPlayers=null)
	{
		int k = 0;
		while (serverStatus != ConnectionStatus.connected && k < 5){
			yield return new WaitForSeconds (1);
			k++;
		}
		if (serverStatus != ConnectionStatus.connected) 
		{
			GUIManager.instance.OpenPage (3);
			serverStatus = ConnectionStatus.disconnected;
			yield break;
		}
		else
		if (roomStatus == ConnectionStatus.connected || roomStatus == ConnectionStatus.connecting) {
			roomStatus = ConnectionStatus.disconnected;
			GUIManager.instance.OpenPage (3);
			yield break;
		}
		if (PhotonNetwork.connectedAndReady)
			PhotonNetwork.CreateRoom (null, new RoomOptions (){ MaxPlayers = (byte)maxplayers }, null);
		else
		{
			roomStatus = ConnectionStatus.disconnected;
			GUIManager.instance.OpenPage (3);
			yield break;
		}
		roomStatus = ConnectionStatus.connecting;
		int j = 0;
		while (roomStatus == ConnectionStatus.connecting && j < 5) {
			yield return new WaitForSeconds (1);
			j++;
		}
			if (roomStatus == ConnectionStatus.connected) {
			print ("Created room!!!");
			if(success!=null)
				success ();
		}
		else if(roomStatus == ConnectionStatus.failed || roomStatus == ConnectionStatus.disconnected || roomStatus == ConnectionStatus.connecting)
		{
			roomStatus = ConnectionStatus.failed;
			print("Failed to create a room!!!");
			if(failed!=null)
				failed();
		}
		int i=0;
		while (PhotonNetwork.room.PlayerCount < maxplayers && i<5) 
		{
			yield return new WaitForSeconds (1);
			i++;
		}
		if (PhotonNetwork.room.PlayerCount != maxplayers) {
			PhotonNetwork.LeaveRoom ();
			if (noPlayers != null)
				noPlayers ();
			print ("No players!!!");
		}
		else
		{
			if(playersFilled!=null)
				playersFilled ();
			print ("Tme to roll!!!");
		}
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
		if (serverStatus == ConnectionStatus.disconnected) 
		{
			if(failed!=null)
			failed ();
			print ("Not connected to host!");
		}
		int j = 0;
		while (serverStatus != ConnectionStatus.connected && j<5){
			yield return new WaitForSeconds (1);
			j++;
		}
		if (serverStatus != ConnectionStatus.connected)
			yield break;
		else
		yield return new WaitForSeconds(0.5f);
		if (roomStatus == ConnectionStatus.connected || roomStatus == ConnectionStatus.connecting) {
			roomStatus = ConnectionStatus.disconnected;
			GUIManager.instance.OpenPage (3);
			yield break;
		}
		if (PhotonNetwork.connectedAndReady)
			PhotonNetwork.JoinRandomRoom ();
		else 
		{
			roomStatus = ConnectionStatus.disconnected;
			GUIManager.instance.OpenPage (3);
			yield break;
		}
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

	void OnJoinedRoom()
	{
		roomStatus = ConnectionStatus.connected;
		new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 1).Send((response)=> {});
		if (PhotonNetwork.room.PlayerCount == maxplayers) 
		{
			PhotonNetwork.room.IsVisible = false;
			GSUpdateMMR.instance.loading.Hide (false);
			print ("Max players reached!");
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		joining = false;
		if (PhotonNetwork.room.PlayerCount == maxplayers) 
		{
			GSUpdateMMR.instance.loading.Hide (false);
			print ("Max players reached!");
		}
	}

	void OnPhotonRandomJoinFailed()
	{
		roomStatus = ConnectionStatus.failed;
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