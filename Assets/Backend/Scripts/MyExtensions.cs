using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MyExtensions
{

	public static T Create<T>() where T : MonoBehaviour
	{
		GameObject obj = new GameObject (typeof(T).ToString ());
		T result = obj.AddComponent<T> ();
		return result;
	}

	public static GSErrorData ToGSErrorData(this string str)
	{		
		if (str.Contains (GSErrorCodes.timeout)) 
		{
			return new GSErrorData (GSErrorCodes.timeout);
		}
		if (str.Contains (GSErrorCodes.UNRECOGNISED)) 
		{
			return new GSErrorData (GSErrorCodes.UNRECOGNISED);
		}
		if (str.Contains (GSErrorCodes.LOCKED)) 
		{
			return new GSErrorData (GSErrorCodes.LOCKED);
		}
		if (str.Contains (GSErrorCodes.TAKEN)) 
		{
			return new GSErrorData (GSErrorCodes.TAKEN);
		}
		if (str.Contains (GSErrorCodes.INVALID)) 
		{
			return new GSErrorData (GSErrorCodes.INVALID);
		}
		if (str.Contains (GSErrorCodes.ACCOUNT_ALREADY_LINKED)) 
		{
			return new GSErrorData (GSErrorCodes.ACCOUNT_ALREADY_LINKED);
		}
		if (str.Contains (GSErrorCodes.NOTAUTHENTICATED)) 
		{
			return new GSErrorData (GSErrorCodes.NOTAUTHENTICATED);
		}
		if (str.Contains (GSErrorCodes.REQUIRED)) 
		{
			return new GSErrorData (GSErrorCodes.REQUIRED);
		}
		return new GSErrorData (GSErrorCodes._null);
	}

	public static List<PhotonRegion> ToRegions(this string str)
	{
		List<PhotonRegion> list = new List<PhotonRegion> ();
		string[] regionAndPing = str.Split ('|');
		foreach (var r in regionAndPing) 
		{
			PhotonRegion region = new PhotonRegion ();
			if (!string.IsNullOrEmpty (r)) 
			{
				string[] split = r.Split ('.');
				region.region = split [0];
				region.ping = int.Parse (split [1]);
				list.Add (region);		
			}
		}
		return list;
	}

	public static CloudRegionCode ToCloudRegionCode(this string str)
	{
		if (str == "asia")
			return CloudRegionCode.asia;
		else if(str == "au")
			return CloudRegionCode.au;
		else if(str == "cae")
			return CloudRegionCode.cae;
		else if(str == "cae")
			return CloudRegionCode.cae;
		else if(str == "in")
			return CloudRegionCode.@in;
		else if(str == "jp")
			return CloudRegionCode.jp;
		else if(str == "kr")
			return CloudRegionCode.kr;
		else if(str == "ru")
			return CloudRegionCode.ru;
		else if(str == "rue")
			return CloudRegionCode.rue;
		else if(str == "sa")
			return CloudRegionCode.sa;
		else if(str == "us")
			return CloudRegionCode.us;
		else if(str == "usw")
			return CloudRegionCode.usw;
		return CloudRegionCode.none;
	}
}

[Serializable]
public class PhotonRegion
{
	public string region;
	public int ping; 
}