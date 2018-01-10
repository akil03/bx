using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IGGLeaderboardBar : MonoBehaviour {


	public Text text; 
	public Image scoreBar;

	RectTransform rect;
	public float originalPos;
	public int snakeIndex = 0;


	void Awake(){
		rect = GetComponent<RectTransform> ();
		originalPos = rect.anchoredPosition.x;
	}


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (IGGLeaderboardManager.instance.snakes.Count <= snakeIndex) {
			return;
		}

		UpdateScoreBarColor ();
		UpdateBarSize ();
		UpdateText ();
	}

	public void UpdateScoreBarColor(){

			scoreBar.color = IGGLeaderboardManager.instance.snakes [snakeIndex].snakeMeshProprietes.snakeColor;
		
	}

	void UpdateText(){


		string snakeScore =  ObliusGameManager.instance.TrimPercentage (IGGLeaderboardManager.instance.ScoreToPercentage (IGGLeaderboardManager.instance.snakes [snakeIndex].ownedGroundPieces.Count).ToString());
		text.text = (snakeIndex+1) + " - " + snakeScore  + "%" + " " + IGGLeaderboardManager.instance.snakes [snakeIndex].name;
	}

	void UpdateBarSize(){
		
		float t = (float)IGGLeaderboardManager.instance.snakes[snakeIndex].ownedGroundPieces.Count/ (float)IGGLeaderboardManager.instance.snakes[0].ownedGroundPieces.Count;

		Vector3 newPos = rect.anchoredPosition;
		newPos.x = Mathf.Lerp (originalPos, 540f, t); 

		rect.anchoredPosition = Vector3.MoveTowards(rect.anchoredPosition,newPos,50f*Time.deltaTime);


	}

}
