﻿using System.Collections.Generic;
using System;
[Serializable]
public class GSPlayerScriptData
{
	public int IsInGame;
	public string data;
	public string FBID;
	
}
[Serializable]
public class AllData
{
	public string displayName;
	public ExternalIds externalIds;
	public string id;
	public bool online;
	public GSPlayerScriptData scriptData;
}
[Serializable]
public class GSLeaderboardScriptData
{
	public AllData AllData;
}

[Serializable]
public class GSLeaderboardData
{
//	public string __invalid_name__@class;
	public string requestId;
	public GSLeaderboardScriptData scriptData;
}