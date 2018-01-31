using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SnakeMeshContainer : MonoBehaviour {

	public GameObject snakeMesh;
	Snake snake;

	Vector3 originalPos;
	Vector3 originalScale;

	public bool isAnimator;

	public AvatarController AnimController;

	void Awake(){
		snake = GetComponentInParent<Snake> ();
		originalPos = transform.localPosition;
		originalScale = transform.localScale;
	}

	// Use this for initialization
	void Start () 
	{
		if (GetComponentInChildren <SnakeMeshProprietes> ().Mesh.GetComponent <AvatarController> ())
			AnimController = GetComponentInChildren  <SnakeMeshProprietes> (true).Mesh.GetComponent <AvatarController> ();
		
	}

	
	// Update is called once per frame
	void Update () {

		Vector3 newRot = Vector3.zero;;
		if (!snake)
			return;
		if (snake.currentMoveDirection == snake.transform.up) {
			newRot.z = 0;
			transform.DORotate (newRot, 0.7f, RotateMode.Fast);
//			if (AnimController)
//				AnimController.GoSpin ();


		}

		if (snake.currentMoveDirection == -snake.transform.up) {
			newRot.z = 180;
			transform.DORotate (newRot, 0.7f, RotateMode.Fast);
//			if (AnimController)
//				AnimController.GoSpin ();
		}

		if (snake.currentMoveDirection == snake.transform.right) {
			newRot.z = -90;
			transform.DORotate (newRot, 0.7f, RotateMode.Fast);
//			if (AnimController)
//				AnimController.GoSpin ();
		}

		if (snake.currentMoveDirection == -snake.transform.right) {
			newRot.z = 90;
			transform.DORotate (newRot, 0.7f, RotateMode.Fast);
//			if (AnimController)
//				AnimController.GoSpin ();
		}

		//transform.eulerAngles = newRot;

	}


	public void SetSnakeMesh(GameObject mesh){

		snakeMesh = mesh;

		if (transform.childCount > 0) {
			for (int i = 0; i < transform.childCount; i++) {
				DestroyImmediate (transform.GetChild (i).gameObject);
			}
		}

		GameObject newSnakeMesh = (GameObject) GameObject.Instantiate (mesh);
		newSnakeMesh.transform.SetParent (transform);
		newSnakeMesh.transform.localPosition = new Vector3 (0, 0, 0);
	}


	public IEnumerator DOSpawnAnimation(){
		yield return StartCoroutine (FallFromSky ());
		yield return StartCoroutine (JigglingAnimation ());
	}


	IEnumerator FallFromSky(){
		Vector3 newLocalPos = new Vector3 (originalPos.x, 4, originalPos.z);
		transform.localPosition = newLocalPos;

		while (transform.localPosition != originalPos) {

			transform.localPosition = Vector3.MoveTowards (transform.localPosition, originalPos, 10 * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator JigglingAnimation(){

		Vector3 targetScale = originalScale;
		targetScale.z = 0.18f;


		while (transform.localScale != targetScale) {

			transform.localScale = Vector3.MoveTowards (transform.localScale, targetScale, 10 * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}

		while (transform.localScale != originalScale) {

			transform.localScale = Vector3.MoveTowards (transform.localScale, originalScale, 10 * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}



	}




}
