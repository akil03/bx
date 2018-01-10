using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceWhenCollected : MonoBehaviour {

	public SpriteRenderer sr;
	public GameObject bottomBorder;
	public Color spriteColor;

	void Awake(){
		sr = GetComponent<SpriteRenderer> (); 
		enabled = false;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Show(Sprite sprite,bool showBorder){


		StopAllCoroutines ();

		if(showBorder){
			//bottomBorder.SetActive (true);
		}
		else{
			bottomBorder.SetActive(false);}

		gameObject.SetActive (true);

		Color transparent = sr.color;
		transparent.a = 0;
		sr.color = transparent;

		sr.sprite = sprite;
		StartCoroutine (DOShow ());
	

	}

	public void Hide(){
		bottomBorder.SetActive (false);
		gameObject.SetActive (false);
	}


	public IEnumerator DOShow(){


		while (sr.color.a != 1) {

			float newAlpha = Mathf.MoveTowards (sr.color.a, 1, 10 * Time.deltaTime);
			Color newColor = sr.color;
			newColor.a = newAlpha;

			sr.color = newColor;

			yield return new WaitForEndOfFrame ();
		}
	}

	public IEnumerator FadeOut(){

		Color newColor = sr.color;
		bottomBorder.SetActive (false);

		while (sr.color.a != 0) {
			newColor.a = Mathf.MoveTowards (newColor.a, 0, 5f * Time.deltaTime);
			sr.color = newColor;
			yield return new WaitForEndOfFrame ();
		}

	}

	public void FadeImmediate(){
		Color newColor = sr.color;
		bottomBorder.SetActive (false);
		newColor.a = 0;
		sr.color = newColor;

	}


}
