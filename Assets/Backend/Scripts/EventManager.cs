using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Messages;

public class EventManager : Singleton<EventManager>
{
		
	public event ParameterlessDelegate googleLoginSuccess;
	public event ParameterlessDelegate googleLoginFailed;
	public event ParameterlessDelegate googleLogout;
	public event ParameterlessDelegate gamesparksLoginSuccess;
	public event GSErrorDataDelegate gamesparksLoginFailed;
	public event ParameterlessDelegate gamesparksLogoutSuccess;
	public event ParameterlessDelegate gamesparksRegistrationSuccess;
	public event GSErrorDataDelegate gamesparksRegistrationFailed;
	public event FbLoginResultDelegate fbLogin;
	public event ParameterlessDelegate fbLogout;
	public event FbFriendsListResultDelegate fbFriendsList;
	public event GSErrorDataDelegate updateMMRFailed;
	public event ParameterlessDelegate listGameFriendsRequestSuccess; 
	public event GSErrorDataDelegate listGameFriendsRequestFailed; 
	public event GSErrorDataDelegate listFBFriendsRequest;
	public event GSErrorDataDelegate getRankFailed;
	public event GSErrorDataDelegate fbConnectFailed;
	public event ParameterlessDelegate fbConnectSuccess;
	public event ParameterlessDelegate getLeaderboardDataSuccess;
	public event GSErrorDataDelegate getLeaderboardDataFailed;
	public event ParameterlessDelegate fbLoginSuccess; 
	public event GSFriendsDelegate socialLeaderboardSuccess;
	public event GSErrorDataDelegate socialLeaderboardFailed;
	public event createChallengeDataDelegate createChallengeSuccess;
	public event GSErrorDataDelegate createChallengeFailed;
	public event GSMessageDelegate recievedChallenge;
	public event GSMessageDelegate declinedChallenge;
	public event GSMessageDelegate acceptedChallenge;
	public event GSMessageDelegate challengeStarted;
	public event ParameterlessDelegate withdrawChallenge;
	public static EventManager instance; 

	void Awake()
	{
		instance = this;
	}

	public void OnGoogleLoginSuccess()
	{
		if (googleLoginSuccess != null) {
			googleLoginSuccess ();
			print ("On google login success event fired!");
		}
	}

	public void OnGoogleLoginFailed()
	{
		if (googleLoginFailed != null) {
			googleLoginFailed ();
			print ("On google login failed event fired!");
		}
	}

	public void OnGoogleLogout()
	{
		if (googleLogout != null) {
			googleLogout ();
			print ("On google logout event fired!");
		}
	}

	public void OnGSLoginSuccess()
	{
		if (gamesparksLoginSuccess != null) {
			gamesparksLoginSuccess ();
			print ("On GS login success event fired!");
		}
	}

	public void OnGSLoginFailed(GSErrorData error)
	{
		if (gamesparksLoginFailed != null) {
			gamesparksLoginFailed (error);
			print ("On GS login failed event fired!");
			print (error.error);
		}
	}

	public void OnGSLogoutSuccess()
	{
		if (gamesparksLogoutSuccess != null) {
			gamesparksLogoutSuccess ();
			print ("On GS logout success event fired!");
		}
	}


	public void OnGSRegistrationSuccess()
	{
		if (gamesparksRegistrationSuccess != null) {
			gamesparksRegistrationSuccess ();
			print ("On GS registration success event fired!");
		}
	}

	public void OnGSRegistrationFailed(GSErrorData error)
	{
		if (gamesparksRegistrationFailed != null) {
			gamesparksRegistrationFailed (error);
			print ("On GS registration failed event fired!");
			print (error.error);
		}
	}

	public void OnFbLogout()
	{
		if (fbLogout != null) {
			fbLogout ();
			print ("FB logged out event fired!");
		}
	}

	public void OnFbLogin(FbLoginData result)
	{
		if (fbLogin != null) {
			fbLogin (result);
			print ("FB logged in event fired!");
		}
	}

