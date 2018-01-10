using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using GameSparks.Api.Requests;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(FBLogin))]
[RequireComponent(typeof(FBLogout))]
[RequireComponent(typeof(ChangeTexture))]
public class FacebookLoginController : Singleton<FacebookLoginController> {

	FBLogin fblogin;
	FBLogout fbLogout;
//	public GSConnectFB connect;
//	public GSUpdateMMR mmr;
//	public GSInviteGameFriends gameFriends;
//	public GameObject LoginBtn, LogOutBtn;
	[SerializeField]FBFriendsData gameFriendsData;
	[SerializeField]GSFriendsData gsFriendsData;
	public List<AddFriendData> addfriendData;
	ChangeTexture change;
	[SerializeField]int friendsAdded;

	public static FacebookLoginController instance;

	void Awake()
	{
		fblogin = GetComponent<FBLogin> ();
		fbLogout = GetComponent<FBLogout> ();
		change = GetComponent<ChangeTexture> ();
		instance = this;
	}

	void Start()
	{
		
	}

	void OnEnable()
	{
		EventManager.instance.fbLoginSuccess += OnFbLoginSuccess;
//		EventManager.instance.fbConnectSuccess += OnFbConnectSuccess;
//		EventManager.instance.listGameFriendsRequestSuccess += GameFriends;
		EventManager.instance.gamesparksLoginSuccess += OnGameSparkLoginSuccess;
	}


	void OnDisable()
	{
		if (EventManager.instance != null) 
		{
			EventManager.instance.fbLoginSuccess -= OnFbLoginSuccess;
//			EventManager.instance.fbConnectSuccess -= OnFbConnectSuccess;
//			EventManager.instance.listGameFriendsRequestSuccess -= GameFriends;
			EventManager.instance.gamesparksLoginSuccess -= OnGameSparkLoginSuccess;
		}
	}

	public void Login()
	{
		fblogin.Login ();
	}

	public void Logout()
	{
		fbLogout.Logout ();
		change.ShowType2 ();
	}

	void OnFbLoginSuccess()
	{
		GameSparkRequests request = new GameSparkRequests();
		request.Request ("LinkFBID","FBID",fblogin.data.user_id);
		print ("fb login success");
		FacebookRequest getFriends = new FacebookRequest ();
		getFriends.Request ("me/friends",HttpMethod.GET,GetFriendsCallback);
		change.ShowType1 ();
		PlayerPrefs.SetInt ("isFB", 1);
		StartCoroutine (GetGSFriends ());
	}

	void GetFriendsCallback(IGraphResult result)
	{
		if (string.IsNullOrEmpty (result.Error)) 
		{
			gameFriendsData = JsonUtility.FromJson<FBFriendsData> (result.RawResult);
			foreach (var f in gameFriendsData.data) 
			{
				GameSparkRequests getIdRequest = new GameSparkRequests ();
				getIdRequest.Request ("GetGSFromFB","FBID",f.id,AddFBFriendCallback);
			}
		}
		else
			print (result.Error);		
	}


	IEnumerator GetGSFriends()
	{
		while (friendsAdded != gameFriendsData.data.Count && friendsAdded==0)
			yield return null;
		GetGameSparksFriends ();
	}

	public void GetGameSparksFriends()
	{
		GameSparkRequests getFriends = new GameSparkRequests ();
		getFriends.Request ("GetFriendsList",GetGSFriendsCallback);
	}

	void GetGSFriendsCallback(string str)
	{
		gsFriendsData = JsonUtility.FromJson<GSFriendsData> (str);
		EventManager.instance.OnSocialLeaderboardSuccess (gsFriendsData);
	}


	void AddFBFriendCallback(string str)
	{
		print (str);
			AddFriendData _addFriendData = JsonUtility.FromJson<AddFriendData> (str);
		if (_addFriendData.scriptData.findQueryResult.Count>0)
		{
			addfriendData.Add (_addFriendData);
			GameSparkRequests addFriendRequest = new GameSparkRequests ();
			addFriendRequest.Request ("AddFriend", "ID", _addFriendData.scriptData.findQueryResult [0].playerID, FriendAdded);
		}
	}

	void FriendAdded(string str)
	{
		print (str);
		friendsAdded++;
	}
//	public InputField GoogleID;

	public void AddGoogleFriend(InputField GoogleID)
	{
		GameSparkRequests getGSId = new GameSparkRequests ();
		getGSId.Request ("GetGSFromGoogle","GoogleID",GoogleID.text.ToLower (),GetGSIDCallback);
		GoogleID.text = "";
	}

	public List<string> iddd;
	void GetGSIDCallback(string str)
	{
		iddd = str.Split ('"').ToList();
		if (iddd.Count < 17)
			GUIManager.instance.ShowLog ("ID does not exist !!");
		else
		{
			GameSparkRequests addFriendRequest = new GameSparkRequests ();
			addFriendRequest.Request ("AddFriend", "ID", iddd[17], GoogleFrndAdded);
		}
	}

	void GoogleFrndAdded(string str)
	{
		GetGameSparksFriends ();
	}

	void OnGameSparkLoginSuccess()
	{
		GetGameSparksFriends ();
		new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 0).Send((response)=> {});
	}
}