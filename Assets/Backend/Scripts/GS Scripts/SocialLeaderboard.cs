using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

public class SocialLeaderboard : MonoBehaviour {

	public LeaderboardData data;
	public void Request()
	{
		new SocialLeaderboardDataRequest().SetEntryCount(100).SetLeaderboardShortCode("lb").Send((response) => {
			if(response.HasErrors)
			{
				print("Social Leaderboard request failed!");
				EventManager.instance.OnSocialLeaderboardFailed(response.JSONString.ToGSErrorData());
			}
			else
			{
				print("Social Leaderboard request success!!");
				data = JsonUtility.FromJson<LeaderboardData>(response.JSONString);
				int i=0;
				foreach(LeaderboardDataResponse._LeaderboardData entry in response.Data) {
					data.data[i].mmr = int.Parse (entry.JSONData["SUM-mmr"].ToString());
					i++;
				}
//				EventManager.instance.OnSocialLeaderboardSuccess();
			}
			print(response.JSONString);
		});;
	}
}
