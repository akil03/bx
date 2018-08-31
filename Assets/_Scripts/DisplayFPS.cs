using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFPS : MonoBehaviour {

    public Text fpsDisplayText;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update () 
    {
        fpsDisplayText.text = (int.Parse((1.0f / Time.smoothDeltaTime).ToString())).ToString();
	}
}
