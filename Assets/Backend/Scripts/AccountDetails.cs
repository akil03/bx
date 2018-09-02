using System.Collections.Generic;
using GameSparks.Api.Requests;
using UnityEngine;

public class AccountDetails : MonoBehaviour
{
    public AccountDetailsData accountDetails;

    public PlayerData playerData = new PlayerData();

    public static AccountDetails instance;

    public bool firstLoad;
    public LeaderboardActor leaderboardActor;
    string tempId;

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
                playerData = JsonUtility.FromJson<PlayerData>(accountDetails.scriptData.data);
                if (playerData == null)
                {
                    playerData = new PlayerData();
                }
                playerData.displayName = accountDetails.displayName;
                if (!firstLoad)
                {
                    Save(Gold: 50000);
                    SetUI();                   
                    UpgradeItem[] upgradeItems = FindObjectsOfType<UpgradeItem>();
                    foreach (var item in upgradeItems)
                    {
                        item.AssignValues();
                    }
                }

            }
        });
    }

    public void Save(int Gold = 0, int Gem = 0, string PING = null, int MMR = 0, float mostAreaCovered = 0f, int totalKills = 0, int totalDeaths = 0, int totalWins = 0, int totalLoss = 0, string slot1 = "0", string slot2 = "0", string slot3 = "0", string slot4 = "0", int rocket = 0, int minishots = 0, int heal = 0, int speed = 0, int freeze = 0, int shield = 0, int lives = 0, int health = 0, int movespeed = 0, string friendID = null)
    {
        if (!firstLoad)
        {
            return;
        }
        Dictionary<string, object> pair = new Dictionary<string, object>();
        playerData.Gold += Gold;
        playerData.Gem += Gem;
        playerData.PING = string.IsNullOrEmpty(PING) ? playerData.PING : PING;
        playerData.MMR += MMR;
        playerData.MMR = playerData.MMR < 0 ? 0 : playerData.MMR;
        playerData.mostAreaCovered = playerData.mostAreaCovered < mostAreaCovered ? mostAreaCovered : playerData.mostAreaCovered;
        playerData.highestTrophies = playerData.highestTrophies > playerData.MMR ? playerData.highestTrophies : playerData.MMR;
        playerData.totalKills += totalKills;
        playerData.totalDeaths += totalDeaths;
        playerData.totalWins += totalWins;
        playerData.totalLoss += totalLoss;
        playerData.slot1 = slot1 != "0" ? slot1 : playerData.slot1;
        playerData.slot2 = slot2 != "0" ? slot2 : playerData.slot2;
        playerData.slot3 = slot3 != "0" ? slot3 : playerData.slot3;
        playerData.slot4 = slot4 != "0" ? slot4 : playerData.slot4;
        playerData.rocket += rocket;
        playerData.minishots += minishots;
        playerData.heal += heal;
        playerData.speed += speed;
        playerData.freeze += freeze;
        playerData.shield += shield;
        playerData.lives += lives;
        playerData.health += health;
        playerData.movespeed += movespeed;
        if (!string.IsNullOrEmpty(friendID))
        {
            if (!playerData.FriendsList.Contains(friendID) && (accountDetails.userId != friendID))
            {
                playerData.FriendsList.Add(friendID);
            }
            else
            {
                friendID = null;
            }
        }
        tempId = friendID;
        string data = JsonUtility.ToJson(playerData);
        accountDetails.scriptData.data = data;
        pair.Add("data", data);
        GameSparkRequests saveRequest = new GameSparkRequests();
        saveRequest.Request("Save", pair, SaveSuccess);
    }

    private void SaveSuccess(string str)
    {
        print(str);
        SetUI();
    }


    void SetUI()
    {
        if (playerData == null)
        {
            return;
        }
        if (firstLoad)
        {
            GUIManager.instance.Gold.text = playerData.Gold.ToString();
            //if (GUIManager.instance.Gold.text != playerData.Gold.ToString())
            //    GUIManager.instance.AddCoins(int.Parse(playerData.Gold.ToString()) - int.Parse(GUIManager.instance.Gold.text));
        }
        else
        {

            print("first retrieve");

            CloudRetrieveSphere();
            GUIManager.instance.playerNameTxt.text = accountDetails.displayName;
            leaderboardActor.Create(accountDetails.userId);
            foreach (var item in playerData.FriendsList)
            {
                leaderboardActor.Create(item);
            }

        }
        GUIManager.instance.Gold.text = playerData.Gold.ToString();
        GUIManager.instance.Gems.text = playerData.Gem.ToString();
        GUIManager.instance.mmrTxt.text = playerData.MMR.ToString();
        GUIManager.instance.mostAreaCoveredTxt.text = playerData.mostAreaCovered + " %";
        GUIManager.instance.highestTrophiesTxt.text = playerData.highestTrophies.ToString();
        GUIManager.instance.totalKillsTxt.text = playerData.totalKills.ToString();
        GUIManager.instance.totalDeathsTxt.text = playerData.totalDeaths.ToString();
        GUIManager.instance.totalWinsTxt.text = playerData.totalWins.ToString();
        GUIManager.instance.totalLossTxt.text = playerData.totalLoss.ToString();

        GUIManager.instance.PlayerStats.selectedPlayer = playerData;
        GUIManager.instance.PlayerStats.AssignPlayerData();

        if (!string.IsNullOrEmpty(tempId))
        {
            leaderboardActor.Create(tempId);
        }
        firstLoad = true;
    }

    void CloudRetrieveSphere()
    {
        string[] slot1, slot2, slot3, slot4;

        if (playerData.slot1.Length > 1)
        {
            if (playerData.slot1.Contains(","))
            {
                slot1 = playerData.slot1.Split(',');
                SphereSlotManager.instance.slot1.sphereType = slot1[0];
                SphereSlotManager.instance.slot1.unlockTime = slot1[1];
            }
            else
                SphereSlotManager.instance.slot1.sphereType = playerData.slot1;
        }
        SphereSlotManager.instance.slot1.SetFromCloud();


        if (playerData.slot2.Length > 1)
        {
            if (playerData.slot2.Contains(","))
            {
                slot2 = playerData.slot2.Split(',');
                SphereSlotManager.instance.slot2.sphereType = slot2[0];
                SphereSlotManager.instance.slot2.unlockTime = slot2[1];
            }
            else
                SphereSlotManager.instance.slot2.sphereType = playerData.slot2;
        }
        SphereSlotManager.instance.slot2.SetFromCloud();

        if (playerData.slot3.Length > 1)
        {
            if (playerData.slot3.Contains(","))
            {
                slot3 = playerData.slot3.Split(',');
                SphereSlotManager.instance.slot3.sphereType = slot3[0];
                SphereSlotManager.instance.slot3.unlockTime = slot3[1];
            }
            else
                SphereSlotManager.instance.slot3.sphereType = playerData.slot3;
        }
        SphereSlotManager.instance.slot3.SetFromCloud();

        if (playerData.slot4.Length > 1)
        {
            if (playerData.slot4.Contains(","))
            {
                slot4 = playerData.slot4.Split(',');
                SphereSlotManager.instance.slot4.sphereType = slot4[0];
                SphereSlotManager.instance.slot4.unlockTime = slot4[1];
            }
            else
                SphereSlotManager.instance.slot4.sphereType = playerData.slot4;
        }
        SphereSlotManager.instance.slot4.SetFromCloud();

    }
}