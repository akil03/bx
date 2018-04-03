using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnimation : MonoBehaviour {
	public GameObject carot;
	public GameObject[] LoadingBar;
    public Text statusBarTxt;
	// Use this for initialization
	void Start () {
		InvokeRepeating("CarotBlinker", 0, 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CarotBlinker()
	{
		carot.SetActive(!carot.activeSelf);

	}

	private void OnEnable()
	{
		StartCoroutine(LoadingBarFill());
	}

	private void OnDisable()
	{
		StopCoroutine(LoadingBarFill());
	}

	IEnumerator LoadingBarFill()
	{

        statusBarTxt.text = "CONNECTING TO SERVER..........";

        for (int i = 0; i < 10; i++)
		{
		   LoadingBar[i].SetActive(false);

		}

        //for (int i = 0; i < 10; i++)
        //{
        //	yield return new WaitForSeconds(1);
        //	LoadingBar[i].SetActive(true);

        //}
        yield return new WaitForSeconds(1.5f);
        int j = 0;
        for (j = 0; j < 4; j++)
        {
            yield return new WaitForSeconds(0.2f);
            LoadingBar[j].SetActive(true);

        }

        statusBarTxt.text = "SEARCHING FOR OPPONENTS..";

        yield return new WaitForSeconds(2f);

        for (j = 4; j < 7; j++)
        {
            yield return new WaitForSeconds(0.3f);
            LoadingBar[j].SetActive(true);

        }

        yield return new WaitForSeconds(2.5f);

        for (j = 7; j < 10; j++)
        {
            yield return new WaitForSeconds(0.3f);
            LoadingBar[j].SetActive(true);

        }
        
        //statusBarTxt.text = "ROOM CREATED..DEPLOYING WEAPONS..";
        statusBarTxt.text = "PLAYER FOUND.. INITIALIZING...";


    }


}
