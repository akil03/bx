using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class SplashLoading : MonoBehaviour {
    public Text LoadingTxt;
    public Image LoadingBar;
    int loadvalue;
    public bool isSplashPage;
	// Use this for initialization
	void Start () {

        if (isSplashPage)
            StartCoroutine(LoadMainMenu());
    }
	
	// Update is called once per frame
	void Update () {
        loadvalue = (int)(LoadingBar.rectTransform.localScale.x * 100);
        LoadingTxt.text = "LOADING... " + loadvalue.ToString() + "%";
        if (loadvalue == 100)
        {
            ObliusGameManager.instance.StartTutorial();
            gameObject.SetActive(false);
        }
    }



    public void Load(float percent)
    {
        LoadingBar.rectTransform.DOScaleX(percent, 0.3f);
    }

    IEnumerator LoadMainMenu()
    {
 

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);

        
        while (!asyncLoad.isDone)
        {
            LoadingBar.rectTransform.localScale =  new Vector3(asyncLoad.progress / 2, 1, 1);
            yield return null;
        }
    }
}
