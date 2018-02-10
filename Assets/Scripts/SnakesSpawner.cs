using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
public class SnakesSpawner : MonoBehaviour
{


	public static SnakesSpawner instance;

	public int maxEnemiesOnGround = 10;

	public List<GameObject> randomSnakeMeshes;
	public List<GameObject> shoppableSnakeMeshes;
	public List<GameObject> usableSnakeMeshes;
	public List<Color> usableColors;
	public List<Sprite> usableTiles;
	public SpriteAtlas tileAtlas;
	public GameObject snakeMeshAssignedToPlayer;
	public GameObject snakeMeshAssignedToEnemy;

	public GameObject[] PlayerMeshes;

	public Snake playerSnake,enemySnake;

	public int spawnedEnemiesCount;
	public List<Snake> spawnedSnakes;
	public int playerLives,enemyLives=3;

	public int selectedMeshIndex,selectedColourIndex,selectedTileIndex;
	public GameObject tempMesh, previewMeshContainer;

	public string playerName;
	public int HealthLvl,LifeLvl,SpeedLvl,Power_RocketLvl,Power_ShotsLvl,Power_SpeedLvl,Power_HPLvl,Power_ShieldLvl;
	public int HealthValue, LifeValue, PowerRocketValue, PowerShotsValue, PowerSpeedValue, PowerHPValue, PowerShieldValue;
	public float SpeedValue;

	void Awake ()
	{
		instance = this;	
		spawnedSnakes = new List<Snake> ();
	}

	// Use this for initialization
	void Start ()
	{
		//selectedMeshIndex = Random.Range (0, usableSnakeMeshes.Count);
		ShowPreview ();
		//LoadUsableMeshesFromResources ();
		//snakeMeshAssignedToPlayer = GetMeshUsedByPlayer ();
		//StartCoroutine (SpawnRoutine ());
	}

	public void KillAllSnakes(){
	//	if(playerSnake)
	//		playerSnake.haveToDie = true;
		try{
		foreach (Snake S in spawnedSnakes)
			S.ForceDie ();

			spawnedSnakes.Clear ();
		}
		catch{
			print("buggy snake");
		}
	}

	public void KillAllNetworkSnakes(){
		try{
			foreach (Snake S in spawnedSnakes){
				spawnedSnakes.Remove(S);
				Destroy(S.gameObject);
			}
		}
		catch{
			print("buggy snake");
		}
	}
		

	public void SpawnPlayer ()
	{
		StartCoroutine (SpawnNewSnake (true,playerLives));
	}



	public void SpawnBot()
	{
//		if(usableSnakeMeshes.Contains (snakeMeshAssignedToPlayer))
//			usableSnakeMeshes.Remove (snakeMeshAssignedToPlayer);
		
		StartCoroutine (SpawnNewSnake (false,playerLives));
	}

	public IEnumerator SpawnRoutine ()
	{

		while (true) {

			if (spawnedEnemiesCount < maxEnemiesOnGround) {
				yield return StartCoroutine (SpawnNewSnake (false,playerLives));					
			}
			yield return new WaitForEndOfFrame ();
		}

	}


	public void ShowPreview(){
		if (tempMesh)
			Destroy (tempMesh);

		selectedMeshIndex++;
		if (selectedMeshIndex == usableSnakeMeshes.Count)
			selectedMeshIndex = 0;
		snakeMeshAssignedToPlayer = usableSnakeMeshes [selectedMeshIndex];
		tempMesh = Instantiate (usableSnakeMeshes [selectedMeshIndex],previewMeshContainer.transform);
		tempMesh.transform.localPosition = Vector3.zero;
		tempMesh.transform.localRotation = Quaternion.identity;
		tempMesh.transform.localScale = Vector3.one;

	}

