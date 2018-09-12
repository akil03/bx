using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonDelayTest : MonoBehaviour {
    public GameObject networkObject;

    public static float startTime;
	// Use this for initialization
	void Start () {
        CreateRoom();

    }
	
	// Update is called once per frame
	void Update () {
       

    }


    public void CreateRoom()
    {
        StartCoroutine(JoinRoom());
    }

    IEnumerator JoinRoom()
    {
        while (!PhotonNetwork.connectedAndReady)
            yield return null;


        PhotonNetwork.JoinOrCreateRoom("DelayTestRoom", new RoomOptions() { MaxPlayers = (byte)2, IsVisible = false, EmptyRoomTtl = 0 }, TypedLobby.Default);

        while (PhotonNetwork.room == null)
            yield return null;


        while (PhotonNetwork.room.PlayerCount == 0)
            yield return null;
       
        networkObject = PhotonNetwork.Instantiate("DelayTestObj", Vector3.zero, Quaternion.identity, new byte());

        yield return new WaitForSeconds(2);

        while (true)
        {
            startTime = Time.time;
            PrintViaServer();
            //PrintViaServerInstant();
            yield return new WaitForSeconds(1f);
        }

        

    }


    public void PrintViaServer()
    {
        PhotonView.Get(networkObject).RPC("ShowLog", PhotonTargets.AllViaServer, PhotonView.Get(networkObject).viewID, "Coming from server");
       
    }

    public void PrintViaServerInstant()
    {
        PhotonView.Get(networkObject).RPC("ShowLog", PhotonTargets.AllViaServer, PhotonView.Get(networkObject).viewID, "Coming from server Instant");
        PhotonNetwork.SendOutgoingCommands();


    }

    public void PrintMsg()
    {
        PhotonView.Get(networkObject).RPC("ShowLog", PhotonTargets.All, PhotonView.Get(networkObject).viewID, "Coming direct");
    }

    

}
