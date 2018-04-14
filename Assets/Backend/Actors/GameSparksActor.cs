﻿using System.Collections.Generic;
using System.Linq;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSparksActor : MonoBehaviour
{
    [SerializeField] StringObject email;
    [SerializeField] StringObject userName;
    [SerializeField] StringObject userId;
    [SerializeField] EventObject loginFailed;
    [SerializeField] EventObject loginSuccess;
    [SerializeField] EventObject logoutSuccess;
    [SerializeField] EventObject registrationSuccess;
    [SerializeField] EventObject gotGSFriends;
    [SerializeField] LeaderboardObject leaderboardData;
    [SerializeField] FbFriendsObject fbFriends;
    [SerializeField] List<bool> friendsAdded;
    [SerializeField] BoolObject isOnline;

    void Awake()
    {
        if (Application.isEditor)
        {
            Login();
        }
        else
        {
            email.Reset();
        }
    }

    private void MatchNotFound(MatchNotFoundMessage obj)
    {
        print(obj.Errors.BaseData);
    }

    public void Login()
    {
        new AuthenticationRequest().SetUserName(email.value).SetPassword(email.value).Send((AR) =>
    {
        if (AR.HasErrors)
        {
            if (AR.JSONString.Contains("UNRECOGNISED"))
                loginFailed.Fire();
            else
            {
                ReloadScene();
            }
            print(AR.Errors.JSON);
        }
        else
        {
            isOnline.value = true;
            userId.value = AR.UserId;
            loginSuccess.Fire();
            //				print("Game Sparks login success!!");
        }
        //			print (AR.JSONString);
    });
    }

    public void Logout()
    {
        GS.Reset();
        GS.Disconnect();
        logoutSuccess.Fire();
        leaderboardData.Reset();
        fbFriends.Reset();
        email.Reset();
        userId.Reset();
        userName.Reset();
        print("Game Sparks logged out!!");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Register()
    {
        new RegistrationRequest()
            .SetDisplayName(userName.value)
            .SetPassword(email.value)
            .SetUserName(email.value)
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    print("Game Sparks Registration failed!");
                }
                else
                {
                    registrationSuccess.Fire();
                    print("Game Sparks Registration success!");
                }
                print(response.JSONString);
            });
    }

    public void AssignPlayerData()
    {
        new LogEventRequest().SetEventKey("GetPlayerDataWithID").SetEventAttribute("ID", userId.value).Send((response) =>
        {
            if (!response.HasErrors)
            {
                leaderboardData.value = JsonUtility.FromJson<GSLeaderboardData>(response.JSONString);
                gotGSFriends.Fire();
            }
        });
    }

    public void GetFriendsCallback()
    {
        foreach (var f in fbFriends.value.data)
        {
            GameSparkRequests getIdRequest = new GameSparkRequests();
            getIdRequest.Request("GetGSFromFB", "FBID", f.id, AddFBFriendCallback);
        }
    }

    void AddFBFriendCallback(string str)
    {
        //print (str);
        AddFriendData _addFriendData = JsonUtility.FromJson<AddFriendData>(str);
        if (_addFriendData.scriptData.findQueryResult.Count > 0)
        {
            //GameSparkRequests addFriendRequest = new GameSparkRequests();
            //addFriendRequest.Request("AddFriend", "ID", _addFriendData.scriptData.findQueryResult[0].playerID, FriendAdded);
            AccountDetails.instance.Save(friendID: _addFriendData.scriptData.findQueryResult[0].playerID);
        }
        else
        {
            friendsAdded.Add(false);
            //print (str);
        }
    }

    void FriendAdded(string str)
    {
        friendsAdded.Add(true);
        if (friendsAdded.Count == fbFriends.value.data.Count)
            AssignPlayerData();
        print(str);
    }

    public void AddGoogleFriend(InputField GoogleID)
    {
        GameSparkRequests getGSId = new GameSparkRequests();
        getGSId.Request("GetGSFromGoogle", "GoogleID", GoogleID.text.ToLower(), GetGSIDCallback);
        GoogleID.text = "";
    }

    void GetGSIDCallback(string str)
    {
        List<string> strSplit = str.Split('"').ToList();
        if (strSplit.Count < 17)
            GUIManager.instance.ShowLog("ID does not exist !!");
        else
        {
            //GameSparkRequests addFriendRequest = new GameSparkRequests();
            //addFriendRequest.Request("AddFriend", "ID", strSplit[17], GoogleFrndAdded);
            AccountDetails.instance.Save(friendID: strSplit[17]);
        }
        
    }

    void GoogleFrndAdded(string str)
    {
        AssignPlayerData();
    }

    public void SetOnlineStatus(int id)
    {
        if (id == 1)
        {
            isOnline.value = false;
        }
        else
        {
            isOnline.value = true;
        }
        new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", id).Send((response) => { });
    }

    void OnApplicationFocus(bool isFocused)
    {
        if (!Application.isEditor)
        {
            if (!isFocused)
            {
                SetOnlineStatus(1);
            }
            else
            {
                SetOnlineStatus(0);
            }
        }
    }

    public void ChangePlayerStatus()
    {
        if (isOnline.value)
        {
            SetOnlineStatus(0);
        }
        else
        {
            SetOnlineStatus(1);
        }
    }

    public void FindPlayers()
    {
        if (!GS.Authenticated)
        {
            print("Not logged in!");
            return;
        }
        Debug.Log("GSM| Attempting Matchmaking...");
        new GameSparks.Api.Requests.MatchmakingRequest()
            .SetMatchShortCode("normal")
            .SetSkill(0)
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    Debug.LogError("GSM| MatchMaking Error \n" + response.Errors.JSON);
                }
            });
    }

    public void StopFinding()
    {
        if (!GS.Authenticated)
        {
            print("Not logged in!");
            return;
        }
        new GameSparks.Api.Requests.MatchmakingRequest().SetAction("cancel").SetMatchShortCode("normal").Send((response) =>
        {
            if (response.HasErrors)
            {
                print(response.Errors.JSON);
            }
            else
            {
                ObliusGameManager.instance.CancelFinding();
            }
        });
    }
}