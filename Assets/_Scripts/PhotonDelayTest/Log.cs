using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [PunRPC]
    public void ShowLog(int a,string log)
    {
        print(log+"  "+(Time.time - PhotonDelayTest.startTime).ToString());
    }
}