	public void ShowPrev(){
		if (tempMesh)
			Destroy (tempMesh);

		snakeMeshAssignedToPlayer = usableSnakeMeshes [selectedMeshIndex];
		tempMesh = Instantiate (usableSnakeMeshes [selectedMeshIndex],previewMeshContainer.transform);
		tempMesh.transform.localPosition = Vector3.zero;
		tempMesh.transform.localRotation = Quaternion.identity;
		tempMesh.transform.localScale = Vector3.one;
		selectedMeshIndex--;
		if (selectedMeshIndex <0 )
			selectedMeshIndex = usableSnakeMeshes.Count-1;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	//	startTimer.text = PhotonNetwork.GetPing ().ToString();

	}


	public IEnumerator SpawnNewSnake (bool isPlayer,int playerLives)
	{
		/*
		if (!isPlayer) {
			spawnedEnemiesCount++;
			yield return StartCoroutine (GetValidSpawnPoint ());
		} else {
			spawnPoint = GetRandomSpawnPoint ();
		}
		*/

		if (!isPlayer) {
			spawnedEnemiesCount++;
		}
		InGameGUI.instance.gameStarted = true;

		SpawnPointFinder spawnPointFinder = new SpawnPointFinder ();
	//	yield return StartCoroutine (spawnPointFinder.GetValidSpawnPoint ());
		yield return new WaitForEndOfFrame();

		GameObject go = (GameObject)Resources.Load ("Snake");
		go = GameObject.Instantiate (go);

		Snake newSnake = go.GetComponent<Snake> ();

		if (isPlayer) {
			newSnake.isBot = false;
			playerSnake = newSnake;
			InGameGUI.instance.userSnake = newSnake;
			InGameGUI.instance.PlayerPanel [0].SelectedSnake = newSnake;
			InGameGUI.instance.EquipPowerup ();
			if (PlayerPrefs.GetInt ("TutorialComplete")!=1)
				newSnake.Lives = playerLives;
			else
				newSnake.Lives = LifeValue;					
			newSnake.playerID = 1;
			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [447]; //area/2 - 3
			playerSnake = newSnake;
		} else {
			newSnake.isBot = true;
			InGameGUI.instance.opponentSnake = newSnake;
			InGameGUI.instance.PlayerPanel [1].SelectedSnake = newSnake;
			if (PlayerPrefs.GetInt ("TutorialComplete")!=1)
				newSnake.Lives = 3;
			else
				newSnake.Lives = LifeValue;
			newSnake.playerID = 2;
			enemySnake = newSnake;
			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [422]; // area/2 - gridlength +2
		}

		SetSnakeMesh (newSnake);

		newSnake.normalSpeed = SpeedValue;
		newSnake.maxHP = HealthValue;
		newSnake.SetSpeed ();

		newSnake.Initialize ();

		newSnake.lastReachedGroundPiece = spawnPointFinder.spawnPoint;
		newSnake.SetFirstOwnedGroundPieces (spawnPointFinder.spawnPoint);

		Vector3 newPos = spawnPointFinder.spawnPoint.transform.position;
		newPos.z = newSnake.transform.position.z;
		newSnake.transform.position = newPos;
		newSnake.originalPos = transform.position;

		newSnake.gameObject.name = newSnake.name;
		spawnedSnakes.Add (newSnake);


		if (isPlayer)
			InGameGUI.instance.PlayerPanel [0].Init ();
		else
			InGameGUI.instance.PlayerPanel [1].Init ();
		
		StartCoroutine (newSnake.StartMove ());
	}

