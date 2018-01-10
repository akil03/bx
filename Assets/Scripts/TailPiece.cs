using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailPiece : MonoBehaviour {

	SpriteRenderer spriteRenderer;
	public Color originalColor;


	void Awake(){
		
		spriteRenderer = GetComponent<SpriteRenderer> ();
		originalColor = spriteRenderer.color;

		enabled = false;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void SetSPrite(Sprite sprite){

		StopAllCoroutines ();	

		spriteRenderer.color = originalColor;
		spriteRenderer.sprite = sprite;

		//GetComponent <AudioSource> ().Play ();
	}



	public IEnumerator FadeOut(){

		Color newColor = spriteRenderer.color;

		while (spriteRenderer.color.a != 0) {			
			newColor.a = Mathf.MoveTowards (newColor.a, 0, 40f* Time.deltaTime);
			spriteRenderer.color = newColor;
			yield return new WaitForEndOfFrame ();
		}
	}

	public void FadeImmediate(){
		Color newColor = spriteRenderer.color;
		newColor.a = 0;
		spriteRenderer.color = newColor;

	}

	public void Hide(){
		gameObject.SetActive (false);
	}

}