	public void OnFbFriendsList(FriendsListData result)
	{
		if (fbFriendsList != null) {
			fbFriendsList (result);
			print ("Got FB friends list event fired!");
		}
	}

	public void OnUpdateMMRFailed(GSErrorData error)
	{
		if (updateMMRFailed != null) {
			updateMMRFailed (error);
			print ("UpdateMMR failed event fired!");
		}
	}

	public void OnListGameFriendsRequestSuccess()
	{
		if (listGameFriendsRequestSuccess != null) {
			listGameFriendsRequestSuccess ();
			print ("List game friends success event fired!");
		}
	}

	public void OnListGameFriendsRequestFailed(GSErrorData error)
	{
		if (listGameFriendsRequestFailed != null) {
			listGameFriendsRequestFailed (error);
			print ("List game friends failed event fired!");
		}
	}

	public void OnListFBFriendsRequestFailed(GSErrorData error)
	{
		if (listFBFriendsRequest != null) {
			listFBFriendsRequest (error);
			print ("List FB friends failed event fired!");
		}
	}

	public void OnGetRankFailed(GSErrorData error)
	{
		if (getRankFailed != null) {
			getRankFailed (error);
			print ("Get rank failed event fired!");
		}
	}

	public void OnFBConnectFailed(GSErrorData error)
	{
		if (fbConnectFailed != null) {
			fbConnectFailed (error);
			print ("FB connect failed event fired!");
		}
	}

	public void OnFBConnectSuccess()
	{
		if (fbConnectSuccess != null) {
			fbConnectSuccess ();
			print ("FB connect success event fired!");
		}
	}

	public void OnGetLeaderboardDataFailed(GSErrorData error)
	{
		if (getLeaderboardDataFailed != null) {
			getLeaderboardDataFailed (error);
			print ("Get leaderboard data failed event fired!");
		}
	}

	public void OnGetLeaderboardDataSuccess()
	{		
		if (getLeaderboardDataSuccess != null) {
			getLeaderboardDataSuccess ();
			print ("Get leaderboard data success event fired!");
		}
	}

	public void OnFbLoginSuccess()
	{
		if (fbLoginSuccess != null) {
			fbLoginSuccess ();
			print ("Fb login success event fired!");
		}
	}

	public void OnSocialLeaderboardSuccess(GSFriendsData data)
	{
		if (socialLeaderboardSuccess != null) {
			socialLeaderboardSuccess (data);
			print ("Social leaderboard success event fired!");
		}
	}

	public void OnSocialLeaderboardFailed(GSErrorData error)
	{
		if (socialLeaderboardFailed != null) {
			socialLeaderboardFailed (error);
			print ("Social leaderboard failed event fired!");
		}
	}

	public void OnCreateChallengeSuccess(CreateChallengeData msg)
	{
		if (createChallengeSuccess != null) {
			createChallengeSuccess (msg);
			print ("Create challenge success event fired!");
		}
	}

	public void OnCreateChallengeFailed(GSErrorData error)
	{
		if (createChallengeFailed != null) {
			createChallengeFailed (error);
			print ("Create challenge failed event fired!");
		}
	}

	public void OnRecievedChallenge(GSMessage msg)
	{
		if (recievedChallenge != null) {
			recievedChallenge (msg);
			print ("On recieved challenge event fired!");
		}
	}

	public void OnDeclinedChallenge(GSMessage msg)
	{
		if (declinedChallenge != null) {
			declinedChallenge (msg);
			print ("Decline challenge event fired!");
		}
	}

	public void OnAcceptedChallenge(GSMessage msg)
	{
		if (acceptedChallenge != null) {
			acceptedChallenge (msg);
			print ("Accepted challenge event fired!");
		}
	}

	public void OnWithdrawChallenge()
	{
		if (withdrawChallenge != null) {
			withdrawChallenge ();
			print ("Withdraw challenge event fired!");
		}
	}

	public void OnChallengeStarted(GSMessage msg)
	{
		if (challengeStarted != null) {
			challengeStarted (msg);
			ChallegeWaitGUI.instance.isBusy = false;
			print ("Accepted challenge event fired!");
		}
	}
}