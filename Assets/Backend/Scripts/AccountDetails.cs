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
                GUIManager.instance.mmrTxt.text = accountDetails.scriptData.MMR;
                if (!firstLoad)
                {
                    GUIManager.instance.Gold.text = accountDetails.currency1.ToString();
                    GUIManager.instance.Gems.text = accountDetails.currency2.ToString();
                    firstLoad = true;
                }
                else
                {
                    if (GUIManager.instance.Gold.text != accountDetails.currency1.ToString())
                        GUIManager.instance.AddCoins(int.Parse(accountDetails.currency1.ToString()) - int.Parse(GUIManager.instance.Gold.text));
                    GUIManager.instance.Gems.text = accountDetails.currency2.ToString();
                }

            }
        });
    }

    [ContextMenu("Save")]
    public void Save()
    {
        Dictionary<string, string> pair = new Dictionary<string, string>();
        pair.Add("PING", accountDetails.scriptData.PING);
        pair.Add("MMR", accountDetails.scriptData.MMR);
        pair.Add("mostAreaCovered", accountDetails.scriptData.mostAreaCovered);
        pair.Add("highestTrophies", accountDetails.scriptData.highestTrophies);
        pair.Add("totalKills", accountDetails.scriptData.totalKills);
        pair.Add("totalDeaths", accountDetails.scriptData.totalDeaths);
        pair.Add("totalWins", accountDetails.scriptData.totalWins);
        pair.Add("totalLoss", accountDetails.scriptData.totalLoss);
        pair.Add("slot1", accountDetails.scriptData.slot1);
        pair.Add("slot2", accountDetails.scriptData.slot2);
        pair.Add("slot3", accountDetails.scriptData.slot3);
        pair.Add("slot4", accountDetails.scriptData.slot4);
        GameSparkRequests saveRequest = new GameSparkRequests();
        saveRequest.Request("SaveDetails", pair, SaveSuccess);
    }

    private void SaveSuccess(string str)
    {
        print(str);
        SetUI();
    }

    void SetUI()
    {
        GUIManager.instance.mmrTxt.text = accountDetails.scriptData.MMR;
        GUIManager.instance.mostAreaCoveredTxt.text = accountDetails.scriptData.mostAreaCovered + " %";
        GUIManager.instance.highestTrophiesTxt.text = accountDetails.scriptData.highestTrophies;
        GUIManager.instance.totalKillsTxt.text = accountDetails.scriptData.totalKills;
        GUIManager.instance.totalDeathsTxt.text = accountDetails.scriptData.totalDeaths;
        GUIManager.instance.totalWinsTxt.text = accountDetails.scriptData.totalWins;
        GUIManager.instance.totalLossTxt.text = accountDetails.scriptData.totalLoss;
    }
}