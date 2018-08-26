using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPiece : MonoBehaviour
{

	public BoxCollider2D boxCollider;
	SpriteRenderer spriteRenderer;


	public Snake snakeOwener;
	public Snake collectingSnake;

	public int indexInGrid;

	public Column column;
	public int indexInColumn;

	public Row row;
	public int indexInRow;

	public TailPiece tailPiece;
	public PieceWhenCollected pieceWhenCollected;

	public Vector2[] groundPieceVertices;

	public int ownerIDForCheck;
	public bool tempHasToBeChecked;

	public bool tempReachableState;

	public GroundPiece[] groundPiecesOnEast;
	public GroundPiece[] groundPiecesOnOvest;
	public GroundPiece[] groundPiecesOnSouth;
	public GroundPiece[] groundPiecesOnNorth;


	public Vector3 tempRaycastedDirection;

	public Sprite spriteWhenIsBoundOfGround;

	public GroundPiece[] groundPiecesAroundMe;

	//public int groundRotationAngle;

	void Awake ()
	{
		
		boxCollider = GetComponent<BoxCollider2D> ();

		enabled = false;
	}

	public void Initialize ()
	{

		spriteRenderer = GetComponent<SpriteRenderer> ();
		tailPiece = GetComponentInChildren<TailPiece> ();
		pieceWhenCollected = GetComponentInChildren<PieceWhenCollected> ();
		pieceWhenCollected.gameObject.SetActive (false);
		tailPiece.gameObject.SetActive (false);


		groundPiecesOnEast = GetGroundPiecesOnEast ();
		groundPiecesOnOvest = GetGroundPiecesOnOvest ();
		groundPiecesOnSouth = GetGroundPiecesOnSouth ();
		groundPiecesOnNorth = GetGroundPiecesOnNorth ();


		if (IsBoundPiece ()) {

			spriteRenderer.sprite = spriteWhenIsBoundOfGround;
			//spriteRenderer.enabled = false;
		}

		Destroy (boxCollider);

		try {
			groundPiecesAroundMe = GetGroundPiecesAroundMe (5);
		} catch {

		}

	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public Bounds bounds ()
	{
		return boxCollider.bounds;
	}

	public void SetSnakeOwner (Snake snake)
	{	
		if (snake == snakeOwener)
			return;

		if (snakeOwener != null && snakeOwener != snake) {
			snakeOwener.ownedGroundPieces.Remove (this); // REMOVING FROM OLD SNAKE 
		}

		if (snakeOwener != snake) {
			snakeOwener = snake; // CREATING NEW SNAKE REFERENCE
			snake.ownedGroundPieces.Add (this);


			if (collectingSnake != null) {
				if (collectingSnake == snake) {
					collectingSnake = null;
				}
			}

			tailPiece.gameObject.SetActive (false);
		}
	}

	public void ShowCollectedPiece(Sprite sprite){

		if (GroundPieceOnSouth ().snakeOwener == null) {
			pieceWhenCollected.Show (sprite, true);
		} else {
			pieceWhenCollected.Show (sprite, false);
		}
	}


	public void RemoveSnakeOwner (Snake snake)
	{
		snakeOwener = null;
		GroundPieceOnNorth ().pieceWhenCollected.bottomBorder.SetActive (true);
		pieceWhenCollected.Hide ();
	}

	public void RemoveCollectingSnake (Snake snake)
	{	
		if(collectingSnake==snake)
			collectingSnake = null;
		tailPiece.gameObject.SetActive (false);
	}

	public void SetCollectingSnake (Snake snake)
	{			

		if (collectingSnake != snake && collectingSnake != null) {
			//StartCoroutine (collectingSnake.Die ());
			collectingSnake.tailGroundPieces.Remove (this); // REMOVING FROM OLD SNAKE
		}

		collectingSnake = snake;	
		snake.tailGroundPieces.Add (this);
		tailPiece.gameObject.SetActive (true);

		if(snake.currentMoveDirection.x==1&&snake.currentMoveDirection.y==0)
			tailPiece.transform.rotation = Quaternion.Euler (0, 0, 270);
		else if(snake.currentMoveDirection.x==-1&&snake.currentMoveDirection.y==0)
			tailPiece.transform.rotation = Quaternion.Euler (0, 0, 90);
		if(snake.currentMoveDirection.x==0&&snake.currentMoveDirection.y==1)
			tailPiece.transform.rotation = Quaternion.Euler (0, 0, 0);
		else if(snake.currentMoveDirection.x==0&&snake.currentMoveDirection.y==-1)
			tailPiece.transform.rotation = Quaternion.Euler (0, 0, 180);

		tailPiece.originalColor = snake.spriteColor;
		tailPiece.SetSPrite (snake.tailPieceSprite);
	}

	public void SetCollectingNetworkSnake (Snake snake)
	{			


		if (collectingSnake != snake && collectingSnake != null) {
			//StartCoroutine (collectingSnake.Die ());
			collectingSnake.tailGroundPieces.Remove (this); // REMOVING FROM OLD SNAKE
		}

		if (collectingSnake != snake) {
			collectingSnake = snake;	
			//snake.tailGroundPieces.Add (this);
			tailPiece.gameObject.SetActive (true);

			if (snake.currentMoveDirection.x == 1 && snake.currentMoveDirection.y == 0)
				tailPiece.transform.rotation = Quaternion.Euler (0, 0, 270);
			else if (snake.currentMoveDirection.x == -1 && snake.currentMoveDirection.y == 0)
				tailPiece.transform.rotation = Quaternion.Euler (0, 0, 90);
			if (snake.currentMoveDirection.x == 0 && snake.currentMoveDirection.y == 1)
				tailPiece.transform.rotation = Quaternion.Euler (0, 0, 0);
			else if (snake.currentMoveDirection.x == 0 && snake.currentMoveDirection.y == -1)
				tailPiece.transform.rotation = Quaternion.Euler (0, 0, 180);

			tailPiece.originalColor = snake.spriteColor;
			tailPiece.SetSPrite (snake.tailPieceSprite);
		}
	}


	public void SetNetworkSnakeOwner (Snake snake)
	{	
		if (snakeOwener != null && snakeOwener != snake) {
			snakeOwener.ownedGroundPieces.Remove (this); // REMOVING FROM OLD SNAKE 
		}

		if (snakeOwener != snake) {
			snakeOwener = snake; // CREATING NEW SNAKE REFERENCE
		


			if (collectingSnake != null) {
				if (collectingSnake == snake) {
					collectingSnake = null;
				}
			}

			pieceWhenCollected.sr.color = snake.spriteColor;
			ShowCollectedPiece (snake.collectedPieceSprite);

			tailPiece.gameObject.SetActive (false);
		}
	}

	public GroundPiece GroundPieceOnEst ()
	{
		return	row.groundPieces [indexInRow + 1];
	}


	public GroundPiece GroundPieceOnOvest ()
	{
		if (indexInRow > 0) {
			return	row.groundPieces [indexInRow - 1];
		} else {
			return null;
		}

	}

	public GroundPiece GroundPieceOnNorth ()
	{
		return	column.groundPieces [indexInColumn - 1];
	}

	public GroundPiece GroundPieceOnSouth ()
	{
		return	column.groundPieces [indexInColumn + 1];
	}



	public GroundPiece[] GetGroundPiecesOnEast ()
	{

		List<GroundPiece> groundPieces = new List<GroundPiece> ();

		for (int i = indexInRow + 1; i < row.groundPieces.Count; i++) {

			groundPieces.Add (row.groundPieces [i]);

		}

		return groundPieces.ToArray ();
	}

	public GroundPiece[] GetGroundPiecesOnOvest ()
	{

		List<GroundPiece> groundPieces = new List<GroundPiece> ();

		for (int i = indexInRow; i > 0; i--) {

			if (i != 0) {
				groundPieces.Add (row.groundPieces [i - 1]);
			}
		}

		return groundPieces.ToArray ();
	}

	public GroundPiece[] GetGroundPiecesOnSouth ()
	{

		List<GroundPiece> groundPieces = new List<GroundPiece> ();

		for (int i = indexInColumn + 1; i < column.groundPieces.Count; i++) {

			groundPieces.Add (column.groundPieces [i]);

		}

		return groundPieces.ToArray ();
	}

	public GroundPiece[] GetGroundPiecesOnNorth ()
	{

		List<GroundPiece> groundPieces = new List<GroundPiece> ();

		for (int i = indexInColumn; i > 0; i--) {

			if (i != 0) {
				groundPieces.Add (column.groundPieces [i - 1]);
			}
		}

		return groundPieces.ToArray ();
	}

	public bool HasGroundPieceOnOvest ()
	{

		if (indexInRow > 0) {

			return true;
		} else {
			return  false;
		}

	}


	public	bool HasGroundPieceOnEast ()
	{

		if (indexInRow < row.groundPieces.Count - 1) {
			return true;
		} else {
			return  false;
		}

	}

	public	bool HasGroundPieceOnSouth ()
	{

		if (indexInColumn < column.groundPieces.Count - 1) {
			return true;
		} else {
			return  false;
		}

	}

	public bool HasGroundPieceOnNorth ()
	{

		if (indexInColumn > 0) {

			return true;
		} else {
			return  false;
		}

	}

	public bool IsBoundPiece ()
	{

		if (HasGroundPieceOnSouth () && HasGroundPieceOnEast () && HasGroundPieceOnNorth () && HasGroundPieceOnOvest ()) {
			return false;
		} else {

			return true;
		}
	}


	public GroundPiece[] GetGroundPiecesAroundMe (int size)
	{

		List<GroundPiece> groundPieces = new List<GroundPiece> ();
		GroundPiece topLeftGroundPiece = GroundSpawner.instance.GetGroundPieceAt (indexInRow - size, indexInColumn - size);

		for (int y = topLeftGroundPiece.indexInColumn; y < topLeftGroundPiece.indexInColumn + size * 2; y++) {
			for (int x = topLeftGroundPiece.indexInRow; x < topLeftGroundPiece.indexInRow + size * 2; x++) {
				groundPieces.Add (GroundSpawner.instance.GetGroundPieceAt (x, y));
			}

		}


		return groundPieces.ToArray ();

	}



}
