using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookRequest
{
	public void Request(string query,HttpMethod method,FacebookDelegate<IGraphResult> callback=null)
	{
		FB.API (query,method,callback);
	}
}
