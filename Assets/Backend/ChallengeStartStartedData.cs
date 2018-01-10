using System.Collections.Generic;
using System;


[Serializable]
public class ChallengeStart
{
	public List<Accepted> accepted;
	public string challengeId;
	public string challengeMessage;
	public string challengeName;
	public List<Challenged> challenged;
	public Challenger challenger;
	public CurrencyWagers currencyWagers;
	public string endDate;
	public string shortCode;
	public string startDate;
	public string state;
	public TurnCount turnCount;
}

[Serializable]
public class ChallengeStartData
{
	public ChallengeStart challenge;
	public string messageId;
	public bool notification;
	public string playerId;
	public string summary;
}