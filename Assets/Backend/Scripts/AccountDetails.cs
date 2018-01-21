using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;

public class AccountDetails : MonoBehaviour {

	public AccountDetailsData accountDetails;

	public static AccountDetails instance;

	void Start()
	{
		instance = this;
	}

	public void Get()
	{
		new AccountDetailsRequest ().Send ((response)=>{
			if(response.HasErrors)
			{
				print ("Getting account details failed!");
			}
			else
			{
				print ("Getting account details success!");
				accountDetails = JsonUtility.FromJson<AccountDetailsData>(response.JSONString);
				GUIManager.instance.playerNameTxt.text = accountDetails.displayName;
				GUIManager.instance.mmrTxt.text = accountDetails.scriptData.MMR;
				GUIManager.instance.Gold.text=accountDetails.currency1.ToString ();
				GUIManager.instance.Gems.text=accountDetails.currency2.ToString ();
				//GUIManager.instance.Gold=accountDetails.currency1;
			}
			print (response.JSONString);
		});
	}

	public void Set()
	{
//		GameSparkRequests req = new GameSparkRequests ();
//		req.Request (
	}
}
