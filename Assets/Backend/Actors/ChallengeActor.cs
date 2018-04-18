using System;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using UnityEngine;

public class ChallengeActor : MonoBehaviour
{

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
    [SerializeField] GameObject homePage;

    void Start()
    {
        challenge.Reset();
        challengeOther.Reset();
        challengeMode.value = false;
        reconnect.value = true;
    }

    void OnDisable()
    {
        challenge.Reset();
        challengeOther.Reset();
        challengeMode.value = false;
        reconnect.value = true;
    }

    public void Create()
    {
        print("creating room");
        new CreateChallengeRequest()
            .SetChallengeShortCode("play")
            .SetEndTime(DateTime.Parse(DateTime.UtcNow.AddMinutes(2).ToString("yyyy-MM-ddTHH:mmZ")))
            .SetChallengeMessage(server.value)
            .SetUsersToChallenge(new List<string>() { opponentId.value })
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    print("Create challenge failed!");
                }
                else
                {
                    cid = response.ChallengeInstanceId;
                    print(response.JSONString);
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
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    print("Withdraw challenge failed!");
                }
                else
                {
                    ObliusGameManager.isFriendlyBattle = false;
                    challenge.Reset();
                    withdrawn.Fire();
                    reconnect.value = true;
                    challengeMode.value = false;
                    print("Withdraw challenge success!");
                }
            });
    }

    public void Accept()
    {
        new AcceptChallengeRequest().SetChallengeInstanceId(challenge.data.challenge.challengeId)
            .Send((response) =>
            {
                if (response.HasErrors)
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
        ObliusGameManager.isFriendlyBattle = false;
        new DeclineChallengeRequest().SetChallengeInstanceId(challenge.data.challenge.challengeId).Send((response) =>
        {
            if (response.HasErrors)
            {
                print("Decline challenge failed!");
            }
            else
            {
                challenge.Reset();
                declined.Fire();
                reconnect.value = true;
                challengeMode.value = false;
                print("Decline challenge success!");
            }
            print(response.JSONString);
        });
    }

    public void DeclineOther()
    {
        new DeclineChallengeRequest().SetChallengeInstanceId(challengeOther.data.challenge.challengeId).Send((response) =>
        {
            if (response.HasErrors)
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

    [SerializeField] BoolObject challengeMode;
    [SerializeField] BoolObject reconnect;

    public void StartChallenge()
    {
        reconnect.value = false;
        PhotonNetwork.Disconnect();
        challengeMode.value = true;
        ObliusGameManager.isFriendlyBattle = true;

    }

    public void DisconnectedFromPhoton()
    {
        if (challengeMode.value)
        {
            if (challenge.data != null)
            {
                PhotonNetwork.ConnectToRegion(challenge.data.message.ToCloudRegionCode(), Application.version);
            }
            else
            {
                PhotonNetwork.ConnectToRegion(server.value.ToCloudRegionCode(), Application.version);
            }
        }
        else
        {
            challenge.data = null;
        }
    }

    public void ConnectedToMaster()
    {
        if (challengeMode.value)
        {
            if (challenge.data != null)
            {
                PhotonNetwork.JoinRoom(challenge.data.challenge.challenged[0].id);
            }
            else
            {
                PhotonNetwork.CreateRoom(opponentId.value, new RoomOptions() { MaxPlayers = (byte)2, IsVisible = false }, TypedLobby.Default);
            }
            print("Connected to master!");
        }
    }

    public void JoinedRoom()
    {
        if (challengeMode.value)
        {
            if (challenge.data != null)
            {
                ObliusGameManager.instance.ResetGame();
                SnakesSpawner.instance.CreateNetworkSnake(2);
                GUIManager.instance.ShowInGameGUI();
                challenge.Reset();
                print("Joined room!");

            }
            else
            {
                ObliusGameManager.instance.ResetGame();
                SnakesSpawner.instance.CreateNetworkSnake(1);
                Create();
                challenge.Reset();
                print("Joined room!");

            }
        }
    }

    public void PlayerConnected()
    {
        if (challengeMode.value && PhotonNetwork.room.PlayerCount == 2)
        {
            accepted.Fire();
            challenge.Reset();
            occupied.value = false;
            GUIManager.instance.ShowInGameGUI();
            print(PhotonNetwork.player.ID + " has connected.");
        }
    }

    public void Reconnect()
    {
        challenge.data = new ChallengeData();
    }

    public void CanAcceptChallenge()
    {
        if (occupied.value)
            DeclineOther();
    }

    public void SetOccupiedStatus(bool val)
    {
        occupied.value = val;
    }


}