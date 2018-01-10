using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IGGCurrentScoreBar : MonoBehaviour {

	Image image;
	RectTransform rect;
	public float originalWidth;

	void Awake(){
		rect = GetComponent<RectTransform> ();
		image = GetComponent<Image> ();
		originalWidth = rect.localScale.x;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (ScoreHandler.instance.highScore > 0) {
			image.color = SnakesSpawner.instance.playerSnake.snakeMeshProprietes.snakeColor;

			float t = (float)ScoreHandler.instance.score / (float)ScoreHandler.instance.highScore;


			Vector3 newScale = rect.localScale;
			newScale.x = Mathf.Lerp (originalWidth, 4f, t); 

			rect.localScale = Vector3.MoveTowards (rect.localScale, newScale, 1 * Time.deltaTime);
		}
	}
}
