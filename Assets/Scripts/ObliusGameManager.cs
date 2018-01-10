using UnityEngine;
using System.Collections;
using DoozyUI;
using System.Linq;
using UnityEngine.SceneManagement;
using GameSparks.Core;
using GameSparks.Api.Requests;

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


	void Awake ()
	{
		Application.targetFrameRate = 60;
		instance = this;
	}
	// Use this for initialization
	void Start ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public IEnumerator GameOverCoroutine (float delay)
	{

		yield return new WaitForSeconds (delay);
		SoundsManager.instance.PlayGameOverSound ();
//		AdNetworks.instance.HideBanner ();
		gameState = GameState.gameover;

//		Leaderboard.instance.reportScore (ScoreHandler.instance.score);
		GUIManager.instance.ShowGameOverGUI ();
		InGameGUI.instance.GetComponent <UIElement> ().Hide (false);
		//InGameGUI.instance.gameObject.SetActive (false);
//		AdNetworks.instance.ShowInterstitial ();


	}


	public void GameOver (float delay)
	{
		
		StartCoroutine (GameOverCoroutine (delay));
		PowerUpManager.instance.dontSpawn = true;
		InGameGUI.instance.gameStarted = false;
		new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 0).Send((response)=> {});
	}

	public void StartGame ()
	{
		ResetGame ();
		ScoreHandler.instance.incrementNumberOfGames ();
		GUIManager.instance.ShowInGameGUI ();
		InGameGUI.instance.startTime = Time.time;

		//GUIManager.instance.tutorialGUI.ShowIfNeverAppeared();
//		AdNetworks.instance.ShowBanner ();

//		PowerUpManager.instance.ClearPowerUps ();
		PowerUpManager.instance.StartSpawn ();

		//PlayerLives = 4;
		//EnemyLives = 4;
		SnakesSpawner.instance.KillAllSnakes();

	//	SnakesSpawner.instance.playerLives=3;
	//	SnakesSpawner.instance.enemyLives=3;
		//SnakesSpawner.instance.KillAllSnakes ();
		GroundSpawner.instance.ClearGround ();

		SnakesSpawner.instance.SpawnPlayer ();
		SnakesSpawner.instance.SpawnBot ();


		SnakesSpawner.instance.previewMeshContainer.transform.parent.gameObject.SetActive (false);


		gameState = GameState.game;



	}


	public void _ShowFindingMatchScreen(int id)
	{
		GUIManager.instance.ShowInGameGUI ();
		InGameGUI.instance.startTime = Time.time;

		//PowerUpManager.instance.StopSpawn ();
		PowerUpManager.instance.ClearPowerUps ();


		SnakesSpawner.instance.previewMeshContainer.transform.parent.gameObject.SetActive (false);
		//GroundSpawner.instance.ClearGround ();
		ObliusGameManager.instance.ResetGame ();
		ScoreHandler.instance.incrementNumberOfGames ();
		SnakesSpawner.instance.CreateNetworkSnake (id);
		gameState = GameState.game;
	}

	public void ShowFindingMatchScreen()
	{
		if (!PhotonNetwork.connected) {
			GUIManager.instance.ShowLog ("Connecting to server. Please wait!");
			return;
		}
		if (GS.Authenticated)
			StartCoroutine (_ShowFindingMatchScreen ());
		else
		{
			googleLoginPopup.gameObject.SetActive (true);
			googleLoginPopup.Show (false);	
		}	
	}


	IEnumerator _ShowFindingMatchScreen()
	{
		if (PhotonManagerAdvanced.instance.serverStatus == ConnectionStatus.connected) {
			GUIManager.instance.OpenPage (6);
			GUIManager.instance.FillBar.GetComponent<FillTween> ().Fill ();
			yield return PhotonManagerAdvanced.instance._JoinRandomRoom (JoinRandomRoomSuccess, JoinRandomRoomFailed);
		}
//		}
//		else
//		{
//			GUIManager.instance.OpenConnectionPopup ();
//		}
	}


	void JoinRandomRoomFailed()
	{
		if(PhotonManagerAdvanced.instance.serverStatus==ConnectionStatus.connected)
			StartCoroutine(PhotonManagerAdvanced.instance._CreateRoom (failed:CreateRoomFailed,playersFilled:CreateRoomSuccess,noPlayers:CreateRoomFailed));
		else
			GUIManager.instance.OpenPage (3);
	}

	void JoinRandomRoomSuccess()
	{
		_ShowFindingMatchScreen (PhotonNetwork.playerList.Where (a => a.IsLocal).First ().ID);
	}


	void CreateRoomSuccess()
	{
		_ShowFindingMatchScreen (PhotonNetwork.playerList.Where (a => a.IsLocal).First ().ID);
	}

	void CreateRoomFailed()
	{
		GUIManager.instance.OpenPage (3);
	}

	void OnConnectionFail(DisconnectCause cause)
	{
//		if(PhotonNetwork.inRoom)
//			PhotonNetwork.LeaveRoom ();
//		GUIManager.instance.BG.SetActive (true);
//		GUIManager.instance.OpenPage (3);
		if (PhotonManagerAdvanced.instance.IsInGame ()) {
			ForceCloseGame ();
		}
		else
		{
			if (GUIManager.instance.Pages [6].isActiveAndEnabled)
				GUIManager.instance.OpenPage (3);
		}
		
	}


	void ForceCloseGame(){
		Server.instance.isGameOver = true;
		GUIManager.instance.ShowGameOverGUI ();
		GSUpdateMMR.instance.UpdateMMR (-25);
		GUIManager.instance.gameOverGUI.OnLose ();
		GUIManager.instance.gameOverGUI.Reason.text = "Network timeout. Disconnected from server!";
		GUIManager.instance.ShowLog ("Network timeout. Disconnected from server!");
		InGameGUI.instance.gameStarted = false;
		PowerUpManager.instance.StopSpawn ();
		PowerUpManager.instance.NetworkClear ();
		PhotonNetwork.LeaveRoom ();
		SnakesSpawner.instance.KillAllNetworkSnakes ();
		new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 0).Send((response)=> {});
		Snake[] snakes = GameObject.FindObjectsOfType<Snake> ();
		foreach (var snake in snakes)
			Destroy (snake.gameObject);
		GroundSpawner.instance.ClearGround ();

	
		if (!InternetChecker.instance.reconnect) 
		{
			PhotonManagerAdvanced.instance.CloseUP ();
			InternetChecker.instance.reconnect = true;
		}
	}

	public void ResetGame (bool resetScore = true, bool resetOneMoreChance = true)
	{
		if (resetOneMoreChance) {
			oneMoreChanceUsed = false;
		}

		if (resetScore) {
			ScoreHandler.instance.reset ();
		}
	}

	public string TrimPercentage(string str)
	{
		if (str.Length >= 8) {
			return str.Remove (percentageCharLimit);
		} else {
			return str;
		}
	}

}
