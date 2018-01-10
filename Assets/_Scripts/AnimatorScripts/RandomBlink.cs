using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBlink : MonoBehaviour {
	public Sprite[] Glow;
	int glowNo;
	// Use this for initialization
	void Start () {
		//Lit ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Lit(){
		float Rand = Random.Range (5, 20);
		if (glowNo == 0)
			glowNo = 1;
		else
			glowNo = 0;
		GetComponent <SpriteRenderer> ().sprite = Glow [glowNo];

		if(glowNo==0)
			Invoke ("Lit", 0.25f);
		else
			Invoke ("Lit", Rand);
	}
}
