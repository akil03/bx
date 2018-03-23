using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       if(GetComponent<MeshRenderer>().isVisible)
        {

           // print("Found");
        }


    }


    void OnWillRenderObject()
    {
        print("test");
    }
}
