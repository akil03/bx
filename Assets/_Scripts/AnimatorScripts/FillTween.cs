using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class FillTween : MonoBehaviour {
	public float TimeToFill;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Fill(){
		transform.localScale = new Vector3 (0, 1, 1);
		transform.DOScaleX (1, TimeToFill);
	}
}
