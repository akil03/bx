using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Core;

public class GSLogin : MonoBehaviour {
	public GSLeaderboardData playerData;
	public string playerID;
	public static GSLogin instance;

	void Awake(){
		instance = this;
	}

	public void Login(string str)
	{
		string[] info = str.Split ('.');  
		Login (info[0],info[1]);
	}

	public void Login(string userName,string password)
	{
		new AuthenticationRequest ().SetUserName (userName).SetPassword (password).Send ((AR) => {

			if (AR.HasErrors) {
				print("GS login failed!!");
				EventManager.instance.OnGSLoginFailed (AR.JSONString.ToGSErrorData ());
				
			} else {
				print("GS login success!!");
				EventManager.instance.OnGSLoginSuccess ();
				#if !UNITY_EDITOR
				if(PlayerPrefs.GetInt ("isFB")==1)
					FacebookLoginController.instance.Login ();
				#endif
				playerID = AR.UserId;
				AssignPlayerData();
			}
			print (AR.JSONString);
		});
	}

	public void ChangeGSName(string newName){
		PlayerPrefs.SetString ("PlayerName",newName);
		new ChangeUserDetailsRequest()
			.SetDisplayName(newName)
			.Send((response) => {
				GSData scriptData = response.ScriptData; 
			});
	}


	public void AssignPlayerData(){
		new LogEventRequest ().SetEventKey ("GetPlayerDataWithID").SetEventAttribute ("ID",playerID ).Send ((response) => {
			if (!response.HasErrors) {
				playerData = JsonUtility.FromJson<GSLeaderboardData> (response.JSONString);
				GUIManager.instance.mmrTxt.text = playerData.scriptData.AllData.scriptData.MMR.ToString ();
				PlayerPrefs.SetInt ("MMR",playerData.scriptData.AllData.scriptData.MMR);
				//print(playerData.scriptData.AllData.scriptData.MMR.ToString ());
			}
		});
	}
}


