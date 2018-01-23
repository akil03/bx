using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedObject : MonoBehaviour {


	public RectTransform rect,targetRect;
	Vector3 originalScale;

	void Awake(){
		rect = GetComponent<RectTransform> ();
		originalScale = rect.localScale;
	}

	// Use this for initialization
	IEnumerator Start () {
		yield return 	StartCoroutine (MoveToTarget ());
		yield return StartCoroutine (EnlargeScale ());
		yield return StartCoroutine (ToOriginalScale ());
		Destroy (gameObject);
	}

	// Update is called once per frame
	void Update () {

	}


	public IEnumerator MoveToTarget(){

		Vector3 targetPos = GUIManager.instance.inGameGUI.specialPointsImage.rectTransform.position;

		//		Debug.Log ("TARGET POS " + targetPos);

		while (rect.position != targetPos) {
			rect.position = Vector3.MoveTowards (rect.position, targetPos, 1200f * Time.deltaTime);


			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}

	public IEnumerator EnlargeScale(){

		Vector3 targetScale = originalScale * 1.5f;

		while (rect.localScale != targetScale) {
			rect.localScale = Vector3.MoveTowards (rect.localScale, targetScale, 15f * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
	}


	public IEnumerator ToOriginalScale(){

		while (rect.localScale != originalScale) {
			rect.localScale = Vector3.MoveTowards (rect.localScale, originalScale, 8f * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
	}

	public void OnDisable(){
		Destroy (gameObject);
	}

}
