using System.Collections.Generic;
using System;

[Serializable]
public class FBFriend
{
	public string name;
	public string id;
}

//public class Cursors
//{
//	public string before;
//	public string after;
//}

//public class Paging
//{
//	public Cursors cursors;
//}
//
//public class Summary
//{
//	public int total_count;
//}

[Serializable]
public class FBFriendsData
{
	public List<FBFriend> data;
//	public Paging paging;
//	public Summary summary;
}
