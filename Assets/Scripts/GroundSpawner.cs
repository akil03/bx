using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{

	public static GroundSpawner instance;

	public GroundPiece seedGroundPiece;
	public int groundHeight = 100;
	public int groundWidth = 100;
	[SerializeField]
	public Column[] columns;
	[SerializeField]
	public Row[] rows;

	public bool groundSpawned;

	public List<GroundPiece> spawnedGroundPieces;
	public List<GroundPiece> groundPiecesWhereSnakesCanSpawn;

	void Awake ()
	{
		instance = this;
		InitializeRows ();
		InitializeColumns ();
		spawnedGroundPieces = new List<GroundPiece> ();
		CreateGround ();
	}

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	void CreateGround ()
	{

		PopulateGround ();

		foreach (GroundPiece piece in spawnedGroundPieces) {
			piece.Initialize ();
		}

		groundSpawned = true;
		}



	public void PopulateGround ()
	{
		Column lastCreatedColumn = null;

		for (int i = 0; i < groundWidth; i++) {			

			columns [i].indexInColumnsList = i;
			GroundPiece lastSpawnedPiece = null;

			for (int i2 = 0; i2 < groundHeight; i2++) {

				GroundPiece newGroundPiece = (GroundPiece)Instantiate (seedGroundPiece);
				columns [i].groundPieces.Add (newGroundPiece);

				rows [i2].groundPieces.Add (newGroundPiece);
				rows [i2].indexInRowsList = i2;

				Vector3 newPos = newGroundPiece.transform.position;

				if (lastSpawnedPiece != null) {					
					newPos.y = lastSpawnedPiece.transform.position.y - lastSpawnedPiece.bounds ().size.y;
				}

				if (lastCreatedColumn != null) {
					newPos.x = lastCreatedColumn.groundPieces [0].transform.position.x + lastCreatedColumn.groundPieces [0].bounds ().size.x;
				}

				newGroundPiece.transform.position = newPos;	
				newGroundPiece.transform.SetParent (transform);

				newGroundPiece.column = columns [i];
				newGroundPiece.indexInColumn = i2;

				newGroundPiece.row = rows [i2];
				newGroundPiece.indexInRow = i;

		//		if (i == 0 && i == groundWidth - 1)
		//			newGroundPiece.groundRotationAngle = 0;

				spawnedGroundPieces.Add (newGroundPiece);

				newGroundPiece.indexInGrid = spawnedGroundPieces.Count - 1;
				lastSpawnedPiece = newGroundPiece;

				if (i > 5 && i < groundWidth - 5 && i2 > 5 && i2 < groundHeight - 5) {
					groundPiecesWhereSnakesCanSpawn.Add (newGroundPiece);
				}

			}

			lastCreatedColumn = columns [i];		
		}

	}


	public void InitializeColumns ()
	{
		columns = new Column[groundWidth];

		for (int i = 0; i < columns.Length; i++) {
			columns [i] = new Column ();
		}
	}


	public void InitializeRows ()
	{
		rows = new Row[groundHeight];
		for (int i = 0; i < rows.Length; i++) {
			rows [i] = new Row ();
		}
	}

	public GroundPiece[] GetPieceNotOwnedBySnake(Snake snake){

		List<GroundPiece> groundPieces = new List<GroundPiece> ();

		foreach (GroundPiece piece in spawnedGroundPieces) {
			if (piece.snakeOwener != snake && !piece.IsBoundPiece()) {
				groundPieces.Add (piece);
			}
		}

		return groundPieces.ToArray ();
	}


	public GroundPiece GetGroundPieceAt(int x, int y){
		return rows[y].groundPieces[x];       
	}

	public GroundPiece GetRandomGroundPiece(){

		int random = Random.Range (0, spawnedGroundPieces.Count);
		return spawnedGroundPieces [random];

	}

	public void ClearGround(){
		foreach (GroundPiece GP in spawnedGroundPieces) {
			GP.tailPiece.Hide ();
			GP.collectingSnake = null;
			GP.snakeOwener = null;
			GP.pieceWhenCollected.Hide ();
		}
		print ("clear ground called!");
	}

}
