using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{

	public SpriteRenderer sr;
	Vector3 originalScale;

	public float lifeTime = 5f;
	public float blinkTimes = 10f;
	public float blinkIntervalTime = 0.3f;

	void Awake ()
	{
		sr = GetComponentInChildren<SpriteRenderer> ();
		originalScale = transform.localScale;
	}

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (SpawnAnim ());
		StartCoroutine (AutoDestruction ());
	}
	
	// Update is called once per frame
	void Update ()
	{


		
	}

	public IEnumerator SpawnAnim(){
		transform.localScale = Vector3.zero;

		while (transform.localScale != originalScale) {

			transform.localScale = Vector3.MoveTowards (transform.localScale, originalScale, 3 * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

	}


	public IEnumerator AutoDestruction ()
	{
		yield return new WaitForSeconds (lifeTime);

		for (int i = 0; i < blinkTimes; i++) {
			sr.enabled = true;
			yield return new WaitForSeconds (blinkIntervalTime);
			sr.enabled = false;
			yield return new WaitForSeconds (blinkIntervalTime);
		}

		Destroy (gameObject);
	}

}
