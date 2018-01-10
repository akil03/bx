using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameSparks.Api.Requests;

public class Server : MonoBehaviour 
{
	public static Server instance;

	public bool isGameOver=false;
	void Start()
	{
		instance = this;
	}

	[PunRPC]
	public void GameOver(int id,string reason)
	{
		if (isGameOver) {
			return;		
		}
		PowerUpManager.instance.dontSpawn = true;
		PlayerInfo[] players = GameObject.FindObjectsOfType<PlayerInfo> ();
		var selected = players.Where (a => (PhotonView.Get (a.gameObject).viewID == id)).First ();

		if (PhotonView.Get (selected.gameObject).isMine) {
			GUIManager.instance.gameOverGUI.OnLose ();
			GSUpdateMMR.instance.UpdateMMR (-25);
		} else {
			GUIManager.instance.gameOverGUI.OnWin ();
			GSUpdateMMR.instance.UpdateMMR (25);
		}
		GUIManager.instance.gameOverGUI.Reason.text = reason;

		if (PhotonNetwork.isMasterClient)			
		{
			PowerUpManager.instance.StopSpawn ();
			PowerUpManager.instance.NetworkClear ();
		}

		if (SnakesSpawner.instance.playerSnake.isHeadOn || SnakesSpawner.instance.enemySnake.isHeadOn) 
		{			
			//GUIManager.instance.gameOverGUI.OnDraw ();
//			PowerUpManager.instance.StopSpawn ();
		}

		InGameGUI.instance.gameStarted = false;
		new LogEventRequest().SetEventKey("SetPlayerStatus").SetEventAttribute("IsInGame", 0).Send((response)=> {});
//		foreach (var player in players) {
//			print (PhotonView.Get(player.gameObject).viewID==id ? PhotonView.Get(player.gameObject).viewID+" lose" : PhotonView.Get(player.gameObject).viewID+" win");
//			if(PhotonView.Get(player.gameObject).viewID==id && )
//				GUIManager.instance.gameOverGUI.OnLose ();
//			else
//				GUIManager.instance.gameOverGUI.OnWin ();
//		}
//		if (SnakesSpawner.instance.playerSnake.Lives > 0)
//			GUIManager.instance.gameOverGUI.OnWin ();
//		else
//			GUIManager.instance.gameOverGUI.OnLose ();
	




		GUIManager.instance.ShowGameOverGUI ();
		PhotonNetwork.LeaveRoom ();
		SnakesSpawner.instance.KillAllNetworkSnakes ();
		Snake[] snakes = GameObject.FindObjectsOfType<Snake> ();
		foreach (var snake in snakes)
			Destroy (snake.gameObject);
		GroundSpawner.instance.ClearGround ();
		print ("someone died!");
		isGameOver = true;
		if (!InternetChecker.instance.reconnect) 
		{
			PhotonManagerAdvanced.instance.CloseUP ();
			InternetChecker.instance.reconnect = true;
		}
	}

	[PunRPC]
	public void OnOpponentLeave()
	{
		if (!isGameOver) {
			GUIManager.instance.gameOverGUI.OnWin ();
			GUIManager.instance.gameOverGUI.Reason.text = "Your opponent left the match !!";
			GUIManager.instance.ShowLog ("Your opponent left the match !!",3);
		}
		InGameGUI.instance.gameStarted = false;
		GUIManager.instance.ShowGameOverGUI ();
		PhotonNetwork.LeaveRoom ();
		SnakesSpawner.instance.KillAllNetworkSnakes ();
		Snake[] snakes = GameObject.FindObjectsOfType<Snake> ();
		foreach (var snake in snakes)
			Destroy (snake.gameObject);
		GroundSpawner.instance.ClearGround ();
		print ("someone died!");
		if (!InternetChecker.instance.reconnect) 
		{
			PhotonManagerAdvanced.instance.CloseUP ();
			InternetChecker.instance.reconnect = true;
		}
	}

	public void WinLose(int id)
	{
		
	}


	[PunRPC]
	public void StartGame()
	{
		//GUIManager.instance.ShowInGameGUI ();
//		InGameGUI.instance.startTime = Time.time;
//		PowerUpManager.instance.ClearPowerUps ();
//		SnakesSpawner.instance.previewMeshContainer.transform.parent.gameObject.SetActive (false);
	}
}
