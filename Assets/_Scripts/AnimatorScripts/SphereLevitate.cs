using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SphereLevitate : MonoBehaviour {
    public float minY, maxY,time;
    public Ease easetype;

    Sequence seq;
    public bool isUI;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        if (isUI)
            Anim();
        else
            Levitate();
    }


    void Anim()
    {
        seq.Append(
        GetComponent<RectTransform>().DOLocalMoveY(maxY, time, false).SetEase(easetype).OnComplete(() =>
        {
            GetComponent<RectTransform>().DOLocalMoveY(minY, time, false).SetEase(easetype).OnComplete(() => {
                Anim();
            });
        }));

    }

    void Levitate()
    {
        seq.Append(
        transform.DOLocalMoveY(maxY, time, false).SetEase(easetype).OnComplete(() =>
        {
            transform.DOLocalMoveY(minY, time, false).SetEase(easetype).OnComplete(() =>
            {
                Levitate();
            });
        }));
    }

    public void StopAnim()
    {
        seq.Kill();
        
    }
}
