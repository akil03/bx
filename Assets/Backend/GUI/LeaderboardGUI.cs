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
	public string id;
	public string region;
	public Image OnlineImage;
	public bool isOnline,isInGame;
	public GSLeaderboardData leaderboardData,serverData;
	Transform parentt;
	public StringObject userId;
	public StringObject opponentId;
	public StringObject server;
	public EventObject challengeLoadingON;
	ParameterlessDelegate detailsSet;


	public void Set(string id,Transform scrollParent,ParameterlessDelegate callback)
	{
		detailsSet = callback;
		this.id = id;
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
		detailsSet ();
	}

	public void SetOpponentID()
	{
		if (id == userId.value)
			return;
		if (!PhotonNetwork.connected)
		{
			GUIManager.instance.ShowLog ("Not Connected to the server!");
			return;
		}
		if (isInGame) 
		{
			GUIManager.instance.ShowLog ("Player is in a game!");
			return;
		}
		opponentId.value = id;
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
		server.value = result [0].region;
		challengeLoadingON.Fire ();
	}

	IEnumerator LoadImage(string id)
	{
		if (string.IsNullOrEmpty (id))
			yield break;
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

		Invoke ("CheckOnline", 4);


	}
}