using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoozyUI;
public class ProfileStatsManager : MonoBehaviour {
    public PlayerData selectedPlayer;

    public Text NameTxt, MMRTxt, MostCoveredTxt, HighestTrophiesTxt, TotalKillsTxt, TotalDeathsTxt, TotalWinsTxt, TotalLossTxt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AssignPlayerData()
    {
        if (selectedPlayer == null)
            return;

        NameTxt.text = selectedPlayer.displayName;
        MMRTxt.text = selectedPlayer.MMR.ToString();
        MostCoveredTxt.text = selectedPlayer.mostAreaCovered.ToString("0.00") + "%";
        HighestTrophiesTxt.text = selectedPlayer.highestTrophies.ToString();
        TotalKillsTxt.text = selectedPlayer.totalKills.ToString();
        TotalDeathsTxt.text = selectedPlayer.totalDeaths.ToString();
        TotalWinsTxt.text = selectedPlayer.totalWins.ToString();
        TotalLossTxt.text = selectedPlayer.totalWins.ToString();

        

    }
}
