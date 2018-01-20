using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class FadeInOutTween : MonoBehaviour {
	public bool isText;
	// Use this for initialization
	void Start () {
		Fade ();
	}


	// Update is called once per frame
	void Update () {
		
	}

	void Fade(){

		if(isText)
			GetComponent<Text>().DOFade (0, 0.5f).SetUpdate (true).OnComplete(()=>{
				Show();
			});
		else
			GetComponent<Image> ().DOFade (0, 0.5f).SetUpdate (true).OnComplete(()=>{
			Show();
		});
	}

	void Show(){
		if(isText)
			GetComponent<Text>().DOFade (1, 0.5f).SetUpdate (true).OnComplete(()=>{
				Fade();
			});
		else
			GetComponent<Image> ().DOFade (1, 0.5f).SetUpdate (true).OnComplete(()=>{
			Fade();
		});
	}
}
