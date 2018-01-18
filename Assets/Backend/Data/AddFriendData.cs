using System.Collections.Generic;

//public class Id
//{
//	public string __invalid_name__$oid;
//}
using System;
[Serializable]
public class FindQueryResult
{
//	public Id _id;
	public string playerID;
	public string FBID;
}
[Serializable]
public class AddFriendScriptData
{
	public List<FindQueryResult> findQueryResult;
}

[Serializable]
public class AddFriendData
{
//	public string __invalid_name__@class;
	public AddFriendScriptData scriptData;
}