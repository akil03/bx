using System.Collections;
using DG.Tweening;
using DoozyUI;
using GameSparks.Api.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{


    public static GUIManager instance;

    public TutorialGUI tutorialGUI;
    public PauseGUI pauseGUI;
    public ShopGUI shopGUI;
    public GameOverGUI gameOverGUI;
    public MainMenuGUI mainMenuGUI;
    public OneMoreChanceGUI oneMoreChanceGUI;
    public InGameGUI inGameGUI;

    public Color[] bgColours;
    public SpriteRenderer Bg, Glow;
    public Material BGMat;

    public UIElement[] Pages;
    public Transform FillBar;
    public UIElement InternetCheckPage, SettingsPage, matchLoading, PlayerStats;
    public GameObject BG;

    public Text mmrTxt, playerID_TXT;
    public InputField playerNameTxt;

    public Sprite goldImg, gemImg, xpImg;
    public GameObject collectingObject;
    public Text Gold, Gems, XP;
    public int currentPage;

    public Color Green, Red;
    public Image MusicImg, SfxImg;
    public AudioSource MusicSource, SFXSource;


    public GameObject GoogleCanvas, GamecenterCanvas;

    void Awake()
    {
        instance = this;
        if (PlayerPrefs.GetInt("MusicMute") == 1)
            MusicBtn();

        if (PlayerPrefs.GetInt("SFXMute") == 1)
            SFXBtn();
    }

    // Use this for initialization
    void Start()
    {
        mmrTxt.text = PlayerPrefs.GetInt("MMR").ToString();


        if (Application.platform == RuntimePlatform.Android)
        {

            GamecenterCanvas.SetActive(false);
            GoogleCanvas.SetActive(true);
        }

        else
        {
            GamecenterCanvas.SetActive(true);
            GoogleCanvas.SetActive(false);
        }
        playerNameTxt.onEndEdit.AddListener(OnPlayerNameChanged);
    }

    private void OnPlayerNameChanged(string newName)
    {
        AccountDetails.instance.accountDetails.displayName = newName;
        new ChangeUserDetailsRequest().SetDisplayName(newName).Send(null);
    }

    // Update is called once per frame
    void Update()
    {




    }

    public void MusicBtn()
    {
        if (MusicImg.color == Red)
        {
            MusicSource.volume = 1;
            MusicImg.color = Green;
            MusicImg.transform.parent.GetComponentInChildren<Text>().text = "ON";
            PlayerPrefs.SetInt("MusicMute", 0);
        }
        else
        {
            MusicSource.volume = 0;
            MusicImg.color = Red;
            MusicImg.transform.parent.GetComponentInChildren<Text>().text = "OFF";
            PlayerPrefs.SetInt("MusicMute", 1);
        }

    }

    public void SFXBtn()
    {
        if (SfxImg.color == Red)
        {
            //MusicSource.volume = 1;
            SfxImg.color = Green;
            SfxImg.transform.parent.GetComponentInChildren<Text>().text = "ON";
            PlayerPrefs.SetInt("SFXMute", 0);
        }
        else
        {
            //MusicSource.volume = 0;
            SfxImg.color = Red;
            SfxImg.transform.parent.GetComponentInChildren<Text>().text = "OFF";
            PlayerPrefs.SetInt("SFXMute", 1);
        }
    }


    //	public void StopFindingGame()
    //	{
    //		PhotonManager.instance.StopFinding ();
    //	}


    public void ShowInGameGUI()
    {
        HideAllPages();
        if (!ObliusGameManager.isFriendlyBattle)
            Regeneration.instance.UseLife();
        inGameGUI.gameObject.SetActive(true);
        inGameGUI.GetComponent<UIElement>().Show(false);
        int Rand = UnityEngine.Random.Range(0, bgColours.Length);
        Bg.color = bgColours[Rand];
        Glow.color = bgColours[Rand];
        BGMat.color = bgColours[Rand];
        BG.SetActive(false);
    }

    public void HideInGameGUI()
    {
        //HideAllPages ();
        inGameGUI.gameObject.SetActive(false);
        BG.SetActive(true);
    }


    public void ShowGameOverGUI()
    {
        //	gameOverGUI.OnWin ();
        if (!PlayerPrefs.HasKey("TutorialComplete"))
        {
            PlayerPrefs.SetInt("TutorialComplete", 1);
            ShowTutorialLog("Congratulations !! You've successfully completed the tutorial !!");
            inGameGUI.GetComponent<UIElement>().Hide(false);
            Invoke("ReloadScene", 3);
            GameSparkRequests req = new GameSparkRequests();
            req.Request("AddGold", "amt", "50", UpdateAccountDetails);


            //	Invoke ("FinishTut", 3);
            return;
        }


        inGameGUI.GetComponent<UIElement>().Hide(false);
        gameOverGUI.gameObject.SetActive(true);
        gameOverGUI.GetComponent<UIElement>().Show(false);
    }

    public void UpdateAccountDetails(string response)
    {
        //print (response);
        AccountDetails.instance.Get();
    }


    public void WatchAd()
    {


    }

    void FinishTut()
    {
        ObliusGameManager.instance.CloseTutorial();
    }

    public void HideGameOverGUI()
    {
        matchLoading.Hide(false);
        gameOverGUI.GetComponent<UIElement>().Hide(false);
        if (PhotonNetwork.inRoom)
            PhotonNetwork.LeaveRoom();
        //gameOverGUI.gameObject.SetActive(false);
    }


    public void ShowTutorialGUI()
    {
        tutorialGUI.Activate();
    }

    public void HideTutorialGUI()
    {
        tutorialGUI.Deactivate();
    }

    public void ShowPauseGUI()
    {
        pauseGUI.Activate();
    }

    public void ShowOneMoreChanceGUI()
    {
        oneMoreChanceGUI.gameObject.SetActive(true);
    }

    public void HideOneMoreChanceGUI()
    {
        oneMoreChanceGUI.gameObject.SetActive(false);
    }

    public void ShowShopGUI()
    {
        ShopHandler.instance.Activate();
    }

    public void ShowMainMenuGUI()
    {
        ObliusGameManager.instance.ChangePlayerStaus(true);
        HideAllPages();
        ObliusGameManager.instance.gameState = ObliusGameManager.GameState.menu;
        // mainMenuGUI.gameObject.SetActive(true);
        //mainMenuGUI.GetComponent <UIElement> ().Show (false);
        OpenPage(3);

        SnakesSpawner.instance.previewMeshContainer.transform.parent.gameObject.SetActive(true);
        BG.SetActive(true);
    }



    public void OpenPage(int pageNo)
    {
        //		if (currentPage == pageNo)
        //			return;


        //		HideAllPages ();
        //
        //		Pages [pageNo - 1].gameObject.SetActive (true);
        //		Pages [pageNo-1].Show (false);
        ObliusGameManager.isFriendlyBattle = false;
        ObliusGameManager.isOnlineBattle = false;
        currentPage = pageNo;

        if (pageNo < 6)
        {
            Pages[0].transform.parent.gameObject.SetActive(true);
            Pages[0].transform.parent.GetComponent<RectTransform>().DOLocalMoveX((-3 + pageNo) * -1500, 0.3f, false);
            Pages[pageNo - 1].gameObject.SetActive(true);
            Pages[pageNo - 1].Show(false);
        }
        else
        {
            Pages[0].transform.parent.gameObject.SetActive(false);
            Pages[5].gameObject.SetActive(true);
            Pages[5].Show(false);
        }
    }

    public void HideAllPages()
    {

        Pages[0].transform.parent.gameObject.SetActive(false);
        Pages[5].Hide(false);

        return;
        foreach (UIElement page in Pages)
            page.Hide(false);
    }

    public void ReloadScene()
    {
        InternetCheckPage.Hide(false);
        //		if(PhotonManagerAdvanced.instance.IsInGame())
        PhotonNetwork.LeaveRoom();
        Application.LoadLevel(0);
        //	SceneManager.LoadScene (0);
    }

    public void OpenConnectionPopup()
    {
        if (PhotonNetwork.connected)
        {
            InternetCheckPage.gameObject.SetActive(true);
            InternetCheckPage.Show(false);
        }
    }


    public void ShowSettings()
    {
        SettingsPage.gameObject.SetActive(true);
        SettingsPage.Show(false);
    }



    public void HideSettings()
    {
        SettingsPage.Hide(false);
    }
    Sprite NotificationSprite;
    bool isLogShown;
    public void ShowLog(string log)
    {
        if (isLogShown)
            return;

        UIManager.ShowNotification("Example_1_Notification_4", 1f, true, log, NotificationSprite);
        isLogShown = true;

        Invoke("EnableLog", 1);
    }

    public void ShowLog(string log, float time)
    {
        if (isLogShown)
            return;

        UIManager.ShowNotification("Example_1_Notification_4", time, true, log, NotificationSprite);
        isLogShown = true;

        Invoke("EnableLog", time);
    }

    public Text NotificationTxt;
    public UnityEvent rematchResponse;
    public UnityEvent leftTheMatch;
    public Text meshClass;
    public Text meshName;
    public Text mostAreaCoveredTxt;
    public Text highestTrophiesTxt;
    public Text totalKillsTxt;
    public Text totalDeathsTxt;
    public Text totalWinsTxt;
    public Text totalLossTxt;

    public void ShowTutorialLog(string log)
    {
        NotificationTxt.transform.parent.parent.gameObject.SetActive(true);
        NotificationTxt.text = log;

    }
    public void ShowTutorialLog(string log, float ticks)
    {
        NotificationTxt.transform.parent.parent.gameObject.SetActive(true);
        NotificationTxt.text = log;
        StartCoroutine(HideTutorialLogRoutine(0, ticks));
    }

    public void ShowTutorialLog(float delay, string log, float ticks)
    {
        NotificationTxt.transform.parent.parent.gameObject.SetActive(true);
        NotificationTxt.text = log;
        StartCoroutine(HideTutorialLogRoutine(delay, ticks));
    }

    IEnumerator HideTutorialLogRoutine(float delay, float ticks)
    {

        isLogShown = true;
        StartCoroutine(InstantHideLog(delay, ticks));
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(ticks);
        Time.timeScale = 1;
        HideTutorialLog();
        StopCoroutine(InstantHideLog(delay, ticks));
    }

    IEnumerator InstantHideLog(float delay, float ticks)
    {
        int i = 0;
        while (i == 0)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                i = 1;
                Time.timeScale = 1;
                HideTutorialLog();
                StopCoroutine(HideTutorialLogRoutine(delay, ticks));
            }
        }

    }

    public void HideTutorialLog()
    {
        NotificationTxt.transform.parent.parent.gameObject.SetActive(false);
    }


    void EnableLog()
    {
        isLogShown = false;
    }



    public void CopyUserID()
    {
        UniClipboard.SetText(playerID_TXT.text);
        UIManager.ShowNotification("Example_1_Notification_4", 1f, true, "Copied to clipboard !", NotificationSprite);
    }

    public void AddCoins(int amt)
    {

        StartCoroutine(AddCollectingObject(amt, Gold, goldImg));

    }



    IEnumerator AddCollectingObject(int count, Text txtObj, Sprite img)
    {
        while (!txtObj.transform.parent.gameObject.activeInHierarchy)
            yield return null;

        int temp = int.Parse(txtObj.text);
        for (int i = 0; i < 20; i++)
        {
            GameObject GO = Instantiate(collectingObject, txtObj.transform.parent);
            GO.transform.position = new Vector3(-80, -60, 20);
            GO.GetComponent<Image>().sprite = img;
            GO.transform.DOMove(txtObj.transform.position, 0.5f, false).OnComplete(() =>
            {
                if (i < 19)
                    txtObj.text = (int.Parse(txtObj.text) + 1).ToString();
                else
                    txtObj.text = (temp + count).ToString();
                txtObj.transform.parent.GetChild(0).DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.1f).OnComplete(() =>
                {
                    txtObj.transform.parent.GetChild(0).DOScale(Vector3.one, 0.1f);
                });
                Destroy(GO);
            });
            yield return new WaitForSeconds(0.5f / 20.0f);

            //			txtObj.text = (int.Parse (txtObj.text) + count/20).ToString ();
            //			txtObj.transform.parent.DOScale (new Vector3 (1.15f, 1.15f, 1.15f), 0.1f).OnComplete (() => {
            //				txtObj.transform.parent.DOScale (Vector3.one, 0.1f);
            //			});
            //
            //			yield return new WaitForSeconds (0.05f );
        }

    }

    public void AddGems()
    {

    }

    public void AddXP()
    {

    }
}
