using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangeTexture : MonoBehaviour 
{
	[SerializeField]GameObject button1, button2;

	public void ShowType1()
	{
		Op (button1,button2);
        ChallengeActor.challengeState = "open";
	}

	public void ShowType2()
	{
		Op (button2,button1);
	}

	void Op(GameObject but1,GameObject but2)
	{
		but1.SetActive (true);
		if (but2.activeSelf)
			but2.SetActive (false);
	}
}