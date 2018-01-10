using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAI : MonoBehaviour
{

	Snake snake;

	public int groundPiecesToCollectOnCurrentMove;
	public int groundPiecesCollectedSinceLastMove;

	public GroundPiece lastReachedGroundPiece;
	public int timesToGoInTheSameDirection;

	public int movesBeforeReturninToOwnedPieces;

	public bool isReachingUncollectedPiece;


	[Header ("Enemy Killing Behaviour Options")]
	public float distanceToTriggerEnemySnakeKilling = 3.5f;
	public Vector3 enemyTailDirection;
	public bool enemySnakeTailNearMe;
	public bool isGoingToKillEnemySnake;
	public Snake targetEnemySnakeToKill;
	public GroundPiece targetEnemySnakeTailGroundPiece;


	[Header ("IA Options")]
	public bool onlyKillEnemyIfPlayer;
	public bool useImprovedIA; // IA NEVER KILLS HIMSELF

	void Awake ()
	{
		snake = GetComponent<Snake> ();
	}

	// Use this for initialization
	void Start ()
	{
		//StartCoroutine (IARoutine ());
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public void Notify (GroundPiece reachedGroundPiece, bool isCollectingNewGroundPieces)
	{

		NewGroundPieceReached (reachedGroundPiece);

		try{
		if (isGoingToKillEnemySnake) {			
			if (targetEnemySnakeToKill != targetEnemySnakeTailGroundPiece.collectingSnake || targetEnemySnakeToKill == null) {
				isGoingToKillEnemySnake = false;
				targetEnemySnakeToKill = null;
				targetEnemySnakeTailGroundPiece = null;
				movesBeforeReturninToOwnedPieces = 0;
			}
		}
		}
		catch{
			print ("bug");
			snake.haveToDie = true;
			snake.ReasonDeath = snake.name + " decided to kill himself";
			GUIManager.instance.ShowLog (snake.name + " decided to kill himself");
		}


			
		if (IsAnotherSnakeTailNearMe (distanceToTriggerEnemySnakeKilling, out enemyTailDirection, out targetEnemySnakeTailGroundPiece)) {

			if (onlyKillEnemyIfPlayer) {
				if (!targetEnemySnakeTailGroundPiece.collectingSnake.isBot) {
					targetEnemySnakeToKill = targetEnemySnakeTailGroundPiece.collectingSnake;		
					isGoingToKillEnemySnake = true;
				}
			} else {
				targetEnemySnakeToKill = targetEnemySnakeTailGroundPiece.collectingSnake;		
				isGoingToKillEnemySnake = true;
			}
		}

			

		if (!isGoingToKillEnemySnake) {
			
			if (isCollectingNewGroundPieces) {
				
				if (movesBeforeReturninToOwnedPieces > 0) {			
					BehaviourWhenCollectingGroundPieces ();
				} else {		
					BehaviourWhenReturninToOwnedGroundPieces ();
				}

			} else {
				BehaviourWhenInsideOwnedGroundPieces ();
			}

		} else {
			BehaviourWhenGoingToKillEnemySnake ();
		}


	}

	public void BehaviourWhenCollectingGroundPieces ()
	{		
		if (movesBeforeReturninToOwnedPieces > 0) {			
			HashSet<Vector3> unwantedDirections = GetDirectionsToExclude ();
			unwantedDirections.Add (-snake.currentMoveDirection);

			if (timesToGoInTheSameDirection <= 0 || unwantedDirections.Contains (snake.currentMoveDirection)) {
		
				timesToGoInTheSameDirection = Random.Range (0, 9);

				Vector3 newDirection = GetNewDirection (unwantedDirections);
				snake.MoveToDirection (newDirection);
				movesBeforeReturninToOwnedPieces--;		
			} else {
				timesToGoInTheSameDirection--;
			}
		}
	}

	public void BehaviourWhenReturninToOwnedGroundPieces ()
	{
	
 		Vector3 pointToReach = centerOfOwnedGroundPieces (); 
		MoveToPoint (pointToReach);

	}

	public void BehaviourWhenInsideOwnedGroundPieces ()
	{
		HashSet<Vector3> unwantedDirections = GetDirectionsToExclude ();

		unwantedDirections.Add (-snake.currentMoveDirection);

		if (unwantedDirections.Contains (snake.currentMoveDirection)) {
			isReachingUncollectedPiece = false;
		}

		if (!isReachingUncollectedPiece) {
			//GroundPiece[] notOwnedGroundPieces = GroundSpawner.instance.GetPieceNotOwnedBySnake (snake);
			//int random = Random.Range (0, notOwnedGroundPieces.Length);
			//GroundPiece groundPieceToReach = notOwnedGroundPieces [random];
			GroundPiece groundPieceToReach = GroundSpawner.instance.GetRandomGroundPiece();
			MoveToPoint (groundPieceToReach.transform.position);
			isReachingUncollectedPiece = true;
		}
	}


	public void BehaviourWhenGoingToKillEnemySnake ()
	{
		snake.MoveToDirection (enemyTailDirection);
	}

	public void MoveToPoint (Vector3 pointToReach)
	{

		HashSet<Vector3> unwantedDirections = GetDirectionsToExclude ();

		unwantedDirections.Add (-snake.currentMoveDirection);

		if (pointToReach.y < transform.position.y) {
			unwantedDirections.Add (transform.up);
		}

		if (pointToReach.y > transform.position.y) {
			unwantedDirections.Add (-transform.up);
		}


		if (pointToReach.x < transform.position.x) {
			unwantedDirections.Add (transform.right);
		}


		if (pointToReach.x > transform.position.x) {
			unwantedDirections.Add (-transform.right);
		}


		/*
		if (unwantedDirections.Count >= 4) {		
			unwantedDirections = GetDirectionsToExclude ();
			unwantedDirections.Add (-snake.currentMoveDirection);
		}
*/


		Vector3 newDirection = GetNewDirection (unwantedDirections);
		snake.MoveToDirection (newDirection);
	}



	public void NewGroundPieceReached (GroundPiece groundPiece)
	{
		lastReachedGroundPiece = groundPiece;
	}

	public Vector3 GetNewDirection (HashSet<Vector3> directionsToExclude)
	{
		Vector3 up = transform.up;
		Vector3 down = -transform.up;
		Vector3 right = transform.right;
		Vector3 left = -transform.right;

		List<Vector3> directions = new List<Vector3> ();


		directions.Add (up);
		directions.Add (down);
		directions.Add (right);
		directions.Add (left);


		foreach (Vector3 dir in directionsToExclude) {
			directions.Remove (dir);
		}


		if (directions.Count == 0) {
			directionsToExclude = new HashSet<Vector3> ();
			directionsToExclude.Add (-snake.currentMoveDirection);
			return GetNewDirection (directionsToExclude);
		}

		int random = Random.Range (0, directions.Count);
		Vector3 direction = directions [random];


		if (useImprovedIA) {
			bool canGoInDirection = false;
			while (!canGoInDirection) {	
		
				if (Poly.CheckIfCanGoInDirection (lastReachedGroundPiece, direction, snake)) {
					canGoInDirection = true;
				} else {				
					directions.Remove (direction);
					random = Random.Range (0, directions.Count);
					direction = directions [random];
				}
			}
		}

		return direction;
	}

	public Vector3 centerOfOwnedGroundPieces ()
	{
		
		Vector3 vectorSum = new Vector3 ();

		foreach (GroundPiece piece in snake.ownedGroundPieces) {
			vectorSum += piece.transform.position;
		}

		vectorSum = vectorSum / snake.ownedGroundPieces.Count;
		return vectorSum;
	}


	public HashSet<Vector3> GetDirectionsToExclude ()
	{

		Vector3 up = transform.up;
		Vector3 down = -transform.up;
		Vector3 right = transform.right;
		Vector3 left = -transform.right;

		HashSet<Vector3> dirToExclude = new HashSet<Vector3> ();

		try {
			if (lastReachedGroundPiece.GroundPieceOnEst ().collectingSnake == snake || lastReachedGroundPiece.GroundPieceOnEst ().IsBoundPiece ()) {
				dirToExclude.Add (transform.right);
			}
		} catch {
			dirToExclude.Add (transform.right);
		}


		try {
			if (lastReachedGroundPiece.GroundPieceOnOvest ().collectingSnake == snake || lastReachedGroundPiece.GroundPieceOnOvest ().IsBoundPiece ()) {
				dirToExclude.Add (-transform.right);
			}
		} catch {
			dirToExclude.Add (-transform.right);
		}

		try {
			if (lastReachedGroundPiece.GroundPieceOnNorth ().collectingSnake == snake || lastReachedGroundPiece.GroundPieceOnNorth ().IsBoundPiece ()) {
				dirToExclude.Add (transform.up);
			}
		} catch {
			dirToExclude.Add (transform.up);
		}

		try {
			if (lastReachedGroundPiece.GroundPieceOnSouth ().collectingSnake == snake || lastReachedGroundPiece.GroundPieceOnSouth ().IsBoundPiece ()) {
				dirToExclude.Add (-transform.up);
			}
		} catch {
			dirToExclude.Add (-transform.up);
		}
			

		return dirToExclude;
	}

	public void Reset ()
	{
		movesBeforeReturninToOwnedPieces = Random.Range (1, 2);
		isReachingUncollectedPiece = false;
	}



	public bool IsAnotherSnakeTailNearMe (float maxDistance, out Vector3 directionToReachEnemySnakeTail, out GroundPiece enemyTailGroundPieceToReach)
	{

		List<GroundPiece> possibleEnemyTailGroundPieces = new List<GroundPiece> ();
		List<Vector3> possibleDirections = new List<Vector3> ();

		directionToReachEnemySnakeTail = Vector3.zero;
		enemyTailGroundPieceToReach = null;

		if (lastReachedGroundPiece.HasGroundPieceOnEast ()) {

			for (int i = lastReachedGroundPiece.indexInRow + 1; i < lastReachedGroundPiece.row.groundPieces.Count; i++) {

				GroundPiece pieceToCheck = lastReachedGroundPiece.row.groundPieces [i]; 

				if (Vector3.Distance (pieceToCheck.transform.position, transform.position) > maxDistance) {
					break;
				}

				if (pieceToCheck.collectingSnake != null) {
					if (pieceToCheck.collectingSnake == snake) {
						break;
					}

					if (pieceToCheck.collectingSnake != snake) {
						possibleEnemyTailGroundPieces.Add (pieceToCheck);
						possibleDirections.Add (transform.right);
						break;
					}
				}
			}
		}

		if (lastReachedGroundPiece.HasGroundPieceOnOvest ()) {
			
			for (int i = lastReachedGroundPiece.indexInRow - 1; i > 0; i--) {

				GroundPiece pieceToCheck = lastReachedGroundPiece.row.groundPieces [i]; 

				if (Vector3.Distance (pieceToCheck.transform.position, transform.position) > maxDistance) {
					break;
				}

				if (pieceToCheck.collectingSnake != null) {
					if (pieceToCheck.collectingSnake == snake) {
						break;
					}

					if (pieceToCheck.collectingSnake != snake) {
						possibleEnemyTailGroundPieces.Add (pieceToCheck);
						possibleDirections.Add (-transform.right);
						break;
					}
				}
			}
		}

		if (lastReachedGroundPiece.HasGroundPieceOnSouth ()) {

			for (int i = lastReachedGroundPiece.indexInColumn + 1; i < lastReachedGroundPiece.column.groundPieces.Count; i++) {

				GroundPiece pieceToCheck = lastReachedGroundPiece.column.groundPieces [i]; 

				if (Vector3.Distance (pieceToCheck.transform.position, transform.position) > maxDistance) {
					break;
				}

				if (pieceToCheck.collectingSnake != null) {
					if (pieceToCheck.collectingSnake == snake) {
						break;
					}

					if (pieceToCheck.collectingSnake != snake) {
						possibleEnemyTailGroundPieces.Add (pieceToCheck);
						possibleDirections.Add (-transform.up);
						break;
					}
				}
			}
		}

		if (lastReachedGroundPiece.HasGroundPieceOnNorth ()) {
			for (int i = lastReachedGroundPiece.indexInColumn - 1; i > 0; i--) {

				GroundPiece pieceToCheck = lastReachedGroundPiece.column.groundPieces [i]; 

				if (Vector3.Distance (pieceToCheck.transform.position, transform.position) > maxDistance) {
					break;
				}

				if (pieceToCheck.collectingSnake != null) {
					if (pieceToCheck.collectingSnake == snake) {
						break;
					}

					if (pieceToCheck.collectingSnake != snake) {
						possibleEnemyTailGroundPieces.Add (pieceToCheck);
						possibleDirections.Add (transform.up);
						break;
					}
				}
			}
		}



		if (possibleDirections.Count > 0) {

			int nearestReachableGrounPieceIndex = 0;

			for (int i = 0; i > possibleEnemyTailGroundPieces.Count; i++) {
				if (Vector3.Distance (possibleEnemyTailGroundPieces [i].transform.position, transform.position) < Vector3.Distance (possibleEnemyTailGroundPieces [nearestReachableGrounPieceIndex].transform.position, transform.position)) {
					nearestReachableGrounPieceIndex = i;
				}
			}


			directionToReachEnemySnakeTail = possibleDirections [nearestReachableGrounPieceIndex];
			enemyTailGroundPieceToReach = possibleEnemyTailGroundPieces [nearestReachableGrounPieceIndex];


			return true;
		} else {
			return false;
		}


	}




}
