using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhoton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Disconnect(){
		PhotonNetwork.Disconnect ();

	}
//	public void OnConnectedToMaster()
//	{		
//		print ("connected");
//	}
//
	void OnDisconnectedFromPhoton()
	{
		print ("disconnected");
		PhotonNetwork.ConnectToRegion ("in".ToCloudRegionCode (), Application.version);
	}
}
