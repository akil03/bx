using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Poly : MonoBehaviour
{


		

	public static void FloodFill (GroundPiece groundPiece, int before, int after)
	{
		if (groundPiece.ownerIDForCheck != before || !groundPiece.tempHasToBeChecked) {
			return;
		}

		groundPiece.ownerIDForCheck = after;

		try {
			FloodFill (groundPiece.GroundPieceOnOvest (), before, after);
		} catch {
		}

		try {
			FloodFill (groundPiece.GroundPieceOnEst (), before, after);
		} catch {
		}

		try {
			FloodFill (groundPiece.GroundPieceOnNorth (), before, after);
		} catch {
		}

		try {
			FloodFill (groundPiece.GroundPieceOnSouth (), before, after);
		} catch {
		}
	}

	public static GroundPiece[] GetGroundPiecesToCheck (Snake snake)
	{
		List<GroundPiece> pieces = new List<GroundPiece> ();

		int topBoundRow = 9999;
		int bottomBoundRow = 0;
		int rightBoundColumn = 0;
		int leftBoundColumn = 9999;

		foreach (GroundPiece piece in snake.ownedGroundPieces) {

			if (piece.row.indexInRowsList < topBoundRow) {
				topBoundRow = piece.row.indexInRowsList;
			}		

			if (piece.row.indexInRowsList > bottomBoundRow) {
				bottomBoundRow = piece.row.indexInRowsList;
			}	

			if (piece.column.indexInColumnsList > rightBoundColumn) {
				rightBoundColumn = piece.column.indexInColumnsList;
			}		

			if (piece.column.indexInColumnsList < leftBoundColumn) {
				leftBoundColumn = piece.column.indexInColumnsList;
			}		
		}

		topBoundRow -= 1;
		bottomBoundRow += 1;
		rightBoundColumn += 1;
		leftBoundColumn -= 1;

		for (int i = topBoundRow; i <= bottomBoundRow; i++) {
			for (int x = leftBoundColumn; x <= rightBoundColumn; x++) {
				pieces.Add (GroundSpawner.instance.rows [i].groundPieces [x]);
			}
		}

		return pieces.ToArray ();
	}

	public static bool CheckIfCanGoInDirection (GroundPiece groundPieceToCheck, Vector3 dir, Snake snake)
	{

		bool canGoInDirection = false;

		if (dir == Vector3.up) {
			try {
				SetReachableBlocks (groundPieceToCheck.GroundPieceOnNorth (), snake);
			} catch {

			}
		}


		if (dir == -Vector3.up) {
			try {
				SetReachableBlocks (groundPieceToCheck.GroundPieceOnSouth (), snake);
			} catch {

			}
		}

		if (dir == Vector3.right) {
			try {
				SetReachableBlocks (groundPieceToCheck.GroundPieceOnEst (), snake);
			} catch {

			}
		}

		if (dir == -Vector3.right) {
			try {
				SetReachableBlocks (groundPieceToCheck.GroundPieceOnOvest (), snake);
			} catch {

			}
		}

	
		foreach (GroundPiece piece in GroundSpawner.instance.rows[1].groundPieces) {
				
			if (piece.tempReachableState == true && piece.collectingSnake != snake) {
				canGoInDirection = true;
			}

		}

		foreach (GroundPiece piece in GroundSpawner.instance.rows[GroundSpawner.instance.rows.Length -2].groundPieces) {
			if (piece.tempReachableState == true && piece.collectingSnake != snake) {				
				canGoInDirection = true;
			}
		}


	
			
		foreach (GroundPiece piece in GroundSpawner.instance.columns[1].groundPieces) {
			if (piece.tempReachableState == true && piece.collectingSnake != snake) {
				canGoInDirection = true;
			}

		}

		foreach (GroundPiece piece in GroundSpawner.instance.columns[GroundSpawner.instance.columns.Length-2].groundPieces) {
			if (piece.tempReachableState == true && piece.collectingSnake != snake) {
				canGoInDirection = true;
			}

		}


		foreach (GroundPiece piece in GroundSpawner.instance.spawnedGroundPieces) {
			piece.tempReachableState = false;
		}

		return canGoInDirection;
	}


	public static void SetReachableBlocks (GroundPiece groundPiece, Snake snake)
	{
		if (groundPiece.tempReachableState == true || groundPiece.collectingSnake == snake || groundPiece.IsBoundPiece ()) {
			return;
		}

		groundPiece.tempReachableState = true;

		try {
			SetReachableBlocks (groundPiece.GroundPieceOnOvest (), snake);
		} catch {
		}

		try {
			SetReachableBlocks (groundPiece.GroundPieceOnEst (), snake);
		} catch {
		}

		try {
			SetReachableBlocks (groundPiece.GroundPieceOnNorth (), snake);
		} catch {
		}

		try {
			SetReachableBlocks (groundPiece.GroundPieceOnSouth (), snake);
		} catch {
		}
	}


}