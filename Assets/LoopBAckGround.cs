using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBAckGround : MonoBehaviour {

    public float endpoint;
    public float speed;

	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);


        if (transform.localPosition.z >= endpoint)
            transform.localPosition = Vector3.zero;
	}
}
