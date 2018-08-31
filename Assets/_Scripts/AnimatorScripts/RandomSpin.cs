using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RandomSpin : MonoBehaviour {
	public Ease EaseType;
	public Vector3 targetRotation;
	public float spinTime;
    public bool isWorld;
	public float minSpin=-60,MaxSpin=60;

  
	// Use this for initialization
	void Start () {
		Spin ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Spin(){
		float rand = Random.Range (minSpin, MaxSpin);
		spinTime = Random.Range (0.5f, 1.5f);
        targetRotation = new Vector3 (0, rand, 0);
        if(isWorld)
            targetRotation = new Vector3(90,Random.Range(minSpin, MaxSpin),Random.Range(minSpin, MaxSpin));
		transform.DOLocalRotate (targetRotation, spinTime , RotateMode.Fast).SetEase (EaseType);
		Invoke ("Spin", spinTime + Random.Range (0.0f, 1.0f));
	}

    public void Stop()
    {

        CancelInvoke();
        Invoke("Normalize", 1.5f);
    }

    void Normalize()
    {
        transform.DOLocalRotate(Vector3.zero, spinTime, RotateMode.Fast).SetEase(EaseType);

    }
}
