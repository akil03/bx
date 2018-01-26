using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;

public class AccountDetails : MonoBehaviour {

	public AccountDetailsData accountDetails;

	public static AccountDetails instance;

	public bool firstLoad;
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
//				print ("Getting account details success!");
				accountDetails = JsonUtility.FromJson<AccountDetailsData>(response.JSONString);
				GUIManager.instance.playerNameTxt.text = accountDetails.displayName;
				GUIManager.instance.mmrTxt.text = accountDetails.scriptData.MMR;
				if(!firstLoad){
					GUIManager.instance.Gold.text=accountDetails.currency1.ToString ();
					GUIManager.instance.Gems.text=accountDetails.currency2.ToString ();
					//GUIManager.instance.Gold=accountDetails.currency1;
					firstLoad=true;
				}
				else{
					if(GUIManager.instance.Gold.text!=accountDetails.currency1.ToString ())
						GUIManager.instance.AddCoins (int.Parse (accountDetails.currency1.ToString ())-int.Parse (GUIManager.instance.Gold.text));
					GUIManager.instance.Gems.text=accountDetails.currency2.ToString ();
				}

			}
//			print (response.JSONString);
		});
	}

	public void Set()
	{
//		GameSparkRequests req = new GameSparkRequests ();
//		req.Request (
	}
}
