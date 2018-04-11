using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public CloudRegionCode regionCode;
    public List<string> list;
    public Text text;
    int index = 0;

    void Start()
    {
        text.text = list[index];
        if (connectOnStart)
            ConnectToMaster();
    }

    void ConnectToMaster()
    {
        if (regionCode == CloudRegionCode.best)
        {
            PhotonNetwork.ConnectToBestCloudServer(Application.version);
        }
        else
        {
            PhotonNetwork.ConnectToRegion(regionCode, Application.version);
        }
    }

    void OnConnectedToPhoton()
    {
        print(PhotonNetwork.CloudRegion);
        connectToMasterSuccess.Fire();
        if (!saved)
        {
            SavePing();
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

    void SavePing()
    {
        if (AccountDetails.instance != null)
        {
            AccountDetails.instance.Save(PING: PhotonPingManager.ping);
        }
        saved = true;
    }

    void OnDisconnectedFromPhoton()
    {
        print("disconnected.");
        disconnectedFromPhoton.Fire();
    }

    public void Reconnect()
    {
        if (reconnect.value)
            ConnectToMaster();
    }

    public void Left()
    {
        if (index > 0)
        {
            index--;
            text.text = list[index];
            regionCode = (CloudRegionCode)index;
            StartCoroutine(m_ChangeServer());
        }
    }

    public void Right()
    {
        if (index < list.Count - 1)
        {
            index++;
            text.text = list[index];
            regionCode = (CloudRegionCode)index;
            StartCoroutine(m_ChangeServer());
        }
    }

    public void ChangeServer(int index)
    {
        regionCode = (CloudRegionCode)index;
        StartCoroutine(m_ChangeServer());
    }

    IEnumerator m_ChangeServer()
    {
        PhotonNetwork.Disconnect();
        while (!PhotonNetwork.connected)
        {
            yield return null;
        }
        ConnectToMaster();
    }
}