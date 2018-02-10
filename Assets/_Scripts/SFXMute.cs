using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXMute : MonoBehaviour {

	void Awake(){
		
		if (PlayerPrefs.GetInt ("SFXMute") == 1)
			Destroy (GetComponent <AudioSource> ());

	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
