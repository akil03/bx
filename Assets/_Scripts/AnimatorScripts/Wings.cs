using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wings : MonoBehaviour {
	public Vector3 min,max,to;
	public float time,speed;
	public Ease EaseType;

	// Use this for initialization
	void Start () {
		//DoMax ();
	
	}

	void OnEnable(){
		StartCoroutine (StartRotation ());
	}

	IEnumerator StartRotation()
	{
		var pointA = transform.position;
		while (true) {
			yield return StartCoroutine(DoRotation(transform, min, max, time));
			yield return StartCoroutine(DoRotation(transform, max, min, time));
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void DoMax(){
		transform.DOLocalRotate (max, time, RotateMode.WorldAxisAdd).SetEase (EaseType);
		Invoke ("DoMin",time);
	}

	void DoMin(){
		transform.DOLocalRotate (min, time, RotateMode.WorldAxisAdd).SetEase (EaseType);
		Invoke ("DoMax",time);
	}

	IEnumerator DoRotation(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		var i= 0.0f;
		var rate= 1.0f/time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.localRotation = Quaternion.Lerp(Quaternion.Euler (startPos.x,startPos.y,startPos.z), Quaternion.Euler (endPos.x,endPos.y,endPos.z), i);
			yield return null; 
		}
	}
}
