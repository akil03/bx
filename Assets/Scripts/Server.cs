using System.Collections;
using System.Linq;
using UnityEngine;

public class Server : MonoBehaviour
{
    public static Server instance;

    public bool isGameOver = false;
    PlayerInfo[] players;


    void Start()
    {
        instance = this;
        players = GameObject.FindObjectsOfType<PlayerInfo>();
        StartCoroutine(SetMaster());
    }

    [PunRPC]
    public void Rematch()
    {
        if (ObliusGameManager.isFriendlyBattle)
        {
            GameOverGUI.instance.rematchRequested = true;
            GUIManager.instance.rematchResponse.Invoke();
        }
    }

    [PunRPC]
    public void LeftTheGame()
    {
        if (ObliusGameManager.isFriendlyBattle)
            GUIManager.instance.leftTheMatch.Invoke();
    }

    [PunRPC]
    public void GameOver(int id, string reason)
    {
        if (isGameOver)
        {
            return;
        }
        PowerUpManager.instance.dontSpawn = true;
        var selected = players.Where(a => (PhotonView.Get(a.gameObject).viewID == id)).First();

        if (PhotonView.Get(selected.gameObject).isMine)
        {
            GUIManager.instance.gameOverGUI.OnLose();
        }
        else
        {
            GUIManager.instance.gameOverGUI.OnWin();
        }
        GUIManager.instance.gameOverGUI.Reason.text = reason;


        if (SnakesSpawner.instance.playerSnake.isHeadOn || SnakesSpawner.instance.enemySnake.isHeadOn)
        {
            //GUIManager.instance.gameOverGUI.OnDraw ();
            //			PowerUpManager.instance.StopSpawn ();
        }

        InGameGUI.instance.gameStarted = false;
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





        GUIManager.instance.ShowGameOverGUI();
        //PhotonNetwork.LeaveRoom ();
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();
        foreach (var snake in snakes)
            Destroy(snake.gameObject);
        print("someone died!");
        isGameOver = true;
        if (!reconnect.value)
        {
            CloseUP();
            reconnect.value = true;
        }
    }

    IEnumerator SetMaster()
    {
        var best = PhotonNetwork.playerList.OrderBy(a => a.CustomProperties["ping"]).ToList()[0];
        foreach (var item in players)
        {
            PhotonView.Get(item).TransferOwnership(best);
        }
        yield return new WaitForSeconds(5);
        StartCoroutine(SetMaster());
    }


    [SerializeField] BoolObject reconnect;

    public void CloseUP()
    {
        challengeMode.value = false;
    }

    [SerializeField] BoolObject challengeMode;

    [PunRPC]
    public void OnOpponentLeave()
    {
        if (!isGameOver)
        {
            GUIManager.instance.gameOverGUI.OnWin();
            GUIManager.instance.gameOverGUI.Reason.text = "Your opponent left the match !!";
            GUIManager.instance.ShowLog("Your opponent left the match !!", 3);
        }
        InGameGUI.instance.gameStarted = false;
        GUIManager.instance.ShowGameOverGUI();
        Snake[] snakes = GameObject.FindObjectsOfType<Snake>();
        foreach (var snake in snakes)
            Destroy(snake.gameObject);
        print("someone died!");
        if (!reconnect.value)
        {
            CloseUP();
            reconnect.value = true;
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
