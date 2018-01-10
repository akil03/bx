using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointFinder {

	public GroundPiece spawnPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public IEnumerator GetValidSpawnPoint ()
	{
		bool validSpawnPointFound = false;

		while (!validSpawnPointFound) {
			GroundPiece seedGroundPiece = GetRandomSpawnPoint ();

			for (int i = seedGroundPiece.indexInGrid; i < GroundSpawner.instance.spawnedGroundPieces.Count; i++) {
				GroundPiece groundPieceToCheck = GroundSpawner.instance.spawnedGroundPieces [i];

				if (IsSpawnPointValid (groundPieceToCheck)) {
					spawnPoint = groundPieceToCheck;
					validSpawnPointFound = true;
					break;
				}
			}
			yield return new WaitForEndOfFrame ();
		}
	}


	public bool IsSpawnPointValid (GroundPiece spawnPoint)
	{
		//return true;

		bool isValid = true;

		if (spawnPoint.indexInColumn <= 5) {
			return false;
		}
		if (spawnPoint.indexInColumn >= GroundSpawner.instance.groundHeight - 5) {
			return false;
		}

		if (spawnPoint.column.indexInColumnsList <= 5) {
			return false;
		}

		if (spawnPoint.column.indexInColumnsList >= GroundSpawner.instance.groundWidth - 5) {
			return false;
		}


		GroundPiece[] piecesAroundSpawnPoint = spawnPoint.groundPiecesAroundMe;

		foreach (GroundPiece piece in piecesAroundSpawnPoint) {
			if (piece.collectingSnake != null || piece.snakeOwener != null) {
				return false;
			}
		}

		return isValid;
	}


	public GroundPiece GetRandomSpawnPoint ()
	{		
		int rand = Random.Range (0, GroundSpawner.instance.groundPiecesWhereSnakesCanSpawn.Count);
		return GroundSpawner.instance.groundPiecesWhereSnakesCanSpawn [rand];
	}

}
