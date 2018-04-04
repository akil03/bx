using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CopiedAnim : MonoBehaviour {
    string copyTxt;
    public Text AnimTxt;
	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CopyAnim()
    {
        copyTxt = GetComponentInChildren<Text>().text;
        AnimTxt.text = "Copied " + "'" + copyTxt + "'" + " to clipboard !!";
        AnimTxt.DOFade(1, 0);
        AnimTxt.DOFade(0, 1.2f);
        AnimTxt.transform.position = transform.position;
        AnimTxt.transform.DOLocalMoveY(-434f, 0.5f,false);
    }
}
