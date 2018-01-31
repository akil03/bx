using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMeshProprietes : MonoBehaviour {

	public Sprite tailPiece;
	public Sprite collectedPiece;

	public Sprite[] patterns;

	public Color snakeColor;
	public GameObject Mesh, Ball, Shield;

	public bool isAnimator;
	void Awake(){
		//collectedPiece = patterns [Random.Range (0, patterns.Length)];
		if(Shield)
			Shield.SetActive (false);

	

		enabled = false;
	}

	public void RandomizePattern(){
//		collectedPiece = patterns [Random.Range (0, patterns.Length)];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