	float StartTime;
	public void RespawnSnake(Snake snake){
		SpawnPointFinder spawnPointFinder = new SpawnPointFinder ();
		if (snake.playerID == 1) {
			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [447];
			snake.nextMoveDirection = snake.currentMoveDirection = transform.up;

		} else {
			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [422];
			snake.nextMoveDirection = snake.currentMoveDirection = -transform.up;
		}
		snake.tailGroundPieces.Clear ();
		snake.lastReachedGroundPiece = spawnPointFinder.spawnPoint;
		snake.groundPieceToReach = null;
		//snake.lastReachedGroundPiece = null;
		snake.SetFirstOwnedGroundPieces (spawnPointFinder.spawnPoint);
		snake.isCollectingNewGroundPieces = false;
		snake.haveToDie = false;
		snake.isDead = false;
		Vector3 newPos = spawnPointFinder.spawnPoint.transform.position;
		newPos.z = snake.transform.position.z;
		snake.transform.position = newPos;
		print (snake.transform.name+" spawned at "+newPos);
		snake.originalPos = transform.position;
		snake.currentHP = snake.maxHP;
		snake.isHeadOn = false;

		snake.snakeMeshContainer.transform.localScale = Vector3.one;

		//snake.SetSpeed ();
		//snake.DisableFreezeHit ();
		snake.DeactivateShield ();
		//snake.DeactivateSpeed ();
		if (snake.isLocal) { 
			snake._networkSnake.shouldTransmit = true;
			snake._networkSnake.currentHP = snake._networkSnake.MaxHP;
		}
//		else
			snake.transform.localScale = Vector3.one;


		StartCoroutine (snake.StartMove ());

	}

	public void RespawnNetworkSnake(Snake snake){
		
		SpawnPointFinder spawnPointFinder = new SpawnPointFinder ();


		GameObject go = (GameObject)Resources.Load ("Snake");
		go = GameObject.Instantiate (go);

		Snake newSnake = go.GetComponent<Snake> ();
		go.transform.position = new Vector3 (transform.position.x, transform.position.y, -0.75f);


		newSnake.isBot = false;
		playerSnake = newSnake;
		InGameGUI.instance.userSnake = newSnake;
		newSnake.loadedPowers = snake.loadedPowers;
		//InGameGUI.instance.PlayerPanel [0].SelectedSnake = newSnake;
		InGameGUI.instance.EquipPowerup ();

		newSnake.isLocal = true;
		newSnake.Lives = snake.Lives;
		newSnake.normalSpeed = snake.normalSpeed;
	
		if(snake.playerID==1){

			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [447];
			newSnake.playerID = 1;
			newSnake.nextMoveDirection = newSnake.currentMoveDirection = transform.up;
			//SetSnakeMeshMultiplayer (newSnake,1);//area/2 - 3
		} else {

			newSnake.playerID = 2;
			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [422]; // area/2 - gridlength +2
			//SetSnakeMeshMultiplayer (newSnake, 2);
			newSnake.nextMoveDirection = newSnake.currentMoveDirection = -transform.up;
			//startTimer.text = StartTime.ToString ();
		}



		GameObject meshToUse;

		meshToUse = snakeMeshAssignedToPlayer;


		newSnake.snakeMeshContainer.SetSnakeMesh (meshToUse);


	
		newSnake.spriteColor = snake.spriteColor;

		newSnake.collectedPieceSprite = snake.collectedPieceSprite;



	

		newSnake.Initialize ();




		newSnake._networkSnake = snake._networkSnake;



		newSnake.lastReachedGroundPiece = spawnPointFinder.spawnPoint;
		newSnake.SetFirstOwnedGroundPieces (spawnPointFinder.spawnPoint);

		Vector3 newPos = spawnPointFinder.spawnPoint.transform.position;
		newPos.z = newSnake.transform.position.z;
		newSnake.transform.position = newPos;
		newSnake.originalPos = transform.position;

		newSnake.gameObject.name = newSnake.name;
		spawnedSnakes.Add (newSnake);

		InGameGUI.instance.userSnake = newSnake;
		InGameGUI.instance.PlayerPanel [0].SelectedSnake = newSnake;
		InGameGUI.instance.PlayerPanel [0].Init ();


		StartCoroutine (newSnake.StartMove ());

		snake._networkSnake.shouldTransmit = true;
		snake._networkSnake.currentHP = snake._networkSnake.MaxHP;
		snake._networkSnake.Player = newSnake;

		//newSnake.ownedGroundPieces.Clear ();
		foreach (GroundPiece GP in snake.ownedGroundPieces) {
			GP.snakeOwener = newSnake;
			newSnake.ownedGroundPieces.Add (GP);
		}


		GetNotifiedNetworkDeath (snake);
		Destroy (snake.gameObject);
	}

