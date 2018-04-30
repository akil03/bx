using System;
using System.Collections;
using EasyMobile;
using UnityEngine;

public class GoogleActor : MonoBehaviour
{
    [SerializeField] EventObject googleLoginSuccess;
    [SerializeField] EventObject googleLogout;
    [SerializeField] StringObject email;
    [SerializeField] StringObject userName;
    string key = "googleLoggedIn";

    void Start()
    {

        if (!Application.isEditor)
        {
            if (Application.platform == RuntimePlatform.Android)
                StartCoroutine(GooglePlayLogin());
            else
                StartCoroutine(SocialLoginWait());
        }

    }

    IEnumerator GooglePlayLogin()
    {
        if (!GameServices.IsInitialized())
        {
            GameServices.Init();
        }
        while (!GameServices.IsInitialized())
            yield return null;
        if (!Social.localUser.authenticated)
        {
            StartCoroutine(SocialLoginWait());
        }
        else
        {
            AfterSocialLoginSuccess();
        }
    }

    private void AfterSocialLoginSuccess()
    {
        email.value = Social.localUser.id;
        userName.value = Social.localUser.userName;
        googleLoginSuccess.Fire();
    }

    void GameCenterLogin()
    {


    }


    void SocialLogin()
    {
        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                GUIManager.instance.splashScreen.Load(0.75f);
                AfterSocialLoginSuccess();
            }
            else
                Debug.Log("Failed to authenticate");
        });


    }

    IEnumerator SocialLoginWait()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success =>
            {

            });
            while (!Social.localUser.authenticated)
                yield return null;
            GUIManager.instance.splashScreen.Load(0.75f);

            AfterSocialLoginSuccess();
        }
        else
        {
            AfterSocialLoginSuccess();
        }

    }

    public void Login(bool status)
    {
        Action<bool> logInCallBack = (Action<bool>)((loggedIn) =>
        {
            if (loggedIn)
            {
                PlayerPrefs.SetInt(key, 1);
                googleLoginSuccess.Fire();
            }

            else
            {
                print("Google Login Failed");
                Application.Quit();
            }
        });
    }


    public void Logout()
    {
        googleLogout.Fire();
        PlayerPrefs.SetInt(key, 0);
    }
}