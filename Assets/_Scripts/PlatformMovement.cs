using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformMovement : MonoBehaviour {
    public Transform twin;
    public float speed;
    public int direction = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.left * speed * direction * Time.deltaTime, Space.Self);
	}

    private void LateUpdate()
    {
        if (transform.position.y < -50)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, twin.transform.localPosition.z - 0.47f);
            
    }
}
