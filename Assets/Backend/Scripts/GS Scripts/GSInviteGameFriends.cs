using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Core;

public class GSInviteGameFriends : MonoBehaviour {

	public FriendsListData friends;

	public void Invite()
	{
		new ListGameFriendsRequest()
			.Send((response) => {
				if(response.HasErrors)
				{
					print ("Invite game friends failed!");
					EventManager.instance.OnListGameFriendsRequestFailed (response.JSONString.ToGSErrorData ());
				}
				else
				{
					friends = JsonUtility.FromJson<FriendsListData>(response.JSONString);
					print ("Invite game friends success!");
					EventManager.instance.OnListGameFriendsRequestSuccess();
				}
				print (response.JSONString);
			});
	}
}