	public void FixSpawnBug(Snake snake){
		SpawnPointFinder spawnPointFinder = new SpawnPointFinder ();
		if (snake.playerID == 1) {
			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [447];
			snake.nextMoveDirection = snake.currentMoveDirection = transform.up;

		} else {
			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [422];
			snake.nextMoveDirection = snake.currentMoveDirection = -transform.up;
		}
		snake.tailGroundPieces.Clear ();
		snake.lastReachedGroundPiece = spawnPointFinder.spawnPoint;
		snake.groundPieceToReach = null;

		snake.SetFirstOwnedGroundPieces (spawnPointFinder.spawnPoint);
		snake.isCollectingNewGroundPieces = false;

		snake.transform.position = snake.originalPos;

		snake.snakeMeshContainer.transform.localScale = Vector3.one;
		snake.transform.localScale = Vector3.one;
	}


	public void CreateNetworkSnake(int playerNo){

		SpawnPointFinder spawnPointFinder = new SpawnPointFinder ();


		GameObject go = (GameObject)Resources.Load ("Snake");
		go = GameObject.Instantiate (go);

		Snake newSnake = go.GetComponent<Snake> ();
		go.transform.position = new Vector3 (transform.position.x, transform.position.y, -0.75f);

	
		newSnake.isBot = false;
		playerSnake = newSnake;
		InGameGUI.instance.userSnake = newSnake;
		//InGameGUI.instance.PlayerPanel [0].SelectedSnake = newSnake;
		InGameGUI.instance.EquipPowerup ();

		newSnake.isLocal = true;
		newSnake.Lives = LifeValue;
		newSnake.normalSpeed = SpeedValue;
		StartTime = 0;
		if(playerNo==1){
			PhotonNetwork.Instantiate ("Server",Vector3.zero,Quaternion.identity,new byte());
//			PowerUpManager.instance.dontSpawn = false;
			//PowerUpManager.instance.SpawnNetworkRunePowerup();


			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [447];
			newSnake.playerID = 1;
			newSnake.nextMoveDirection = newSnake.currentMoveDirection = transform.up;
			SetSnakeMeshMultiplayer (newSnake,1);//area/2 - 3
		} else {

			newSnake.playerID = 2;
			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [422]; // area/2 - gridlength +2
			StartTime = float.Parse(PhotonNetwork.time.ToString())+3.5f;
			SetSnakeMeshMultiplayer (newSnake, 2);
			newSnake.nextMoveDirection = newSnake.currentMoveDirection = -transform.up;
			StartCoroutine (StartMultiplayerGame (StartTime));
			//startTimer.text = StartTime.ToString ();
		}
		newSnake.speed = 0;

		GameObject NS =  PhotonNetwork.Instantiate("NetworkSnake", Vector3.zero, Quaternion.identity, new byte());

		newSnake.Initialize ();


		NS.GetComponent<PlayerInfo> ().AssignValues (playerNo, newSnake.name, newSnake.Lives, HealthValue,SpeedValue, selectedMeshIndex, selectedTileIndex,selectedColourIndex, newSnake, StartTime);

		newSnake._networkSnake = NS.GetComponent<PlayerInfo> ();



		newSnake.lastReachedGroundPiece = spawnPointFinder.spawnPoint;
		newSnake.SetFirstOwnedGroundPieces (spawnPointFinder.spawnPoint);

		Vector3 newPos = spawnPointFinder.spawnPoint.transform.position;
		newPos.z = newSnake.transform.position.z;
		newSnake.transform.position = newPos;
		newSnake.originalPos = transform.position;

		newSnake.gameObject.name = newSnake.name;
		spawnedSnakes.Add (newSnake);

		InGameGUI.instance.userSnake = newSnake;
		InGameGUI.instance.PlayerPanel [0].SelectedSnake = newSnake;
		InGameGUI.instance.PlayerPanel [0].Init ();
	

		StartCoroutine (newSnake.StartMove ());

	}


//	public void CreateNetworkSnake(int playerNo){
//		//    if (playerSnake)
//		//      return;
//
//
//		SpawnPointFinder spawnPointFinder = new SpawnPointFinder ();
//
//
//		GameObject go = (GameObject)Resources.Load ("Snake");
//		go = GameObject.Instantiate (go);
//
//		Snake newSnake = go.GetComponent<Snake> ();
//		go.transform.position = new Vector3 (transform.position.x, transform.position.y, -0.75f);
//
//		newSnake.isBot = false;
//		playerSnake = newSnake;
//		InGameGUI.instance.userSnake = newSnake;
//		//InGameGUI.instance.PlayerPanel [0].SelectedSnake = newSnake;
//		InGameGUI.instance.EquipPowerup ();
//
//		newSnake.isLocal = true;
//		StartTime = 0;
//		if(playerNo==1){
//
//			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [447];
//			newSnake.playerID = 1;//area/2 - 3
//		} else {
//
//			newSnake.playerID = 2;
//			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [422]; // area/2 - gridlength +2
//			StartTime = float.Parse(PhotonNetwork.time.ToString())+3;
//			StartCoroutine (StartMultiplayerGame (StartTime));
//			//startTimer.text = StartTime.ToString ();
//		}
//		newSnake.speed = 0;
//
//		SetSnakeMeshMultiplayer (newSnake);
//
//		if (PhotonNetwork.isMasterClient) {
//			{
//				PhotonNetwork.Instantiate ("Server",Vector3.zero,Quaternion.identity,new byte());
//			}
//		}
//		GameObject NS =  PhotonNetwork.Instantiate("NetworkSnake", Vector3.zero, Quaternion.identity, new byte());
//
//		newSnake.Initialize ();
//
//		NS.GetComponent<PlayerInfo> ().AssignValues (playerNo, newSnake.name, 1, 350, selectedMeshIndex, selectedTileIndex,selectedColourIndex, newSnake, StartTime);
//
//		newSnake._networkSnake = NS.GetComponent<PlayerInfo> ();
//
//
//		newSnake.lastReachedGroundPiece = spawnPointFinder.spawnPoint;
//		newSnake.SetFirstOwnedGroundPieces (spawnPointFinder.spawnPoint);
//
//		Vector3 newPos = spawnPointFinder.spawnPoint.transform.position;
//		newPos.z = newSnake.transform.position.z;
//		newSnake.transform.position = newPos;
//		newSnake.originalPos = transform.position;
//
//		newSnake.gameObject.name = newSnake.name;
//		spawnedSnakes.Add (newSnake);
//
//		InGameGUI.instance.userSnake = newSnake;
//		InGameGUI.instance.PlayerPanel [0].SelectedSnake = newSnake;
//		InGameGUI.instance.PlayerPanel [0].Init ();
//
//
//		StartCoroutine (newSnake.StartMove ());
//
//	}
//

