using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {


	void Start(){
		transform.SetParent (null);
		Invoke ("test", 5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void test(){

		transform.parent = null;

	}


}

