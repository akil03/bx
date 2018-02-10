using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameGUI : MonoBehaviour
{

    public static InGameGUI instance;
    public Text scoreText;
    public Text specialPointsText;
	public Text highscoreText;
	public Text TimerText;
	public Image specialPointsImage;
    

	public GameObject takenGUIDiamond;
	public float TimeRemaining,startTime;
	public int totalGameTime=120;
	public Snake userSnake,opponentSnake; 
	public GameObject[] powerSlots;
	public Sprite Shield, Speed, Freeze, Health, Minishots, Heatseeker;
	public PlayerPanelUI[] PlayerPanel;
	public bool gameStarted;


    void Awake()
    {
        instance = this;

	
			
    }

    // Use this for initialization
    void Start()
    {

    }
	public GameObject NetworkUI;
    // Update is called once per frame
    void Update()
    {
		//UpdateScoreText ();
		//UpdateSpecialPointsText();
		//UpdateHighScoreText();
		UpdateTimer ();
	
		if (PhotonNetwork.GetPing () > 300)
			NetworkUI.SetActive (true);
		else
			NetworkUI.SetActive (false);
    }



	public void UpdateScoreText(){
		string scoreString = ObliusGameManager.instance.TrimPercentage (((100 * (float)ScoreHandler.instance.score) / (float)GroundSpawner.instance.spawnedGroundPieces.Count).ToString ());
		scoreText.text = "" + scoreString +  "%";
	}

	public void UpdateSpecialPointsText(){
		specialPointsText.text = ScoreHandler.instance.specialPoints.ToString();
	}

	public void UpdateHighScoreText(){
		string highscoreString =  ObliusGameManager.instance.TrimPercentage (((100 * (float)ScoreHandler.instance.highScore) / (float)GroundSpawner.instance.spawnedGroundPieces.Count).ToString());
		highscoreText.text = "BEST SCORE: " + highscoreString + "%";
	}

	void UpdateTimer(){
		if (SnakesSpawner.instance.playerSnake != null && SnakesSpawner.instance.playerSnake.isGameStarted && !gameStarted && (PhotonNetwork.connected && PhotonNetwork.room.PlayerCount==maxPlayers)) 
		{
			gameStarted = true;
			TimeRemaining = totalGameTime;
			PowerUpManager.instance.dontSpawn = false;
		}
			
		if (gameStarted) 
		{
			gameStarted = true;
			TimeRemaining = totalGameTime + startTime - Time.time;
			if (TimeRemaining < 0) {
				TimeRemaining = 0;
				if (userSnake.ownedGroundPieces.Count > opponentSnake.ownedGroundPieces.Count) {
					GUIManager.instance.gameOverGUI.OnWin ();
					GUIManager.instance.gameOverGUI.Reason.text = userSnake.name + " covered more blocks than " + opponentSnake.name;
					GUIManager.instance.ShowLog (userSnake.name + " covered more blocks than " + opponentSnake.name);
				} else {
					GUIManager.instance.gameOverGUI.OnLose ();
					GUIManager.instance.gameOverGUI.Reason.text = opponentSnake.name + " covered more blocks than " + userSnake.name;
					GUIManager.instance.ShowLog (userSnake.name + " covered more blocks than " + opponentSnake.name);
				}
				GUIManager.instance.ShowGameOverGUI ();
			}

			TimerText.text = Mathf.RoundToInt (TimeRemaining).ToString ();	
		}
		else if (!gameStarted && (PhotonNetwork.connected)) 
		{
			TimerText.text = totalGameTime.ToString ();
			PowerUpManager.instance.dontSpawn = true;
		}
	}
	public int maxPlayers;
    public void OnTutorialButtonClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();
        GUIManager.instance.ShowTutorialGUI();
    }

    public void OnPauseButtonClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();
        GUIManager.instance.ShowPauseGUI();
    }





//	public void InstantiateTakeGUIObject(Vector3 pos){
//		GameObject newObj = (GameObject)Instantiate (CollectedObject);
//		newObj.transform.SetParent(transform,false);
//		CollectedObject CO = newObj.GetComponent<CollectedObject> ();
//		CO.rect.position = Camera.main.ScreenToWorldPoint (0,Screen.height,0);	
//	}



	public void InstantiateTakeGUIDiamond(Vector3 pos){
		GameObject newObj = (GameObject)Instantiate (takenGUIDiamond);
		newObj.transform.SetParent(transform,false);
		TakenGUIDiamond newDiamond = newObj.GetComponent<TakenGUIDiamond> ();
		newDiamond.rect.position = pos;	
	}


	public void EquipPowerup(){
		for (int i = 0; i < powerSlots.Length; i++)
			powerSlots [i].SetActive (false);

		for (int i = 0; i < userSnake.loadedPowers.Count; i++) {
			powerSlots [i].SetActive (true);
			switch (SnakesSpawner.instance.playerSnake.loadedPowers [i]) {

			case "Speed":
				powerSlots [i].GetComponent <PowerSlot> ().selectedPower.sprite = Speed;
				break;

			case "Shield":
				powerSlots [i].GetComponent <PowerSlot> ().selectedPower.sprite = Shield;
				break;

			case "Health":
				powerSlots [i].GetComponent <PowerSlot> ().selectedPower.sprite = Health;
				break;

			case "Heatseeker":
				powerSlots [i].GetComponent <PowerSlot> ().selectedPower.sprite = Heatseeker;
				break;
			
			case "3Shots":
				powerSlots [i].GetComponent <PowerSlot> ().selectedPower.sprite = Minishots;
				break;

			case "Freeze":
				powerSlots [i].GetComponent <PowerSlot> ().selectedPower.sprite = Freeze;
				break;

			case "Mine":
				powerSlots [i].GetComponent <PowerSlot> ().selectedPower.sprite = Freeze;
				break;


			}
		}
	}

	public void SwitchPower(int selected){
		string temp = userSnake.loadedPowers [0];
		userSnake.loadedPowers [0] = userSnake.loadedPowers [selected];
		userSnake.loadedPowers [selected] = temp;

		EquipPowerup ();
	}

	public void OnPowerUp(){
		userSnake.UsePower ();
	}

}
