using UnityEngine;
using System.Collections;

#if UNITY_MOBILE
using UnityEngine.Advertisements;
#endif

public class OneMoreChanceGUI : MonoBehaviour {



    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Activate()
    {
        gameObject.SetActive(true);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void OnOneMoreChanceButtonClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();
		#if UNITY_MOBILE

        UnityRewardAds.instance.ShowRewardedAd(HandleShowResult);
		#endif

        Deactivate();
    }

    public void OnGameOverButtonClick()
    {
        SoundsManager.instance.PlayMenuButtonSound();

        Deactivate();
        ObliusGameManager.instance.oneMoreChanceUsed = true;
        ObliusGameManager.instance.GameOver(0);
    }

		#if UNITY_MOBILE

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:

                ObliusGameManager.instance.gameState = ObliusGameManager.GameState.game;
                ObliusGameManager.instance.oneMoreChanceUsed = true;
                ObliusGameManager.instance.ResetGame(false,false);
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
          
                ObliusGameManager.instance.gameState = ObliusGameManager.GameState.game;
                ObliusGameManager.instance.oneMoreChanceUsed = true;
                ObliusGameManager.instance.ResetGame(false,false);
                break;
            case ShowResult.Failed:

                ObliusGameManager.instance.gameState = ObliusGameManager.GameState.game;
                ObliusGameManager.instance.oneMoreChanceUsed = true;
                ObliusGameManager.instance.ResetGame(false,false);
                break;
        }
    }
		#endif

}
