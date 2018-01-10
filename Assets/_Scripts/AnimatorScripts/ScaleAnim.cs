using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleAnim : MonoBehaviour {
	public Vector3 resetScale,maxScale;
	public float speed;
	public Ease easeType;
	// Use this for initialization
	void Start () {
		resetScale = transform.localScale;
		GoMax ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GoReset(){
		transform.DOScale (resetScale, speed).SetEase (easeType).OnComplete (() => {
			GoMax ();
		});
	}

	void GoMax(){
		transform.DOScale (maxScale, speed).SetEase (easeType).OnComplete (() => {
			GoReset ();
		});
	}
}
