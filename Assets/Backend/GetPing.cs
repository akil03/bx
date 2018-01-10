using System;

[Serializable]
public class PingScriptData
{
	public string PING;
}

[Serializable]
public class GetPingData
{
	public string requestId;
	public PingScriptData scriptData;
}