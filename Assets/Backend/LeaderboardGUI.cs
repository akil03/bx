using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Requests;
using System.Linq;
using GameSparks.Api.Messages;

public class LeaderboardGUI : MonoBehaviour 
{
	public Text playerName;
	public Text mmr;
	public Image sprite;
//	string id;
	public CreateChallenge challenge;
//	public UserData userData;
	public string region;
//	public static UserData u;

	public Image OnlineImage;
	public bool isOnline,isInGame;
	public GSLeaderboardData leaderboardData,serverData;
	public static GSLeaderboardData l;
//	public 
	Transform parentt;
	void Start()
	{
		l = null;

	}

	public void Set(string id,Transform scrollParent)
	{
		parentt = scrollParent;
		GameSparkRequests getPlayerDetail = new GameSparkRequests ();
		getPlayerDetail.Request ("GetPlayerDataWithID","ID",id,Callback);
		print ("id is "+id);
		transform.SetParent (scrollParent);
		transform.localRotation = Quaternion.identity;
		transform.localScale = new Vector3 (1,1,0);
		transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,0);
		CheckOnline ();
	}

	void Callback(string str)
	{
		print (str);
		leaderboardData = JsonUtility.FromJson<GSLeaderboardData> (str);
		playerName.text = leaderboardData.scriptData.AllData.displayName;
		mmr.text = leaderboardData.scriptData.AllData.scriptData.MMR.ToString ();
		StartCoroutine (LoadImage (leaderboardData.scriptData.AllData.scriptData.FBID));
		Sort ();
	}



	void Sort()
	{
		LeaderboardController.instance.comps = parentt.GetComponentsInChildren<LeaderboardGUI> ().ToList ();
		LeaderboardController.instance.comps = LeaderboardController.instance.comps.OrderByDescending (a=>a.leaderboardData.scriptData.AllData.scriptData.MMR).ToList ();
		for (int i = 0; i < LeaderboardController.instance.comps.Count; i++) 
		{
			LeaderboardController.instance.comps[i].transform.SetSiblingIndex (i);
		}
	}

	IEnumerator LoadImage(string id)
	{
		WWW www = new WWW ("http://graph.facebook.com/"+id+"/picture?width=100&height=100");
		yield return www;
		if(www.texture!=null)
		sprite.sprite = Sprite.Create (www.texture, new Rect (0, 0, 100, 100), Vector2.zero);
	}

	public void CheckOnline(){
		

		new LogEventRequest().SetEventKey("GetPlayerDataWithID").SetEventAttribute("ID", leaderboardData.scriptData.AllData.id).Send((response) =>{
			if (!response.HasErrors) {
				serverData = JsonUtility.FromJson<GSLeaderboardData> (response.JSONString);
				isOnline = serverData.scriptData.AllData.online;
//				isInGame = serverData.scriptData.AllData.scriptData.IsInGame;
				playerName.text = serverData.scriptData.AllData.displayName;
				mmr.text = serverData.scriptData.AllData.scriptData.MMR.ToString ();
				OnlineImage.gameObject.SetActive (isOnline);
//				leaderboardData = serverData;
				if(serverData.scriptData.AllData.scriptData.IsInGame==1){
					OnlineImage.color = Color.red;
					isInGame=true;
				}
				else{
					OnlineImage.color = Color.green;
					isInGame=false;
				}

			
			}
		});

//		new LogEventRequest().SetEventKey("OnlineStatus").SetEventAttribute("ID", leaderboardData.scriptData.AllData.id).Send((response) => {
//			if (!response.HasErrors) {
//				if(response.JSONString.Contains ("true"))
//					isOnline = true;
//				else
//					isOnline=false;								
//			}
//		
//		});
//		new LogEventRequest().SetEventKey("GetPlayerStatus").SetEventAttribute("ID", leaderboardData.scriptData.AllData.id).Send((response)=> {
//			if (!response.HasErrors){
//				if(response.JSONString.Contains ("\"IsInGame\":1")){
//					isInGame=true;
//					OnlineImage.color = Color.red;
//				}
//				else{
//					isInGame=false;
//					OnlineImage.color = Color.green;
//				}
//			}
//
//		});


		Invoke ("CheckOnline", 4);


	}
	public string SelectedRegion;
	public List<PhotonRegion> resu;
	public void CreateChallenge()
	{
		if (!PhotonNetwork.connected) {
			GUIManager.instance.ShowLog ("Please wait! Server not ready.");
			return;
		}
		if (leaderboardData.scriptData.AllData.id == AccountDetails.instance.accountDetails.userId||!isOnline||isInGame)
			return;
		
		var regions = serverData.scriptData.AllData.scriptData.PING.ToRegions ();
		List<PhotonRegion> myRegions = PhotonManagerAdvanced.instance.ping.ToRegions ();
		List<PhotonRegion> result = new List<PhotonRegion>();
		for(int i=0;i<regions.Count;i++)
		{
			PhotonRegion r = new PhotonRegion ();
			r.region = regions[i].region;
			r.ping = regions[i].ping+myRegions[i].ping;
			result.Add(r);
		}
		result = result.OrderBy (a => a.ping).ToList ();
		resu = result;
//		Connect (result.OrderBy (a=>a.ping).First ().region,leaderboardData);
//		PhotonManagerAdvanced.instance.selectedServer = result.OrderBy (a=>a.ping).First ().region;
		Connect (result[0].region,leaderboardData);
		PhotonManagerAdvanced.instance.selectedServer = result[0].region;

		PhotonManagerAdvanced.instance.loading.Show (true);
		ChallegeWaitGUI.instance.isBusy = true;
	}

	public void Connect(string region,GSLeaderboardData data)
	{
		LeaderboardGUI.l = data;
		if (LeaderboardGUI.l == this.leaderboardData)
		{
			InternetChecker.instance.reconnect = false;
			PhotonNetwork.Disconnect ();
			this.region = region;
		}
	}

	public void OnConnectedToMaster()
	{		
		if (LeaderboardGUI.l == this.leaderboardData && !InternetChecker.instance.reconnect) {
			print ("Connected to master!");
			print (leaderboardData.scriptData.AllData.displayName);
			PhotonNetwork.CreateRoom (leaderboardData.scriptData.AllData.id,new RoomOptions(){MaxPlayers = (byte)2,IsVisible = false},TypedLobby.Default);
		}
	}

	void OnDisconnectedFromPhoton()
	{
		print ("disconnected request");
		if (LeaderboardGUI.l == this.leaderboardData && !InternetChecker.instance.reconnect) {
//			PhotonNetwork.OverrideBestCloudServer (region.ToCloudRegionCode ());
			PhotonNetwork.ConnectToRegion (region.ToCloudRegionCode (), Application.version);

		}
	}

	void OnJoinedRoom()
	{
		if (LeaderboardGUI.l == this.leaderboardData && !InternetChecker.instance.reconnect) {
			print ("Joined room!");
			ObliusGameManager.instance.ResetGame ();
			SnakesSpawner.instance.CreateNetworkSnake (1);
			challenge.Create (leaderboardData.scriptData.AllData.id);
			LeaderboardGUI.l = null;
		}
	}

	void OnPhotonCreateRoomFailed (object[] codeAndMsg)
	{
		if (codeAndMsg [0].ToString () == "32766") 
		{
			GUIManager.instance.ShowLog ("He's in a game already!");
			ChallengeController.instance.loadingMatch.Hide (false);
			ChallegeWaitGUI.instance.isBusy = false;
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (PhotonNetwork.room.PlayerCount == 2) 
		{
			GUIManager.instance.ShowInGameGUI ();
		}
	}
}