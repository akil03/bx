using GameSparks.Api.Messages;
using System.Collections.Generic;

public delegate void ParameterlessDelegate();
public delegate void FbLoginResultDelegate(FbLoginData result);
public delegate void FbFriendsListResultDelegate(FriendsListData result);
public delegate void GSErrorDataDelegate(GSErrorData error);
public delegate void GSMessageDelegate(GSMessage msg);
public delegate void createChallengeDataDelegate(CreateChallengeData data);
public delegate void StringDelegate(string str);
public delegate void GSFriendsDelegate(GSFriendsData data);
 