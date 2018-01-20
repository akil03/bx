using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameSparksUnity))]
public class GSInit : MonoBehaviour {
	public static GSInit instance;

	void Start()
	{
		if (instance == null)
			instance = this;
		if (instance != this)
			Destroy (gameObject);
		else
			DontDestroyOnLoad (gameObject);
	}

}
