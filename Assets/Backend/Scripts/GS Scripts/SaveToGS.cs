using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;

public class SaveToGS : MonoBehaviour {

	public string eventKey;

	public string attributeKey;

	public void Save(string attributeValue)
	{
		new LogEventRequest().SetEventKey(eventKey).SetEventAttribute(attributeKey, attributeValue).Send((response) => {
			if (!response.HasErrors) {
				print ("Save success!");
			}
			else
			{
				print ("Save failed!");
				print (response.JSONString);
			}
			print (response.JSONString);
		});
	}
}