using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeaderboardController : Singleton<LeaderboardController>
{

//	public SocialLeaderboard socialLeaderboard;
//	public GSInviteGameFriends gameFriends;
	public Transform scrollParent;
	public LeaderboardGUI player;
	public List<LeaderboardGUI> players;
	public List<LeaderboardGUI> comps;
	public static LeaderboardController instance;

	void Awake()
	{
		instance = this;
	}

	void OnEnable()
	{
		EventManager.instance.socialLeaderboardSuccess += GotLeaderboardData;
//		EventManager.instance.gamesparksLoginSuccess += Get;

	}

	void OnDisable()
	{
		if (EventManager.instance != null) 
		{
			EventManager.instance.socialLeaderboardSuccess -= GotLeaderboardData;
//			EventManager.instance.gamesparksLoginSuccess -= Get;
		}
	}

//	public void Get()
//	{
//		gameFriends.Invite ();
//		socialLeaderboard.Request();
//		GotLeaderboardData ();
//	}

	void GotLeaderboardData(GSFriendsData data)
	{
		if (players.Count == data.scriptData.FriendsList.Count)
			return;
		else
		{
			if(players.Count==0)
			{
				foreach(var f in data.scriptData.FriendsList) 
				{
					LeaderboardGUI _player = GameObject.Instantiate<LeaderboardGUI> (player);
					players.Add (_player);
					_player.Set (f,scrollParent);

				}
			}
			else
			{
				LeaderboardGUI _player = GameObject.Instantiate<LeaderboardGUI> (player);
				players.Add (_player);
				_player.Set (data.scriptData.FriendsList[data.scriptData.FriendsList.Count-1],scrollParent);
				GUIManager.instance.ShowLog ("Friend added !");
			}
			scrollParent.GetComponent <RectTransform> ().sizeDelta = new Vector2 (0, scrollParent.childCount*150);

		}
	}

}
