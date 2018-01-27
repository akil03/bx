using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NormaliseScroll : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnEnable(){
		GetComponent <Scrollbar> ().value = 1f;
	}
	void OnDisable(){
		GetComponent <Scrollbar> ().value = 1f;
	}

}
