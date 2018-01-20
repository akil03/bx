using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Core;
using DoozyUI;

public class GSUpdateMMR : MonoBehaviour
{
	public static GSUpdateMMR instance;
	public UIElement loading;

	void Start()
	{
		instance = this;
	}

	public void UpdateMMR(int score)
	{
		new LogEventRequest().SetEventKey("UpdateMMR").SetEventAttribute("MMR", score).Send((response) => {
			if (!response.HasErrors) {
				print ("Player MMR update success!");
			}
			else
			{
				print ("Player MMR update failed!");
				print (response.JSONString);
				EventManager.instance.OnUpdateMMRFailed (response.JSONString.ToGSErrorData ());
			}
			print (response.JSONString);
		});
	}
}