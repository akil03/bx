﻿using System.Collections;
using System.Linq;
using DoozyUI;
using GameSparks.Api.Messages;
using GameSparks.Core;
using UnityEngine;

public class ObliusGameManager : MonoBehaviour
{

    public static ObliusGameManager instance;

    public int percentageCharLimit = 4;

    public enum GameState
    {
        menu,
        game,
        gameover,
        shop

    }

    public GameState gameState;
    public bool oneMoreChanceUsed = false;
    public int PlayerLives, EnemyLives;
    public UIElement googleLoginPopup;
    public static int BotType;
    public static bool isFriendlyBattle, isOnlineBattle;
    [SerializeField] EventObject ChangeOnlineStatus;
    [SerializeField] BoolObject isOnline;
    string roomId;
    public GameSparksActor gameSparksActor;

    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("TutorialComplete", 1);
        MatchFoundMessage.Listener += MatchFound;
        MatchNotFoundMessage.Listener += MatchNotFound;
        MatchUpdatedMessage.Listener += MatchUpdated;
    }

    private void MatchUpdated(MatchUpdatedMessage obj)
    {
        print("updated");
    }

    private void MatchNotFound(MatchNotFoundMessage obj)
    {
        FakeBotMatch();
    }

    private void MatchFound(MatchFoundMessage obj)
    {
        PhotonNetwork.JoinOrCreateRoom(obj.MatchId, new RoomOptions { maxPlayers = (byte)maxplayers }, TypedLobby.Default);
    }


    public void ChangePlayerStaus(bool status)
    {
        isOnline.value = status;
        ChangeOnlineStatus.Fire();
    }

    public IEnumerator GameOverCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SoundsManager.instance.PlayGameOverSound();
        gameState = GameState.gameover;
        GUIManager.instance.ShowGameOverGUI();
        InGameGUI.instance.GetComponent<UIElement>().Hide(false);
    }


    public void GameOver(float delay)
    {

        StartCoroutine(GameOverCoroutine(delay));
        PowerUpManager.instance.ClearPowerUps();
        InGameGUI.instance.gameStarted = false;
    }


    public void PlayAgain()
    {
        if (isOnlineBattle)
        {
            GUIManager.instance.gameOverGUI.OnPlayButtonClick();
            Invoke("ShowFindingMatchScreen", 0.8f);
        }
        else
        {
            GUIManager.instance.gameOverGUI.OnPlayButtonClick();
            Invoke("StartGame", 0.8f);
        }
    }



    public void StartGame()
    {
        if (PhotonNetwork.inRoom)
        {
            print("Can't start normal game in a room!");
            return;
        }
        ChangePlayerStaus(false);
        BotType = 0;
        if (Regeneration.instance.LifeAmount < 1)
        {
            Regeneration.instance.UseLife();
            return;
        }
        ResetGame();
        ScoreHandler.instance.incrementNumberOfGames();
        GUIManager.instance.ShowInGameGUI();
        InGameGUI.instance.startTime = Time.time;
        PowerUpManager.instance.StartSpawn();
        SnakesSpawner.instance.KillAllSnakes();
        GroundSpawner.instance.ClearGround();
        SnakesSpawner.instance.SpawnPlayer();
        SnakesSpawner.instance.SpawnBot();
        SnakesSpawner.instance.previewMeshContainer.transform.parent.gameObject.SetActive(false);
        gameState = GameState.game;
    }

    bool tutStarted;

    public void StartTutorial()
    {
        if (PlayerPrefs.HasKey("TutorialComplete"))
        {
            GUIManager.instance.OpenPage(3);
            return;
        }

        if (tutStarted)
            return;

        tutStarted = true;
        StartCoroutine(TutorialList());
    }

    public void StartTutorialOverride()
    {
        if (tutStarted)
            return;

        tutStarted = true;
        StartCoroutine(TutorialList());
    }

    IEnumerator TutorialList()
    {
        ChangePlayerStaus(false);
        BotType = 0;
        if (PlayerPrefs.HasKey("TutorialComplete"))
            PlayerPrefs.DeleteKey("TutorialComplete");

        ResetGame();
        //SnakesSpawner.instance.SpawnPlayer ();

        PowerUpManager.instance.ClearPowerUps();

        //GUIManager.instance.mainMenuGUI.GetComponent <UIElement> ().Hide (false);
        StartCoroutine(SnakesSpawner.instance.SpawnNewSnake(true, 999));
        SnakesSpawner.instance.previewMeshContainer.transform.parent.gameObject.SetActive(false);
        gameState = GameState.game;
        GUIManager.instance.BG.SetActive(false);
        GUIManager.instance.mainMenuGUI.transform.parent.gameObject.SetActive(false);
        GUIManager.instance.ShowTutorialLog("Welcome to Battle Xonix !! \nI will be your trainer today, Just follow my instructions !!", 2);

        yield return new WaitForSeconds(1.3f);


        //		PlayerPrefs.SetInt ("TutorialComplete", 1);
        //		Application.LoadLevel (0);
        //		yield break; 
        //CloseTutorial ();//s		

        //	yield break;


        GUIManager.instance.ShowTutorialLog("Swipe right");
        //yield return new WaitForSeconds (0.5f);
        Time.timeScale = 0;
        while (SwipeHandler.instance.lastSwipeDirection != SwipeHandler.SwipeDirection.right)
        {
            yield return null;
        }
        Time.timeScale = 1;
        GUIManager.instance.HideTutorialLog();

        yield return new WaitForSeconds(0.85f);
        GUIManager.instance.ShowTutorialLog("Swipe down");
        //	yield return new WaitForSeconds (0.5f);
        Time.timeScale = 0;
        while (SwipeHandler.instance.lastSwipeDirection != SwipeHandler.SwipeDirection.down)
        {
            yield return null;
        }
        Time.timeScale = 1;
        GUIManager.instance.HideTutorialLog();

        yield return new WaitForSeconds(0.85f);
        GUIManager.instance.ShowTutorialLog("Swipe left");
        //yield return new WaitForSeconds (0.5f);
        Time.timeScale = 0;
        while (SwipeHandler.instance.lastSwipeDirection != SwipeHandler.SwipeDirection.left)
        {
            yield return null;
        }
        Time.timeScale = 1;
        GUIManager.instance.HideTutorialLog();

        yield return new WaitForSeconds(1f);
        GUIManager.instance.ShowTutorialLog("Trail completed. Good Work !!", 3);



        yield return new WaitForSeconds(2f);
        GUIManager.instance.ShowTutorialLog("Now cover more than 50% of the area !!", 2);


        GUIManager.instance.ShowInGameGUI();
        GUIManager.instance.inGameGUI.PlayerPanel[1].gameObject.SetActive(false);
        GUIManager.instance.inGameGUI.PlayerPanel[1].FillRatio.transform.parent.gameObject.SetActive(false);
        GUIManager.instance.inGameGUI.totalGameTime = 9999;
        GUIManager.instance.inGameGUI.PlayerPanel[0].RemainingLives.gameObject.SetActive(false);
        GUIManager.instance.inGameGUI.TimerText.transform.parent.gameObject.SetActive(false);
        while (GUIManager.instance.inGameGUI.PlayerPanel[0].fillamount < 50)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        GUIManager.instance.inGameGUI.GetComponent<UIElement>().Hide(false);
        yield return new WaitForSeconds(1);

        SnakesSpawner.instance.KillAllSnakes();
        GUIManager.instance.inGameGUI.PlayerPanel[1].gameObject.SetActive(true);
        GroundSpawner.instance.ClearGround();
        StartCoroutine(SnakesSpawner.instance.SpawnNewSnake(false, 1));
        GUIManager.instance.ShowInGameGUI();
        GUIManager.instance.inGameGUI.PlayerPanel[1].FillRatio.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CameraHandler.instance.objectToFollow = SnakesSpawner.instance.enemySnake.gameObject;
        SnakesSpawner.instance.enemySnake.speed = 0;
        yield return new WaitForSeconds(0.5f);
        GUIManager.instance.ShowTutorialLog("Great !! Now lets see your performance against me");
        yield return new WaitForSeconds(2f);
        GUIManager.instance.HideTutorialLog();
        SnakesSpawner.instance.enemySnake.speed = SnakesSpawner.instance.enemySnake.normalSpeed;
        yield return new WaitForSeconds(0.5f);
        GUIManager.instance.ShowTutorialLog("Hit my trail before I make a completion to kill me !! ");
        yield return new WaitForSeconds(4f);
        GUIManager.instance.HideTutorialLog();
        StartCoroutine(SnakesSpawner.instance.SpawnNewSnake(true, 999));
        yield return new WaitForSeconds(0.5f);
        CameraHandler.instance.objectToFollow = SnakesSpawner.instance.playerSnake.gameObject;
    }

    public void CloseTutorial()
    {
        Time.timeScale = 1;
        GUIManager.instance.BG.SetActive(true);
        SnakesSpawner.instance.KillAllSnakes();
        GroundSpawner.instance.ClearGround();
        GUIManager.instance.HideTutorialLog();
        GUIManager.instance.inGameGUI.TimerText.transform.parent.gameObject.SetActive(true);
        GUIManager.instance.inGameGUI.PlayerPanel[0].gameObject.SetActive(true);
        GUIManager.instance.inGameGUI.PlayerPanel[1].gameObject.SetActive(true);
        GUIManager.instance.OpenPage(3);
        ChangePlayerStaus(true);
    }



    public void _ShowFindingMatchScreen(int id)
    {
        //Regeneration.instance.UseLife ();
        GUIManager.instance.ShowInGameGUI();
        InGameGUI.instance.startTime = Time.time;

        //PowerUpManager.instance.StopSpawn ();
        PowerUpManager.instance.ClearPowerUps();


        SnakesSpawner.instance.previewMeshContainer.transform.parent.gameObject.SetActive(false);
        //GroundSpawner.instance.ClearGround ();
        ObliusGameManager.instance.ResetGame();
        ScoreHandler.instance.incrementNumberOfGames();
        SnakesSpawner.instance.CreateNetworkSnake(id);
        gameState = GameState.game;
        isFinding = false;
    }

    //attached to play button
    public void ShowFindingMatchScreen()
    {
        ChangePlayerStaus(false);
        if (Regeneration.instance.LifeAmount < 1)
        {
            Regeneration.instance.UseLife();
            return;
        }



        if (!PhotonNetwork.connected)
        {
            GUIManager.instance.ShowLog("Connecting to server. Please wait!");
            return;
        }
        if (GS.Authenticated)
            StartCoroutine(_ShowFindingMatchScreen());
        else
        {
            googleLoginPopup.gameObject.SetActive(true);
            googleLoginPopup.Show(false);
            GUIManager.instance.ShowLog("Please login!");
        }

        isOnlineBattle = true;
        isFinding = true;
    }

    bool matchmakingPhase;
    bool findingPhase;
    bool normal;

    IEnumerator _ShowFindingMatchScreen()
    {
        if (!PhotonNetwork.connected)
        {
            GUIManager.instance.ShowLog("Connecting to server. Please wait!");
            yield break;
        }
        if (!GS.Authenticated)
        {
            GUIManager.instance.ShowLog("Please login!");
            yield break;
        }
        GUIManager.instance.OpenPage(6);
        GUIManager.instance.FillBar.GetComponent<FillTween>().Fill();
        gameSparksActor.FindPlayers();
    }

    bool isServer;
    int maxplayers = 2;

    public void JoinedRoom()
    {
        if (!isFriendlyBattle)
        {
            _ShowFindingMatchScreen(PhotonNetwork.player.ID);
        }        
    }

    public void FakeBotMatch()
    {
        isFinding = false;
        BotType = 1;
        StartGame();
    }


    public void CancelFinding()
    {
        GUIManager.instance.ShowMainMenuGUI();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && isFinding)
        {
            CancelFinding();
        }
    }

    public void StartRematch()
    {
       // GroundSpawner.instance.ClearGround();
        _ShowFindingMatchScreen(PhotonNetwork.playerList.Where(a => a.IsLocal).First().ID);
    }

    void OnConnectionFail(DisconnectCause cause)
    {
        //		if(PhotonNetwork.inRoom)
        //			PhotonNetwork.LeaveRoom ();
        //		GUIManager.instance.BG.SetActive (true);
        //		GUIManager.instance.OpenPage (3);
        //	if (PhotonManagerAdvanced.instance.IsInGame ()) {
        ForceCloseGame();
        //		}
        //		else
        //		{
        //			if (GUIManager.instance.Pages [5].isActiveAndEnabled)
        //				GUIManager.instance.OpenPage (3);
        //		}

    }


    void ForceCloseGame()
    {
        Server.instance.isGameOver = true;
        GUIManager.instance.ShowGameOverGUI();
        GSUpdateMMR.instance.UpdateMMR(-25);
        GUIManager.instance.gameOverGUI.OnLose();
        GUIManager.instance.gameOverGUI.Reason.text = "Network timeout. Disconnected from server!";
        GUIManager.instance.ShowLog("Network timeout. Disconnected from server!");
        InGameGUI.instance.gameStarted = false;
        PowerUpManager.instance.ClearPowerUps();
        PhotonNetwork.LeaveRoom();
        SnakesSpawner.instance.KillAllNetworkSnakes();
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();
        foreach (var snake in snakes)
            Destroy(snake.gameObject);
        GroundSpawner.instance.ClearGround();


        if (!reconnect.value)
        {
            Server.instance.CloseUP();
            reconnect.value = true;
        }
    }

    [SerializeField] BoolObject reconnect;
    private bool isFinding;


    public void ResetGame(bool resetScore = true, bool resetOneMoreChance = true)
    {
        if (resetOneMoreChance)
        {
            oneMoreChanceUsed = false;
        }

        if (resetScore)
        {
            ScoreHandler.instance.reset();
        }
    }

    public string TrimPercentage(string str)
    {
        if (str.Length >= 8)
        {
            return str.Remove(percentageCharLimit);
        }
        else
        {
            return str;
        }
    }

}
