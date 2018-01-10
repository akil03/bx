using System.Collections.Generic;
using System;

[Serializable]
public class Friend
{
	public string displayName;
	public string id;
	public string profilePic;
}

[Serializable]
public class FriendsListData
{
	public List<Friend> friends;
	public string requestId;
}