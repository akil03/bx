using System;

[Serializable]
public class FbLoginData
{
	public string access_token;
	public string user_id;
	public string callback_id;
	public string key_hash;
	public string permissions;
	public string expiration_timestamp;
	public string last_refresh;
	public bool opened;
	public string declined_permissions;
}