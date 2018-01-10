using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;



public class GetLeaderboardData : MonoBehaviour {

	public LeaderboardData data;

	public void Get()
	{
		new LeaderboardDataRequest().SetLeaderboardShortCode("lb").SetEntryCount(10).Send((response) => {
			if (!response.HasErrors) {
				print ("Getting Leaderboard data success!");
				data = JsonUtility.FromJson<LeaderboardData>(response.JSONString);
				int i=0;
				foreach(LeaderboardDataResponse._LeaderboardData entry in response.Data) {
					data.data[i].mmr = int.Parse (entry.JSONData["SUM-mmr"].ToString());
					i++;
				}
				EventManager.instance.OnGetLeaderboardDataSuccess();
			} else {
				print ("Getting Leaderboard data failed!");
				EventManager.instance.OnGetLeaderboardDataFailed (response.JSONString.ToGSErrorData ());
			}
			print (response.JSONString);
		});
	}
}
