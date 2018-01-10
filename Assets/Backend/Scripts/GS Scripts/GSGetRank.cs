using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Core;
using GameSparks.Api.Responses;

public class GSGetRank : MonoBehaviour {

	public PlayerRankData data;

	public void Get(string userId)
	{
		new GetLeaderboardEntriesRequest().SetPlayer(userId)
			.SetLeaderboards(new List<string>(){"lb"})
			.Send((response) => {
				if (!response.HasErrors)
				{
					print ("Get player rank success!");
					data = JsonUtility.FromJson<PlayerRankData>(response.JSONString);
					foreach(var v in response.BaseData.BaseData)
					{
						if(v.Key=="lb")
						{
							GSData gs = v.Value as GSData;
							foreach(var w in gs.BaseData)
							{
								if(w.Key=="SUM-mmr")
									data.lb.mmr = int.Parse (w.Value.ToString ());
							}
						}
					}

				}
				else
				{
					print ("Get player rank failed!");
					EventManager.instance.OnGetRankFailed (response.JSONString.ToGSErrorData ());
				}
				print (response.JSONString);
			});
	}

}