	public Text startTimer,DisplayStart;
	public IEnumerator StartMultiplayerGame(float startTime)
	{
		GUIManager.instance.matchLoading.Hide (false);
		int temptimer;
		while (PhotonNetwork.time < startTime) {
			yield return null;
			if ((startTime - PhotonNetwork.time) > 2)
				DisplayStart.text = "3";
			else if ((startTime - PhotonNetwork.time) > 1)
				DisplayStart.text = "2";
			else if ((startTime - PhotonNetwork.time) > 0)
				DisplayStart.text = "1";

		}
		InGameGUI.instance.startTime = Time.time;
		DisplayStart.text = "GO !!";
		if(PhotonNetwork.isMasterClient)
			PowerUpManager.instance.StartNetworkPower();


		if (playerSnake) 
		{
			playerSnake.SetSpeed ();
			playerSnake.isGameStarted = true;
		}
		if (enemySnake) {
			enemySnake.SetSpeed ();
			enemySnake.isGameStarted = true;
		}
		yield return new WaitForSeconds (1f);
		DisplayStart.text = "";
		//startTimer.text = startTime.ToString ()+"\n"+PhotonNetwork.time.ToString();
	}



	public void CreateSnakeFromInfo(int playerNo,string _playerName, int _lives, int _maxhp, float _baseSpeed, int _meshtype, int _tiletype, int _colortype, bool isLocal,PlayerInfo NS){
//		if (enemySnake)
//			return;


		SpawnPointFinder spawnPointFinder = new SpawnPointFinder ();


		GameObject go = (GameObject)Resources.Load ("Snake");
		go = GameObject.Instantiate (go);

		Snake newSnake = go.GetComponent<Snake> ();
		go.transform.position = new Vector3 (transform.position.x, transform.position.y, -0.75f);

		newSnake.isBot = false;
		newSnake.Lives = _lives;
		newSnake.normalSpeed = _baseSpeed;

		InGameGUI.instance.userSnake = newSnake;
		//InGameGUI.instance.PlayerPanel [0].SelectedSnake = newSnake;
		InGameGUI.instance.EquipPowerup ();

		if(playerNo==1){

			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [447];
			newSnake.playerID = 1;//area/2 - 3
			newSnake.nextMoveDirection = newSnake.currentMoveDirection = transform.up;
		} else {

			newSnake.playerID = 2;
			spawnPointFinder.spawnPoint = GroundSpawner.instance.spawnedGroundPieces [422]; // area/2 - gridlength +2
			newSnake.nextMoveDirection = newSnake.currentMoveDirection = -transform.up;
		}

		SetSnakeMesh (newSnake,_meshtype,_tiletype,_colortype);
		newSnake.speed = 0;
		enemySnake = newSnake;

		//playerSnake.speed = 1.5f;


		newSnake._networkSnake = NS;
		NS.Player = newSnake;
		newSnake.Initialize ();

		newSnake.lastReachedGroundPiece = spawnPointFinder.spawnPoint;
		newSnake.SetFirstOwnedGroundPieces (spawnPointFinder.spawnPoint);

		Vector3 newPos = spawnPointFinder.spawnPoint.transform.position;
		newPos.z = newSnake.transform.position.z;
		newSnake.transform.position = newPos;
		newSnake.originalPos = transform.position;

		newSnake.name = _playerName;
		newSnake.gameObject.name = newSnake.name;
		spawnedSnakes.Add (newSnake);

		InGameGUI.instance.opponentSnake = newSnake;
		InGameGUI.instance.PlayerPanel [1].SelectedSnake = newSnake;
		InGameGUI.instance.PlayerPanel [1].Init ();

		StartCoroutine (newSnake.StartMove ());

	}


