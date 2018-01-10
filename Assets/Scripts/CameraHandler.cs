using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
	public GameObject objectToFollow;
	public float followSpeed = 10f;

	public static CameraHandler instance;
	void Awake ()
	{
		instance = this;
	}


	// Use this for initialization
	void Start ()
	{




	}

	public void SetDirection(int direction){

		transform.rotation = Quaternion.Euler (0, 0, direction);
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		if (objectToFollow == null) {

			if (SnakesSpawner.instance.playerSnake != null) {
				objectToFollow = SnakesSpawner.instance.playerSnake.gameObject;

				Vector3 newCameraPos = objectToFollow.transform.position;
				newCameraPos.z = transform.position.z;

				transform.position = newCameraPos;

			}
		}

		if (objectToFollow != null) {
			Vector3 newCameraPos = objectToFollow.transform.position;
			newCameraPos.z = transform.position.z;

			transform.position = newCameraPos;
		}
	}


}
