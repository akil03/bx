using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoozyUI;
public class ProfileSelectManager : MonoBehaviour {
    public static ProfileSelectManager instance;
    public LeaderboardGUI selectedProfile;
    public Image playerImage, OnlineImage;
    public Text playerName,MMRTxt;
    public Button ChallengeButton;
	// Use this for initialization
	void Awake () {
        instance = this;
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (selectedProfile)
        {
            
            OnlineImage.gameObject.SetActive(selectedProfile.IsOnline());
            OnlineImage.color = selectedProfile.IsFree() ? Color.green : Color.red;
            ChallengeButton.interactable = selectedProfile.IsFree()&&selectedProfile.IsOnline();
        }
	}

    public void AssignValues()
    {
        playerImage.sprite = selectedProfile.sprite.sprite;
        playerName.text = selectedProfile.playerName.text;
        MMRTxt.text = selectedProfile.mmr.text;

    }

    public void OnShowSelectedProfile()
    {
        GUIManager.instance.PlayerStats.selectedPlayer = selectedProfile.data;
        GUIManager.instance.PlayerStats.AssignPlayerData();
        GUIManager.instance.PlayerStats.gameObject.SetActive(true);
        GUIManager.instance.PlayerStats.GetComponent<UIElement>().Show(false);
        gameObject.SetActive(false);
    }

    public void OnFriendlyBattle()
    {
        gameObject.SetActive(false);
        selectedProfile.SetOpponentID();
    }
}