	void StartGame(){

	}

	public void SetPlayerSnakeColor ()
	{
		Snake snake = FindObjectOfType<Snake> ();
		SetSnakeMesh (snake);
	}

	public void  SetSnakeMesh (Snake snake)
	{	GameObject meshToUse = null;
		if (snake.isBot) {
			int random = Random.Range (0, usableSnakeMeshes.Count);
			meshToUse = usableSnakeMeshes [random];

			//meshToUse = snakeMeshAssignedToEnemy;

		} else {
			int random = Random.Range (0, usableSnakeMeshes.Count);
			//meshToUse = usableSnakeMeshes [random];
			meshToUse = snakeMeshAssignedToPlayer;
		}

		snake.snakeMeshContainer.SetSnakeMesh (meshToUse);
		usableSnakeMeshes.Remove (meshToUse);

		int rand = Random.Range (0, usableColors.Count);
		snake.spriteColor = usableColors [rand];
		usableColors.RemoveAt (rand);

		selectedColourIndex = rand;


		rand = Random.Range (0, usableTiles.Count);
		snake.collectedPieceSprite = tileAtlas.GetSprite (usableTiles [rand].name); 
		//usableTiles.RemoveAt (rand);
		selectedTileIndex = rand;
	}

	public void  SetSnakeMeshMultiplayer (Snake snake,int playerID)
	{	
		GameObject meshToUse = null;

		meshToUse = snakeMeshAssignedToPlayer;


		snake.snakeMeshContainer.SetSnakeMesh (meshToUse);


		int rand;

		if(playerID==1)
			rand = Random.Range (0, 4);
		else
			rand = Random.Range (4, 8);
		snake.spriteColor = usableColors [rand];
	//	usableColors.RemoveAt (rand);

		selectedColourIndex = rand;


		rand = Random.Range (0, usableTiles.Count);
		snake.collectedPieceSprite = tileAtlas.GetSprite (usableTiles [rand].name); 
		//usableTiles.RemoveAt (rand);
		selectedTileIndex = rand;
	}



