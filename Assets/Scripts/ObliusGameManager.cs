using System.Collections;
using System.Linq;
using DoozyUI;
using GameSparks.Api.Requests;
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
    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("TutorialComplete", 1);
        //StartTutorial ();
    }

    // Update is called once per frame
    void Update()
    {
        //if (PhotonNetwork.connected)
        //	print(PhotonNetwork.GetPing().ToString());
    }

    public IEnumerator GameOverCoroutine(float delay)
    {

        yield return new WaitForSeconds(delay);
        SoundsManager.instance.PlayGameOverSound();
        //		AdNetworks.instance.HideBanner ();
        gameState = GameState.gameover;

        //		Leaderboard.instance.reportScore (ScoreHandler.instance.score);
        GUIManager.instance.ShowGameOverGUI();
        InGameGUI.instance.GetComponent<UIElement>().Hide(false);
        //InGameGUI.instance.gameObject.SetActive (false);
        //		AdNetworks.instance.ShowInterstitial ();


    }


    public void GameOver(float delay)
    {

        StartCoroutine(GameOverCoroutine(delay));
        PowerUpManager.instance.dontSpawn = true;
        InGameGUI.instance.gameStarted = false;
        new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 0).Send((response) => { });
    }


    public void PlayAgain()
    {
        if (isOnlineBattle)
        {
            GUIManager.instance.gameOverGUI.OnPlayButtonClick();
            Invoke("ShowFindingMatchScreen", 1.5f);
        }
        else
        {
            GUIManager.instance.gameOverGUI.OnPlayButtonClick();
            Invoke("StartGame", 1.5f);
        }
    }



    public void StartGame()
    {
        //		PlayerPrefs.DeleteAll ();
        //		if(!PlayerPrefs.HasKey ("TutorialComplete")){
        //			StartTutorial ();
        //			return;
        //		}
        //
        BotType = 0;
        if (Regeneration.instance.LifeAmount < 1)
        {
            Regeneration.instance.UseLife();
            return;
        }
        //Regeneration.instance.UseLife ();


        ResetGame();
        ScoreHandler.instance.incrementNumberOfGames();
        GUIManager.instance.ShowInGameGUI();
        InGameGUI.instance.startTime = Time.time;

        //GUIManager.instance.tutorialGUI.ShowIfNeverAppeared();
        //		AdNetworks.instance.ShowBanner ();

        //		PowerUpManager.instance.ClearPowerUps ();
        PowerUpManager.instance.StartSpawn();

        //PlayerLives = 4;
        //EnemyLives = 4;
        SnakesSpawner.instance.KillAllSnakes();

        //	SnakesSpawner.instance.playerLives=3;
        //	SnakesSpawner.instance.enemyLives=3;
        //SnakesSpawner.instance.KillAllSnakes ();
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
    }

    //attached to play button
    public void ShowFindingMatchScreen()
    {
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
        matchmakingPhase = true;
        PhotonNetwork.JoinOrCreateRoom("matchmaking", null, null);
    }
    bool isServer;
    int maxplayers = 2;

    public void JoinedRoom()
    {
        AddToMatchmakingQueue();

        if (matchmakingPhase)
            return;
        if (findingPhase)
        {
            new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 1).Send((response) => { });
            if (reconnect.value)
                _ShowFindingMatchScreen(PhotonNetwork.player.ID);
            print("Max players reached!");
            isServer = false;
            findingPhase = false;
        }
    }

    public void OnPlayerConnected()
    {
        AddToMatchmakingQueue();
        if (findingPhase)
        {
            if (PhotonNetwork.room.PlayerCount == maxplayers)
                PhotonNetwork.room.IsVisible = false;
        }
    }

    void AddToMatchmakingQueue()
    {
        if (!ObliusGameManager.isFriendlyBattle)
            Invoke("FakeBotMatch", 10);

        if (matchmakingPhase)
        {
            if (PhotonNetwork.room.PlayerCount >= 2)
            {
                if (PhotonNetwork.player.ID % 2 != 0)
                {
                    isServer = true;
                }
                else
                {
                    isServer = false;
                }
                PhotonNetwork.LeaveRoom();
                print("Begin a game!");
                findingPhase = true;
                return;
            }
            else
            {
                print("Added to queue!");
                return;
            }
        }
    }

    public void FakeBotMatch()
    {
        if (PhotonNetwork.room.PlayerCount < 2)
        {

            PhotonNetwork.LeaveRoom();
            isGameSearchFail = true;
        }

    }


    public void CancelFinding()
    {
        if (PhotonNetwork.room.PlayerCount < 2)
        {
            //normal = false;
            PhotonNetwork.LeaveRoom();
            GUIManager.instance.ShowMainMenuGUI();
        }
    }

    void StartGameServer()
    {
        if (matchmakingPhase)
        {
            if (isServer)
            {
                print("create room");
                PhotonNetwork.CreateRoom(null);
            }
            else
            {
                print("join room");
                PhotonNetwork.JoinRandomRoom();
            }
            matchmakingPhase = false;
        }
    }

    public void ConnectedToMaster()
    {
        StartGameServer();
    }


    public void RandomJoinFailed()
    {
        if (findingPhase)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    void JoinRandomRoomFailed()
    {
        //		if (PhotonManagerAdvanced.instance.serverStatus == ConnectionStatus.connected)
        //			StartCoroutine (PhotonManagerAdvanced.instance._CreateRoom (failed: CreateRoomFailed, playersFilled: CreateRoomSuccess, noPlayers: CreateRoomFailed));
        //		else {
        //			GUIManager.instance.OpenPage (3);
        //
        //		}
    }

    void JoinRandomRoomSuccess()
    {
        _ShowFindingMatchScreen(PhotonNetwork.playerList.Where(a => a.IsLocal).First().ID);
    }


    void CreateRoomSuccess()
    {
        _ShowFindingMatchScreen(PhotonNetwork.playerList.Where(a => a.IsLocal).First().ID);
    }



    public void StartRematch()
    {
        _ShowFindingMatchScreen(PhotonNetwork.playerList.Where(a => a.IsLocal).First().ID);
    }

    bool isGameSearchFail;
    void CreateRoomFailed()
    {
        //GUIManager.instance.OpenPage (3);
        PhotonNetwork.LeaveRoom();
        isGameSearchFail = true;
        //StartGame ();
    }

    void OnLeftRoom()
    {
        if (isGameSearchFail)
        {
            StartGame();
            isGameSearchFail = false;
            BotType = 1;
        }
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
        PowerUpManager.instance.StopSpawn();
        PowerUpManager.instance.ClearPowerUps();
        PhotonNetwork.LeaveRoom();
        SnakesSpawner.instance.KillAllNetworkSnakes();
        new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 0).Send((response) => { });
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
