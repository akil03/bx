using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintComponent : MonoBehaviour {

	public int waitTime;
	public string text;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (waitTime);
		print (text);
	}

}