	public void  SetSnakeMesh (Snake snake,int meshType, int tileType, int colorType)
	{	
		GameObject meshToUse = null;


		meshToUse = usableSnakeMeshes [meshType];

		snake.snakeMeshContainer.SetSnakeMesh (meshToUse);
		//usableSnakeMeshes.Remove (meshToUse);

		int rand = Random.Range (0, usableColors.Count);
//
		while (colorType == rand) {
			rand = Random.Range (0, usableColors.Count);
		}
//
		if (colorType == selectedColourIndex)
			colorType = rand;

		snake.spriteColor = usableColors [colorType];
	//	usableColors.RemoveAt (rand);



		rand = Random.Range (0, usableTiles.Count);
		//snake.collectedPieceSprite = usableTiles [tileType];
		snake.collectedPieceSprite = tileAtlas.GetSprite (usableTiles [rand].name); 

	}

	public void GetNotifiedNetworkDeath(Snake snake){
	//	usableColors.Add (snake.spriteColor);

		spawnedSnakes.Remove (snake);
	}



	public void GetNotifiedSnakeDeath (Snake snake)
	{
		usableSnakeMeshes.Add (snake.snakeMeshContainer.snakeMesh);

		usableColors.Add (snake.spriteColor);

		//usableTiles.Add (snake.collectedPieceSprite);

		spawnedSnakes.Remove (snake);

		if (snake.isBot) {
			spawnedEnemiesCount--;
		}
	}

	public void LoadUsableMeshesFromResources(){

		Object[] meshesInResources = Resources.LoadAll("SnakesModels/Random");

		foreach (Object obj in meshesInResources) {
			GameObject mesh = obj as GameObject;
			randomSnakeMeshes.Add (mesh);
			usableSnakeMeshes.Add (mesh);
		}

		meshesInResources = Resources.LoadAll("SnakesModels/Shoppable");

		foreach (Object obj in meshesInResources) {
			GameObject mesh = obj as GameObject;
			shoppableSnakeMeshes.Add (mesh);
			usableSnakeMeshes.Add (mesh);
		}

	}

	public GameObject GetMeshUsedByPlayer(){
		ShopItem shopItemToUse = ShopHandler.instance.shopItemToUse;
	
		string shopItemName = shopItemToUse.name;

		if (shopItemName == "Random Color") {
			int rand = Random.Range (0, randomSnakeMeshes.Count);
			shopItemName = randomSnakeMeshes [rand].name;
		}

		GameObject meshToRemove = null;
		foreach (GameObject mesh in usableSnakeMeshes) {			
			if (mesh.name == shopItemName) {
				meshToRemove = mesh;
				break;
			}
		}

		Debug.Log ("USABLE SNAKE MESH SIZE BEFORE = " + usableSnakeMeshes.Count);

		usableSnakeMeshes.Remove (meshToRemove);

		Debug.Log ("USABLE SNAKE MESH SIZE AFTER = " + usableSnakeMeshes.Count);

		return meshToRemove;
	}

	public class PlayerProperties{


	}
}
