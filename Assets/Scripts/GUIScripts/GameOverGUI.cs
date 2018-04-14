using System.Collections;
using DG.Tweening;
using DoozyUI;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_MOBILE
using UnityEngine.Advertisements;
#endif

public class GameOverGUI : MonoBehaviour
{

    public Text scoreText;
    public Text highScoreText;
    public Text diamondText;


    public Text coinText;

    public Button GetCoinButton;

    public Image[] PlayerImage;
    public Text[] PlayerNames, MMR;
    public Text Result, Reason;

    public GameObject RewardWindow, RematchButton, PlayAgainBtn;

    public RenderTexture slot1, slot2, slot3, slot4;
    public GameObject SlotFull, AssignedSlot;

    public Text RematchText;
    public GameObject Fireworks, Confetti, Stars1, Stars2;
    public bool rematchRequested;
    public static GameOverGUI instance;
    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    void OnEnable()
    {
        if (ObliusGameManager.isFriendlyBattle)
        {
            RematchButton.SetActive(true);
            PlayAgainBtn.SetActive(false);
        }
        else
        {
            PlayAgainBtn.SetActive(true);
            RematchButton.SetActive(false);
        }


        RewardWindow.GetComponent<RectTransform>().localPosition = new Vector3(0, -190);



    }

    void OnDisable()
    {
        Fireworks.SetActive(false);
        Confetti.SetActive(false);
        Stars1.SetActive(false);
        Stars2.SetActive(false);

        SlotFull.SetActive(false);
        AssignedSlot.SetActive(false);

        Regeneration.instance.LifeLoseAnim();
    }


    // Update is called once per frame
    void Update()
    {

        string scoreString = ObliusGameManager.instance.TrimPercentage(((100 * (float)ScoreHandler.instance.score) / (float)GroundSpawner.instance.spawnedGroundPieces.Count).ToString());
        string highscoreString = ObliusGameManager.instance.TrimPercentage(((100 * (float)ScoreHandler.instance.highScore) / (float)GroundSpawner.instance.spawnedGroundPieces.Count).ToString());

        scoreText.text = "" + scoreString + "%";
        highScoreText.text = "" + highscoreString + "%";
        diamondText.text = "" + ScoreHandler.instance.specialPoints;

    }




    public void OnWin()
    {

        Fireworks.SetActive(true);
        Confetti.SetActive(true);

        Stars1.SetActive(true);


        SnakesSpawner.instance.playerSnake.isBot = true;
        Result.text = "You Win !!";
        PlayerNames[0].text = InGameGUI.instance.PlayerPanel[0].PlayerName.text;
        PlayerNames[1].text = InGameGUI.instance.PlayerPanel[1].PlayerName.text;

        PlayerImage[0].sprite = InGameGUI.instance.PlayerPanel[0].Shape.sprite;
        PlayerImage[0].color = InGameGUI.instance.PlayerPanel[0].Shape.color;

        PlayerImage[1].sprite = InGameGUI.instance.PlayerPanel[1].Shape.sprite;
        PlayerImage[1].color = InGameGUI.instance.PlayerPanel[1].Shape.color;

        //GameSparkRequests req = new GameSparkRequests();
        //req.Request("AddGold", "amt", "50", UpdateAccountDetails);

       
        GiveRewards();

       

        Invoke("ShowRewards", 1f);

        if (PhotonNetwork.inRoom || ObliusGameManager.BotType == 1)
        {
            //RewardWindow.transform.DOMove (
            //GSUpdateMMR.instance.UpdateMMR(25);

            AccountDetails.instance.Save(MMR: 25);

            MMR[0].text = "+25";
            MMR[1].text = "-25";
        }
        else
        {
            MMR[0].text = "0";
            MMR[1].text = "0";
        }
        //print ("Win");
    }

    void GiveRewards()
    {

        AccountDetails.instance.Save(Gold: 50);
        int slotNo = SphereSlotManager.instance.AssignSphere();
        if (slotNo == 0)
        {
            SlotFull.SetActive(true);
            AssignedSlot.SetActive(false);
        }
        else
        {
            SlotFull.SetActive(false);
            AssignedSlot.SetActive(true);
            switch (slotNo)
            {
                case 1:
                    AssignedSlot.GetComponent<RawImage>().texture = slot1;
                    SphereSlotManager.instance.slot1._sphereProperties.gameObject.SetActive(true);
                    break;
                case 2:
                    AssignedSlot.GetComponent<RawImage>().texture = slot2;
                    SphereSlotManager.instance.slot2._sphereProperties.gameObject.SetActive(true);
                    break;
                case 3:
                    AssignedSlot.GetComponent<RawImage>().texture = slot3;
                    SphereSlotManager.instance.slot3._sphereProperties.gameObject.SetActive(true);
                    break;
                case 4:
                    AssignedSlot.GetComponent<RawImage>().texture = slot4;
                    SphereSlotManager.instance.slot4._sphereProperties.gameObject.SetActive(true);
                    break;
            }

        }
    }

