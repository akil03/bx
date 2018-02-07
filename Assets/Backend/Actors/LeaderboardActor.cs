using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeaderboardActor : MonoBehaviour 
{
	public LeaderboardGUI player;
	public Transform parent;
	public LeaderboardObject leaderboardData;
	public List<LeaderboardGUI> players;

	void OnEnable()
	{
		leaderboardData.Reset ();
	}

	void OnDisable()
	{
		leaderboardData.Reset ();
	}

	public void Create()
	{
		foreach (var friend in leaderboardData.value.scriptData.AllData.scriptData.FriendsList) 
		{
			if (!Exists (friend)) 
			{
				LeaderboardGUI player = GameObject.Instantiate<LeaderboardGUI> (this.player);
				player.Set (friend,parent,Sort);
				players.Add (player);
			}
		}
		parent.GetComponent <RectTransform> ().sizeDelta = new Vector2 (0, 10+parent.childCount*200);
	}

	bool Exists(string id)
	{
		foreach (var player in players) 
		{
			if(player.id==id)
			return true;
		}
		return false;
	}

	void Sort()
	{
		players = players.OrderByDescending(a=>a.leaderboardData.scriptData.AllData.scriptData.MMR).ToList ();
		for (int i = 0; i < players.Count; i++) 
		{
			players[i].transform.SetSiblingIndex (i);
		}
	}

	public void ClearFriends()
	{
		foreach (var p in players)
			Destroy (p.gameObject);
		players = new List<LeaderboardGUI> ();
		leaderboardData.Reset ();
	}
}