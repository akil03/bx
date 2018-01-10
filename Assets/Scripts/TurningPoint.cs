using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoint : MonoBehaviour {

	public GroundPiece parentGroundPiece;

	void Awake(){
		enabled = false;
		parentGroundPiece = GetComponentInParent<GroundPiece> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
