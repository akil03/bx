using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DoozyUI;
using GameSparks.Api.Requests;
using DG.Tweening;

#if UNITY_MOBILE
using UnityEngine.Advertisements;
#endif

public class GameOverGUI : MonoBehaviour {

    public Text scoreText;
    public Text highScoreText;
    public Text diamondText;


    public Text coinText;

    public Button GetCoinButton;

	public Image[] PlayerImage;
	public Text[] PlayerNames,MMR;
	public Text Result,Reason;

	public GameObject RewardWindow,RematchButton;

	public Text RematchText;

	// Use this for initialization
	void Start () {
	
	}

	void OnEnable(){
		if (ObliusGameManager.isFriendlyBattle)
			RematchButton.SetActive (true);
		else
			RematchButton.SetActive (false);
		
		RewardWindow.GetComponent <RectTransform> ().localPosition = new Vector3(0,-190);
	}


	// Update is called once per frame
	void Update () {

		string scoreString = ObliusGameManager.instance.TrimPercentage(((100 * (float)ScoreHandler.instance.score) / (float)GroundSpawner.instance.spawnedGroundPieces.Count).ToString());
		string highscoreString = ObliusGameManager.instance.TrimPercentage(((100 * (float)ScoreHandler.instance.highScore) / (float)GroundSpawner.instance.spawnedGroundPieces.Count).ToString());

		scoreText.text = "" + scoreString + "%";
		highScoreText.text = "" + highscoreString + "%";
        diamondText.text = "" + ScoreHandler.instance.specialPoints;

	}




	public void OnWin(){
		SnakesSpawner.instance.playerSnake.isBot = true;
		Result.text = "You Win !!";
		PlayerNames [0].text = InGameGUI.instance.PlayerPanel [0].PlayerName.text;
		PlayerNames [1].text = InGameGUI.instance.PlayerPanel [1].PlayerName.text;

		PlayerImage [0].sprite = InGameGUI.instance.PlayerPanel [0].Shape.sprite;
		PlayerImage [0].color = InGameGUI.instance.PlayerPanel [0].Shape.color;

		PlayerImage [1].sprite = InGameGUI.instance.PlayerPanel [1].Shape.sprite;
		PlayerImage [1].color = InGameGUI.instance.PlayerPanel [1].Shape.color;

		GameSparkRequests req = new GameSparkRequests ();
		req.Request ("AddGold", "amt", "50",UpdateAccountDetails);

		Invoke ("ShowRewards", 1f);

		if (PhotonNetwork.inRoom||ObliusGameManager.BotType==1)
		{
			//RewardWindow.transform.DOMove (
			GSUpdateMMR.instance.UpdateMMR (25);
			MMR [0].text = "+25";
			MMR [1].text = "-25";
		} else {
			MMR [0].text = "0";
			MMR [1].text = "0";
		}
		//print ("Win");
	}

	void ShowRewards(){
		
		RewardWindow.GetComponent <RectTransform> ().DOLocalMoveY (-367, 0.5f, false);
	}

	public void UpdateAccountDetails(string response){
		//print (response);
		AccountDetails.instance.Get ();
	}


	public void OnLose(){
		Result.text = "You lose !!";
		PlayerNames [0].text = InGameGUI.instance.PlayerPanel [0].PlayerName.text;
		PlayerNames [1].text = InGameGUI.instance.PlayerPanel [1].PlayerName.text;

		PlayerImage [0].sprite = InGameGUI.instance.PlayerPanel [0].Shape.sprite;
		PlayerImage [0].color = InGameGUI.instance.PlayerPanel [0].Shape.color;

		PlayerImage [1].sprite = InGameGUI.instance.PlayerPanel [1].Shape.sprite;
		PlayerImage [1].color = InGameGUI.instance.PlayerPanel [1].Shape.color;



		if (PhotonNetwork.inRoom||ObliusGameManager.BotType==1)
		{
			GSUpdateMMR.instance.UpdateMMR (-25);
			MMR [0].text = "-25";
			MMR [1].text = "+25";
		} else {
			MMR [0].text = "0";
			MMR [1].text = "0";
		}
		//print ("Lost");
	}

	public void OnDraw(){
		Result.text = "Draw !!";
		PlayerNames [0].text = InGameGUI.instance.PlayerPanel [0].PlayerName.text;
		PlayerNames [1].text = InGameGUI.instance.PlayerPanel [1].PlayerName.text;

		PlayerImage [0].sprite = InGameGUI.instance.PlayerPanel [0].Shape.sprite;
		PlayerImage [0].color = InGameGUI.instance.PlayerPanel [0].Shape.color;

		PlayerImage [1].sprite = InGameGUI.instance.PlayerPanel [1].Shape.sprite;
		PlayerImage [1].color = InGameGUI.instance.PlayerPanel [1].Shape.color;

		MMR [1].text = "+0";
		MMR [0].text = "+0";

		Reason.text = "";
		//print ("Lost");
	}

    public void OnGetCoinButtonClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();

		#if UNITY_MOBILE

        UnityRewardAds.instance.ShowRewardedAd(HandleShowResult);

		#endif

        GetCoinButton.interactable = false;
    }

    public void OnBallShopClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();

        Deactivate();
        GUIManager.instance.ShowShopGUI();
    }

    public void OnRemoveAdsButtonClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();

     //   AdRemover.instance.BuyNonConsumable();
    }

    public void OnRestorePurchaseButtonClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();

      //  AdRemover.instance.RestorePurchases();
    }

    public void OnLeaderboardButtonClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();

      //  Leaderboard.instance.showLeaderboard();
    }

    public void OnShareButtonClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();

        ShareManager.instance.share();
    }

    public void OnPlayButtonClick()
    {
		if(PhotonNetwork.inRoom)
			PhotonNetwork.LeaveRoom ();
		
		GUIManager.instance.ShowMainMenuGUI ();			
		SnakesSpawner.instance.KillAllSnakes ();
		new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 0).Send((response)=> {});
		Deactivate ();
	}

	public void WantRematch(){
		RematchText.text = "Waiting for opponent !";
		StartCoroutine (WaitforRematch ());
	}

	IEnumerator WaitforRematch(){
		SnakesSpawner.instance.playerNetworkSnake.isWantRematch = 2;
		if (SnakesSpawner.instance.enemyNetworkSnake) {
			while (SnakesSpawner.instance.enemyNetworkSnake.isWantRematch == 0) {
				yield return null;

			}
			if (SnakesSpawner.instance.enemyNetworkSnake.isWantRematch == 2) {
				ObliusGameManager.instance.StartRematch ();
				gameObject.SetActive (false);
			}
			else
				RematchText.text = "Opponent left the match";
		} else {
			RematchText.text = "Opponent left the match";
			print ("bug");
		}
		yield return null;
		print ("wait over");
	}


    public void Deactivate()
    {
		gameObject.GetComponent<UIElement> ().Hide (false);
      //  gameObject.SetActive(false);
    }

	#if UNITY_MOBILE
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                ScoreHandler.instance.increaseSpecialPoints(UnityRewardAds.instance.GetCoinsToRewardOnVideoWatched());
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
	#endif


}
