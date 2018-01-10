using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FBInit : MonoBehaviour {

	void Awake()
	{
		FB.Init(this.OnInitComplete);
	}

	void OnInitComplete()
	{
		
	}
}
