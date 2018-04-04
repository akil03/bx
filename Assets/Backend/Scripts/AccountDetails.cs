using System.Collections.Generic;
using GameSparks.Api.Requests;
using UnityEngine;

public class AccountDetails : MonoBehaviour
{

    public AccountDetailsData accountDetails;

    public static AccountDetails instance;

    public bool firstLoad;

    void Awake()
    {
        instance = this;
    }

    public void Get()
    {
        new AccountDetailsRequest().Send((response) =>
        {
            if (response.HasErrors)
            {
                print("Getting account details failed!");
            }
            else
            {
                accountDetails = JsonUtility.FromJson<AccountDetailsData>(response.JSONString);
                GUIManager.instance.playerNameTxt.text = accountDetails.displayName;

                if (!isLoaded)
                    SetUI();
                              
            }
        });
    }

    [ContextMenu("Save")]
    public void Save(int Gold = 0, int Gem = 0, string PING = null, int MMR = 0, float mostAreaCovered = 0f, int highestTrophies = 0, int totalKills = 0, int totalDeaths = 0, int totalWins = 0, int totalLoss = 0, string slot1 = null, string slot2 = null, string slot3 = null, string slot4 = null)
    {        
        Dictionary<string, object> pair = new Dictionary<string, object>();
        accountDetails.scriptData.Gold += Gold;
        pair.Add("Gold", accountDetails.scriptData.Gold);
        accountDetails.scriptData.Gem += Gem;
        pair.Add("Gem", accountDetails.scriptData.Gem);
        accountDetails.scriptData.PING = string.IsNullOrEmpty(PING) ? accountDetails.scriptData.PING : PING;
        pair.Add("PING", accountDetails.scriptData.PING);
        accountDetails.scriptData.MMR += MMR;
        pair.Add("MMR", accountDetails.scriptData.MMR);
        accountDetails.scriptData.mostAreaCovered = accountDetails.scriptData.mostAreaCovered < mostAreaCovered ? mostAreaCovered : accountDetails.scriptData.mostAreaCovered;
        pair.Add("mostAreaCovered", accountDetails.scriptData.mostAreaCovered);
        accountDetails.scriptData.highestTrophies = accountDetails.scriptData.highestTrophies < highestTrophies ? highestTrophies : accountDetails.scriptData.highestTrophies;
        pair.Add("highestTrophies", accountDetails.scriptData.highestTrophies);
        accountDetails.scriptData.totalKills += totalKills;
        pair.Add("totalKills", accountDetails.scriptData.totalKills);
        accountDetails.scriptData.totalDeaths += totalDeaths;
        pair.Add("totalDeaths", accountDetails.scriptData.totalDeaths);
        accountDetails.scriptData.totalWins += totalWins;
        pair.Add("totalWins", accountDetails.scriptData.totalWins);
        accountDetails.scriptData.totalLoss += totalLoss;
        pair.Add("totalLoss", accountDetails.scriptData.totalLoss+totalLoss);
        accountDetails.scriptData.slot1 = string.IsNullOrEmpty(slot1) ? accountDetails.scriptData.slot1 : slot1;
        pair.Add("slot1", accountDetails.scriptData.slot1);
        accountDetails.scriptData.slot2 = string.IsNullOrEmpty(slot2) ? accountDetails.scriptData.slot2 : slot2;
        pair.Add("slot2", accountDetails.scriptData.slot2);
        accountDetails.scriptData.slot3 = string.IsNullOrEmpty(slot3) ? accountDetails.scriptData.slot3 : slot3;
        pair.Add("slot3", accountDetails.scriptData.slot3);
        accountDetails.scriptData.slot4 = string.IsNullOrEmpty(slot4) ? accountDetails.scriptData.slot4 : slot4;
        pair.Add("slot4", accountDetails.scriptData.slot4);
        GameSparkRequests saveRequest = new GameSparkRequests();
        saveRequest.Request("SaveDetails", pair, SaveSuccess);
    }

    private void SaveSuccess(string str)
    {
        print(str);
        SetUI();
    }

    bool isLoaded;
    void SetUI()
    {
        if (isLoaded)
        {
            if (GUIManager.instance.Gold.text != accountDetails.scriptData.Gold.ToString())
                GUIManager.instance.AddCoins(int.Parse(accountDetails.scriptData.Gold.ToString()) - int.Parse(GUIManager.instance.Gold.text));
        }
        else
        {
            GUIManager.instance.Gold.text = accountDetails.scriptData.Gold.ToString();
           
        }
        GUIManager.instance.playerNameTxt.text = accountDetails.displayName;
        GUIManager.instance.Gems.text = accountDetails.scriptData.Gem.ToString();
        GUIManager.instance.mmrTxt.text = accountDetails.scriptData.MMR.ToString();
        GUIManager.instance.mostAreaCoveredTxt.text = accountDetails.scriptData.mostAreaCovered + " %";
        GUIManager.instance.highestTrophiesTxt.text = accountDetails.scriptData.highestTrophies.ToString();
        GUIManager.instance.totalKillsTxt.text = accountDetails.scriptData.totalKills.ToString();
        GUIManager.instance.totalDeathsTxt.text = accountDetails.scriptData.totalDeaths.ToString();
        GUIManager.instance.totalWinsTxt.text = accountDetails.scriptData.totalWins.ToString();
        GUIManager.instance.totalLossTxt.text = accountDetails.scriptData.totalLoss.ToString();
        isLoaded = true;
    }
}