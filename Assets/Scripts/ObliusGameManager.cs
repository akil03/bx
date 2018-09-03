using System.Collections;
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
    [SerializeField] BoolObject reconnect;
    public bool isFinding;
    public GameSparksActor gameSparksActor;
    int maxplayers = 2;
    public bool matchFailed;

    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        PlayerPrefs.SetInt("TutorialComplete", 1);
        MatchFoundMessage.Listener += MatchFound;
        MatchNotFoundMessage.Listener += MatchNotFound;
        MatchUpdatedMessage.Listener += MatchUpdated;
        GS.GameSparksAvailable += IsGameSparksAvailable;
        matchFailed = true;
    }

    private void IsGameSparksAvailable(bool obj)
    {
        if (!obj)
        {
            ShowReloadPopup();
        }
    }

    private void MatchUpdated(MatchUpdatedMessage obj)
    {
        print("updated");
    }

    private void MatchNotFound(MatchNotFoundMessage obj)
    {
        if (!matchFailed)
        {
            matchFailed = true;
            print("match failed");
            FakeBotMatch();
        }
    }

    private void MatchFound(MatchFoundMessage obj)
    {
        //RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = (byte)maxplayers;
        //roomOptions.EmptyRoomTtl = 0;
        //PhotonNetwork.JoinOrCreateRoom(obj.MatchId, roomOptions, TypedLobby.Default);

        print("Room Name: " + obj.MatchId);
        PhotonNetwork.JoinOrCreateRoom(obj.MatchId, new RoomOptions() { MaxPlayers = (byte)2, IsVisible = false, EmptyRoomTtl = 0 }, TypedLobby.Default);
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
        if (gameState == GameState.game)
            return;

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
        SnakesSpawner.instance.SpawnPlayer();
        SnakesSpawner.instance.SpawnBot();
        SnakesSpawner.instance.previewMeshContainer.transform.parent.gameObject.SetActive(false);
        gameState = GameState.game;

    }

    public void FakeStartGame()
    {
        if (gameState == GameState.game)
            return;

        if (PhotonNetwork.inRoom)
        {
            print("Can't start normal game in a room!");
            return;
        }
        ChangePlayerStaus(false);

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

        GUIManager.instance.inGameGUI.PlayerPanel[1].gameObject.SetActive(true);
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
        if (!PhotonNetwork.connectedAndReady)
        {
            EasyMobile.NativeUI.Alert("Connection Failure", "Establishing connection to the server.. Please Wait !!");
            return;
        }
        if (GS.Authenticated)
        {
            if (Regeneration.instance.LifeAmount < 1)
            {
                Regeneration.instance.UseLife();
                return;
            }

            ChangePlayerStaus(false);
            StartCoroutine(_ShowFindingMatchScreen());
            isOnlineBattle = true;
            isFinding = true;
        }
        else
        {
            EasyMobile.NativeUI.Alert("Connection Failure", "Establishing connection to the server.. Please Wait !!");
        }
    }

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

    public void JoinedRoom()
    {
        if (!isFriendlyBattle)
        {
            StartCoroutine(WaitForPlayers());
        }
    }

    IEnumerator WaitForPlayers()
    {
        while(PhotonNetwork.room==null)
        {
            yield return null;
        }
        print("inside room: "+PhotonNetwork.CloudRegion.ToString());


        while (PhotonNetwork.room.PlayerCount != PhotonNetwork.room.MaxPlayers)
        {
            print(PhotonNetwork.room.PlayerCount);
            yield return null;
        }

        print("players in");
        _ShowFindingMatchScreen(PhotonNetwork.player.ID);
    }

    public void FakeBotMatch()
    {
        if (gameState == GameState.game)
            return;


        isFinding = false;
        BotType = 1;
        FakeStartGame();
    }


    public void CancelFinding()
    {
        GUIManager.instance.ShowMainMenuGUI();
        isFinding = true;
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

    //void OnConnectionFail(DisconnectCause cause)
    //{
    //    if (Server.instance)
    //        ForceCloseGame();
    //}
    bool isPopped;

    void OnFailedToConnectToPhoton()
    {
        ShowReloadPopup();
    }

    void ShowReloadPopup()
    {
        if (isPopped)
            return;

        if (Application.isEditor)
            print("Restart alert popped");

        EasyMobile.NativeUI.AlertPopup alert = EasyMobile.NativeUI.Alert("Time out", "Disconnected from the server. Reload ");
        if (alert != null)
            alert.OnComplete += RestartApp;

    }

    void RestartApp(int r)
    {
        isPopped = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
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
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();
        foreach (var snake in snakes)
            Destroy(snake.gameObject);
        if (!reconnect.value)
        {
            Server.instance.CloseUP();
            reconnect.value = true;
        }
        Invoke("ShowReloadPopup", 2);
    }

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


    public void Reset()
    {
        SnakesSpawner.instance.KillAllSnakes();
        PowerUpManager.instance.ClearPowerUps();
        GroundSpawner.instance.ClearGround();
        if (PhotonNetwork.inRoom)
        {
            SnakesSpawner.instance.KillAllNetworkSnakes();
            PhotonNetwork.LeaveRoom();
        }
    }
}
