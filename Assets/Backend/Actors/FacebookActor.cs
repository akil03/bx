using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class FacebookActor : MonoBehaviour
{
    [SerializeField] EventObject fbLoginSuccess;
    [SerializeField] EventObject fbLogout;
    [SerializeField] EventObject gotFBFriends;
    [SerializeField] FbFriendsObject fbFriends;
    string key = "fbLoggedIn";

    void OnEnable()
    {
        FB.Init();
        fbFriends.Reset();
        StartCoroutine(OnInitialized());
    }


    IEnumerator OnInitialized()
    {
        while (!FB.IsInitialized)
            yield return null;
        CanLogin();
    }

    void OnDisable()
    {
        fbFriends.Reset();
    }

    public void CanLogin()
    {
        if (PlayerPrefs.GetInt(key) == 1 && !FB.IsLoggedIn)
            Login();

        if (FB.IsLoggedIn)
            fbLoginSuccess.Fire();
    }

    public void Login()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, LoginCallback);
    }

    void LoginCallback(ILoginResult result)
    {
        if (string.IsNullOrEmpty(result.Error))
        {
            if (!result.RawResult.Contains("cancelled"))
            {
                GameSparkRequests request = new GameSparkRequests();
                request.Request("LinkFBID", "FBID", AccessToken.CurrentAccessToken.UserId, Print);
                fbLoginSuccess.Fire();
                PlayerPrefs.SetInt(key, 1);
            }
        }
    }

    public void Logout()
    {
        FB.LogOut();
        fbLogout.Fire();
        fbFriends.Reset();
        PlayerPrefs.SetInt(key, 0);
        print("FB logged out!");
    }

    public void GetFriends()
    {
        FB.API("me/friends", HttpMethod.GET, GetFriendsCallback);
    }

    void GetFriendsCallback(IGraphResult result)
    {
        fbFriends.value = JsonUtility.FromJson<FBFriendsData>(result.RawResult);
        gotFBFriends.Fire();
    }

    void Print(string str)
    {
        print(str);
    }
}