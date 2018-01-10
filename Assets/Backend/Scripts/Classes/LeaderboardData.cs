using System.Collections.Generic;
using System;

//[Serializable]
//public class ExternalIds
//{
//	public string FB;
//}

[Serializable]
public class UserData
{
	public string userId;
	public int mmr;
	public string when;
	public string city;
	public string country;
	public string userName;
	public ExternalIds externalIds;
	public int rank;
}

[Serializable]
public class LeaderboardData
{
	public List<UserData> data;
	public string leaderboardShortCode;
	public string requestId;
}