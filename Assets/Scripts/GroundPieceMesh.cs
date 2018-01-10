using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPieceMesh : MonoBehaviour
{

	public MeshRenderer rend;
	Vector3 originalPos;

	void Awake ()
	{
		
		rend = GetComponent<MeshRenderer> ();
		originalPos = transform.localPosition;
		enabled = false;

	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void SetMaterialColor (Color color)
	{
		rend.material.color = color;
	}

	public Color GetMaterialColor(){
		return rend.material.color;
	}


	public void OnCollectedAnimation(){
		StopAllCoroutines ();
		StartCoroutine (DOOnCollectedAnimation ());
	}


	IEnumerator DOOnCollectedAnimation ()
	{
		transform.localPosition = originalPos;

		Vector3 targetPos = transform.localPosition;
		targetPos.z -= 2f;

		while (transform.localPosition != targetPos) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, targetPos, 5 * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		yield return null;
	}

	public void CollapseAnimation(){
		StopAllCoroutines ();

	}


	IEnumerator DOCollapseAnimation ()
	{
		while (transform.localPosition != originalPos) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, originalPos, 5 * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		yield return null;
	}

}
