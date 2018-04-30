using System.Collections;
using UnityEngine;

public class PhotonActor : MonoBehaviour
{
    [SerializeField] bool connectOnStart;
    [SerializeField] EventObject connectToMasterSuccess;
    [SerializeField] EventObject connectToMasterFailed;
    [SerializeField] EventObject roomCreated;
    [SerializeField] EventObject createRoomFailed;
    [SerializeField] EventObject joinedRoom;
    [SerializeField] EventObject joinRoomFailed;
    [SerializeField] EventObject randomJoinFailed;
    [SerializeField] EventObject playerConnected;
    [SerializeField] EventObject playerDisconnected;
    [SerializeField] EventObject leftRoom;
    [SerializeField] EventObject disconnectedFromPhoton;
    [SerializeField] BoolObject reconnect;
    bool saved;

    void Start()
    {
        if (connectOnStart && !PhotonNetwork.connectedAndReady)
            ConnectToMaster();
        else if (PhotonNetwork.connectedAndReady)
        {
            connectToMasterSuccess.Fire();
        }
    }

    void ConnectToMaster()
    {
        if (!ObliusGameManager.isFriendlyBattle)
        {
            PhotonNetwork.ConnectUsingSettings(Application.version);
        }
    }

    void OnConnectedToPhoton()
    {
        connectToMasterSuccess.Fire();
        if (!saved)
        {
            StartCoroutine(SavePing());
        }
    }

    void OnConnectionFail(DisconnectCause cause)
    {
        connectToMasterFailed.Fire();
    }

    void OnCreateRoom()
    {
        roomCreated.Fire();
    }

    void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        createRoomFailed.Fire();
    }

    void OnJoinedRoom()
    {
        joinedRoom.Fire();
    }

    void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        joinRoomFailed.Fire();
    }

    void OnPhotonRandomJoinFailed()
    {
        randomJoinFailed.Fire();
    }

    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        playerConnected.Fire();
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        playerDisconnected.Fire();
    }

    void OnLeftRoom()
    {
        leftRoom.Fire();
    }

    IEnumerator SavePing()
    {
        while (!AccountDetails.instance.firstLoad)
            yield return null;
        AccountDetails.instance.Save(PING: PhotonPingManager.ping);
        saved = true;
    }

    void OnDisconnectedFromPhoton()
    {
        disconnectedFromPhoton.Fire();
    }

    public void Reconnect()
    {
        if (!ObliusGameManager.isFriendlyBattle)
        {
            ConnectToMaster();
        }
    }
}