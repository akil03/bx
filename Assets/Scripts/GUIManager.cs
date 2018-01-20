using UnityEngine;
using System.Collections;
using DoozyUI;
using UnityEngine.SceneManagement;
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
	public UIElement InternetCheckPage,SettingsPage;
	public GameObject BG;

	public Text mmrTxt,playerID_TXT;
	public InputField playerNameTxt;
	public int currentPage;
    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
		mmrTxt.text = PlayerPrefs.GetInt ("MMR").ToString ();
    }

    // Update is called once per frame
    void Update()
    {

        


    }

//	public void StopFindingGame()
//	{
//		PhotonManager.instance.StopFinding ();
//	}


	public void ShowInGameGUI()
	{
		HideAllPages ();
		inGameGUI.gameObject.SetActive(true);
		inGameGUI.GetComponent <UIElement> ().Show (false);
		int Rand = Random.Range (0, bgColours.Length);
		Bg.color = bgColours [Rand];
		Glow.color = bgColours [Rand];
		BGMat.color = bgColours [Rand];
		BG.SetActive (false);
	}

	public void HideInGameGUI()
	{
		//HideAllPages ();
		inGameGUI.gameObject.SetActive(false);
		BG.SetActive (true);
	}


    public void ShowGameOverGUI()
    {
	//	gameOverGUI.OnWin ();
		if (!PlayerPrefs.HasKey ("TutorialComplete")) {
			PlayerPrefs.SetInt ("TutorialComplete", 1);
			ShowTutorialLog ("Congratulations !! You've successfully completed the tutorial !!");
			inGameGUI.GetComponent <UIElement> ().Hide (false);
		//	Invoke ("ReloadScene", 3);
			Invoke ("FinishTut", 3);
			return;
		}



        gameOverGUI.gameObject.SetActive(true);
		gameOverGUI.GetComponent <UIElement> ().Show (false);
    }

	void FinishTut(){
		ObliusGameManager.instance.CloseTutorial ();
	}

    public void HideGameOverGUI()
    {
		gameOverGUI.GetComponent <UIElement> ().Hide (false);
		if (PhotonManagerAdvanced.instance.IsInGame ())
			PhotonNetwork.LeaveRoom ();

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
        ObliusGameManager.instance.gameState = ObliusGameManager.GameState.menu;
        mainMenuGUI.gameObject.SetActive(true);
		mainMenuGUI.GetComponent <UIElement> ().Show (false);

		SnakesSpawner.instance.previewMeshContainer.transform.parent.gameObject.SetActive (true);
		BG.SetActive (true);
    }



	public void OpenPage(int pageNo){
//		if (currentPage == pageNo)
//			return;
		HideAllPages ();

		Pages [pageNo - 1].gameObject.SetActive (true);
		Pages [pageNo-1].Show (false);
		currentPage = pageNo;
	}

	public void HideAllPages()
	{

		foreach (UIElement page in Pages)
			page.Hide (false);
	}

	public void ReloadScene(){
		InternetCheckPage.Hide (false);
//		if(PhotonManagerAdvanced.instance.IsInGame())
		PhotonNetwork.LeaveRoom();
		Application.LoadLevel (0);
	//	SceneManager.LoadScene (0);
	}

	public void OpenConnectionPopup(){
		if (PhotonManagerAdvanced.instance.IsInGame ()) {
			InternetCheckPage.gameObject.SetActive (true);
			InternetCheckPage.Show (false);
		}
	}


	public void ShowSettings(){
		SettingsPage.gameObject.SetActive (true);
		SettingsPage.Show (false);
	}

	public void HideSettings(){
		SettingsPage.Hide (false);
	}
	Sprite NotificationSprite;
	bool isLogShown;
	public void ShowLog(string log){
		if (isLogShown)
			return;

		UIManager.ShowNotification("Example_1_Notification_4", 1f, true, log, NotificationSprite);
		isLogShown = true;

		Invoke ("EnableLog", 1);
	}

	public void ShowLog(string log,float time){
		if (isLogShown)
			return;

		UIManager.ShowNotification("Example_1_Notification_4", time, true, log, NotificationSprite);
		isLogShown = true;

		Invoke ("EnableLog", time);
	}

	public Text NotificationTxt;
	public void ShowTutorialLog(string log){
		NotificationTxt.transform.parent.parent.gameObject.SetActive (true);
		NotificationTxt.text = log;

	}
	public void ShowTutorialLog(string log, float ticks){
		NotificationTxt.transform.parent.parent.gameObject.SetActive (true);
		NotificationTxt.text = log;
		StartCoroutine (HideTutorialLogRoutine (0,ticks));
	}

	public void ShowTutorialLog(float delay,string log, float ticks){
		NotificationTxt.transform.parent.parent.gameObject.SetActive (true);
		NotificationTxt.text = log;
		StartCoroutine (HideTutorialLogRoutine (delay,ticks));
	}

	IEnumerator HideTutorialLogRoutine(float delay,float ticks){

		isLogShown = true;
		yield return new WaitForSeconds (delay);
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime (ticks);
		Time.timeScale = 1;
		HideTutorialLog ();
	
	}

	public void HideTutorialLog(){
		NotificationTxt.transform.parent.parent.gameObject.SetActive (false);
	}


	void EnableLog(){
		isLogShown = false;
	}
}