    void ShowRewards()
    {

        RewardWindow.GetComponent<RectTransform>().DOLocalMoveY(-367, 0.5f, false);
    }

    public void UpdateAccountDetails(string response)
    {
        //print (response);
        AccountDetails.instance.Get();
    }


    public void OnLose()
    {
        Stars2.SetActive(true);

        Result.text = "You lose !!";
        PlayerNames[0].text = InGameGUI.instance.PlayerPanel[0].PlayerName.text;
        PlayerNames[1].text = InGameGUI.instance.PlayerPanel[1].PlayerName.text;

        PlayerImage[0].sprite = InGameGUI.instance.PlayerPanel[0].Shape.sprite;
        PlayerImage[0].color = InGameGUI.instance.PlayerPanel[0].Shape.color;

        PlayerImage[1].sprite = InGameGUI.instance.PlayerPanel[1].Shape.sprite;
        PlayerImage[1].color = InGameGUI.instance.PlayerPanel[1].Shape.color;



        if (PhotonNetwork.inRoom || ObliusGameManager.BotType == 1)
        {
            //GSUpdateMMR.instance.UpdateMMR(-25);
            AccountDetails.instance.Save(MMR: -25);

            MMR[0].text = "-25";
            MMR[1].text = "+25";
        }
        else
        {
            MMR[0].text = "0";
            MMR[1].text = "0";
        }
        //print ("Lost");
    }

    public void OnDraw()
    {
        Result.text = "Draw !!";
        PlayerNames[0].text = InGameGUI.instance.PlayerPanel[0].PlayerName.text;
        PlayerNames[1].text = InGameGUI.instance.PlayerPanel[1].PlayerName.text;

        PlayerImage[0].sprite = InGameGUI.instance.PlayerPanel[0].Shape.sprite;
        PlayerImage[0].color = InGameGUI.instance.PlayerPanel[0].Shape.color;

        PlayerImage[1].sprite = InGameGUI.instance.PlayerPanel[1].Shape.sprite;
        PlayerImage[1].color = InGameGUI.instance.PlayerPanel[1].Shape.color;

        MMR[1].text = "+0";
        MMR[0].text = "+0";

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
        if (Server.instance != null)
            PhotonView.Get(Server.instance).RPC("LeftTheGame", PhotonTargets.Others);
        GUIManager.instance.ShowMainMenuGUI();
        if (PhotonNetwork.inRoom)
            PhotonNetwork.LeaveRoom();
        SnakesSpawner.instance.KillAllSnakes();
        Deactivate();
    }

    public void WantRematch()
    {
        //  SnakesSpawner.instance.KillAllNetworkSnakes();
        GroundSpawner.instance.ClearGround();
        if (!rematchRequested)
        {
            RematchText.text = "Waiting for opponent !";
            PhotonView.Get(Server.instance).RPC("Rematch", PhotonTargets.Others);
            StartCoroutine(WaitforRematch());
        }
        else
        {
            rematchRequested = false;
            StartCoroutine(WaitforRematch());
        }
    }

    public void RematchStatus(string status)
    {
        RematchText.text = status;
    }

    IEnumerator WaitforRematch()
    {
        SnakesSpawner.instance.playerNetworkSnake.isWantRematch = 2;
        if (SnakesSpawner.instance.enemyNetworkSnake)
        {
            while (SnakesSpawner.instance.enemyNetworkSnake.isWantRematch == 0)
            {
                yield return null;

            }
            if (SnakesSpawner.instance.enemyNetworkSnake.isWantRematch == 2)
            {
                ObliusGameManager.instance.StartRematch();
                gameObject.SetActive(false);
            }
            else
                RematchText.text = "Opponent left the match";
        }
        else
        {
            RematchText.text = "Opponent left the match";
            print("bug");
        }
        yield return null;
        print("wait over");
    }


    public void Deactivate()
    {
        gameObject.GetComponent<UIElement>().Hide(false);
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
