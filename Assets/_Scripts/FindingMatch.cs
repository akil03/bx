using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FindingMatch : MonoBehaviour {
	public Text ServerText, RoomText,playerCount;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (PhotonNetwork.connected)
		{
			string serverStatus = PhotonNetwork.connected ? "Connected" : "Waiting";
			string roomStatus = PhotonNetwork.inRoom ? "Connected" : "Waiting";
			ServerText.text = "Server status: " + serverStatus;
			RoomText.text = "Room status: " + roomStatus;
			if (PhotonNetwork.room != null)
				playerCount.text = "Players not found !! Room Created, Player Count: " + PhotonNetwork.room.PlayerCount + " /2 ";
			else
				playerCount.text = "Searcing for existing players !!";
		}
	}
}
