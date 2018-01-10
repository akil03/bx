using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Core;

public class GSInviteFBFriends : MonoBehaviour {

	public FriendsListData friends;

	public void Invite()
	{
		new ListInviteFriendsRequest()
			.Send((response) => {
				if(response.HasErrors)
				{
					print ("fb friends invite failed!");
					EventManager.instance.OnListFBFriendsRequestFailed (response.JSONString.ToGSErrorData ());
				}
				else
				{
					friends = JsonUtility.FromJson<FriendsListData>(response.JSONString);
					print ("fb friends invite success!");
				}
				print (response.JSONString);
			});
	}

}
