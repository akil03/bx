using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class FadingSplashScreen : MonoBehaviour {
     Color color1;
     Color color2;
    public SpriteRenderer screen1;
    public SpriteRenderer screen2;
    public SpriteRenderer logo;
    AsyncOperation async;

    public float fadeTime = 2;

    public float minWaitTimeBeforeChangeScene =5f;

    Color logoColor;
    // Use this for initialization
    void Start () {
        color1 = screen1.color;
        color2 = screen2.color;
        StartCoroutine(LoadLevelAsync()); ;

        screen1.color = color2;
        screen2.color = color1;

        logoColor = logo.color;
        logoColor.a = 0;
        logo.color = logoColor;

        StartCoroutine(FadingScreenEffect());
        StartCoroutine(SymbolFadeFromAlpha()); ;


        StartCoroutine(StartGame()); ;



    }

    // Update is called once per frame
    void Update () {
	
	}


    public IEnumerator LoadLevelAsync() {
            Debug.LogWarning("ASYNC LOAD STARTED - " +
               "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
            async = Application.LoadLevelAsync(1);
            async.allowSceneActivation = false;
            yield return async;
        
    }

    public IEnumerator StartGame() {
        yield return new WaitForSeconds(minWaitTimeBeforeChangeScene);
        async.allowSceneActivation = true;
    }



    public IEnumerator SymbolFadeFromAlpha() {

       float lerper = 0;
        while (lerper < 1) {
                        lerper += Time.deltaTime/ fadeTime;

            logoColor.a = Mathf.Lerp(0, 1, lerper);
            logo.color = logoColor;


            yield return new WaitForEndOfFrame();

        }





        yield return null;
    }


    public IEnumerator FadingScreenEffect() {


        float lerper = 0;
        while (lerper < 1) {

            lerper += Time.deltaTime/ fadeTime;

            screen1.color = Color.Lerp(color2, color1, lerper);
            screen2.color = Color.Lerp(color1, color2, lerper);

            yield return new WaitForEndOfFrame();






        }
        


        yield return null;
    }
}
