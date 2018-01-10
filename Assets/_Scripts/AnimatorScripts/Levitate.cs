using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Levitate : MonoBehaviour {
	public float min,max;
	public float time;
	public Ease EaseType;
	// Use this for initialization
	void Start () {
		DoMax ();
	}

	// Update is called once per frame
	void Update () {

	}

	void DoMax(){
		transform.DOLocalMoveZ (max, time, false).SetEase (EaseType);
		Invoke ("DoMin",time);
	}

	void DoMin(){
		transform.DOLocalMoveZ (min, time, false).SetEase (EaseType);
		Invoke ("DoMax",time);
	}
}
