﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonActor : MonoBehaviour 
{
	[SerializeField]bool connectOnStart; 
	[SerializeField]EventObject connectToMasterSuccess;
	[SerializeField]EventObject connectToMasterFailed;
	[SerializeField]EventObject roomCreated;
	[SerializeField]EventObject createRoomFailed;
	[SerializeField]EventObject joinedRoom;
	[SerializeField]EventObject joinRoomFailed;
	[SerializeField]EventObject randomJoinFailed;
	[SerializeField]EventObject playerConnected;
	[SerializeField]EventObject playerDisconnected;
	[SerializeField]EventObject leftRoom;
	[SerializeField]EventObject disconnectedFromPhoton;
	[SerializeField]BoolObject reconnect;

	void Start()
	{
		if (connectOnStart)
			ConnectToMaster ();
	}

	void ConnectToMaster()
	{
		PhotonNetwork.ConnectUsingSettings (Application.version);	
	}

	void OnConnectedToMaster()
	{
		connectToMasterSuccess.Fire ();
		SavePing ();
	}

	void OnConnectionFail(DisconnectCause cause)
	{
		connectToMasterFailed.Fire ();
	}

	void OnCreateRoom()
	{
		roomCreated.Fire ();
	}

	void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{
		createRoomFailed.Fire ();
	}

	void OnJoinedRoom()
	{
		joinedRoom.Fire ();	
	}

	void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		joinRoomFailed.Fire ();
	}

	void OnPhotonRandomJoinFailed()
	{
		randomJoinFailed.Fire ();
	}

	void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		playerConnected.Fire ();	
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		playerDisconnected.Fire ();
	}

	void OnLeftRoom()
	{
		leftRoom.Fire ();
	}

	void SavePing()
	{
		GameSparkRequests request = new GameSparkRequests ();
		request.Request ("SetPlayerPing", "PING", PhotonPingManager.ping,Print);
	}

	public void Print(string str)
	{
		print (str);
	}

	void OnDisconnectedFromPhoton()
	{
		disconnectedFromPhoton.Fire ();
	}

	public void Reconnect()
	{
		if (reconnect)
			ConnectToMaster ();
	}
}
