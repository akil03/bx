using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	public bool start;
	 

	// Use this for initialization
	IEnumerator Start () {
		if (start) {
			yield return (PhotonManagerAdvanced.instance._CreateRoom());
			print ("iiii");
		} else
			yield return null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}

