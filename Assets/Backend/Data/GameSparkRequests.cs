using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;

public class GameSparkRequests
{
	public void Request(string eventKey,string attributeKey,string attributeValue,StringDelegate strDelegate=null)
	{
		new LogEventRequest()
			.SetEventKey(eventKey)
			.SetEventAttribute (attributeKey,attributeValue)
		.Send((response) => {
				if(response.HasErrors)
				{
					Debug.Log (eventKey+" request failed!");
				}
				else
				{
//					Debug.Log (eventKey+" request success!");
				}
				if(strDelegate!=null)
				strDelegate(response.JSONString);
			});
	}

	public void Request(string eventKey,StringDelegate strDelegate=null)
	{
		new LogEventRequest()
			.SetEventKey(eventKey)
			.Send((response) => {
				if(response.HasErrors)
				{
					Debug.Log ("Request failed!");
				}
				else
				{
					Debug.Log ("Request success!");
				}
				if(strDelegate!=null)
					strDelegate(response.JSONString);
			});
	}
}
