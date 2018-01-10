using System.Collections.Generic;
using System;
[Serializable]

public class ExternalIds
{
	public string FB;
}
[Serializable]

public class Accepted
{
	public ExternalIds externalIds;
	public string id;
	public string name;
}
[Serializable]

public class ExternalIds2
{
	public string FB;
}
[Serializable]

public class Challenged
{
	public ExternalIds2 externalIds;
	public string id;
	public string name;
}
[Serializable]

public class ExternalIds3
{
	public string FB;
}
[Serializable]

public class Challenger
{
	public ExternalIds3 externalIds;
	public string id;
	public string name;
}
[Serializable]

public class CurrencyWagers
{
}

[Serializable]

public class TurnCount
{
	public int __invalid_name__5a439c6d2f481f04b6d7ac69;
}
[Serializable]

public class Challenge
{
	public List<Accepted> accepted;
	public string challengeId;
	public string challengeName;
	public List<Challenged> challenged;
	public Challenger challenger;
	public CurrencyWagers currencyWagers;
	public string endDate;
	public string shortCode;
	public string state;
	public TurnCount turnCount;
}

[Serializable]
public class ChallengeData
{
//	public string __invalid_name__@class;
	public Challenge challenge;
	public string message;
	public string messageId;
	public bool notification;
	public string playerId;
	public string summary;
	public string who;
}