using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SphereSlotProperties : MonoBehaviour
{
    public GameObject sphereSlot, sphereRender, tapToOpen, openNow, sphereUnlock, glow;
    public SphereProperties _sphereProperties, _unlockProperties;
    public Text timer, gemCount, gemCountWindow, AdCount, timerWindow, adCountWindow, cardName, cardCount, randomGold, randomGem, rewardGold, rewardGems;
    public string sphereType, unlockTime;
    public int slotNo;
    public int silverMin, silverMax, goldMin, goldMax, crystalMin, crystalMax;
    public int silverMinGem, silverMaxGem, goldMinGem, goldMaxGem, crystalMinGem, crystalMaxGem;
    public int requiredGem, requiredAds;

    public RectTransform machineUI;
    public RawImage popupRender;
    public RenderTexture[] slotPreviews;

    public GameObject selectedCard,goldCard,gemCard;
    public Transform fromPosition, toPosition;
    public SpriteRenderer cardGrid;
    // Use this for initialization
    void Start()
    {
        Empty();

        //if (slotNo < 3)
        //    RandomSphere();
            //Invoke("Test", 8);

    }

    

    // Update is called once per frame
    void Update()
    {
        if (unlockTime != "")
            CalculateTimings();
    }

    public void OnClick()
    {

        if (timer.text == "Open !!")
        {
            OpenSphere();

        }
        else if (!SphereSlotManager.instance.activeSlot && unlockTime == "" && sphereType!="")
        {
            StartUnlock();
        }
        else
        {
            SphereSlotManager.instance.OnClick();
        }

        return;

        if (sphereType == "")
        {
            RandomSphere();
            return;
        }
        if (unlockTime == "")
        {
            StartUnlock();
            return;
        }
        else
            OpenSphere();



    }

    public void Empty()
    {
        sphereSlot.SetActive(true);
        sphereRender.SetActive(false);
        tapToOpen.SetActive(false);
        openNow.SetActive(false);

        sphereType = "";
        unlockTime = "";
        timer.text = "";
        AdCount.text = "";
        gemCount.text = "";
    }


    public void RandomSphere()
    {
        int rand = UnityEngine.Random.Range(0, 1000);
        if (rand < 500)
            SetSphere("silver");
        else if (rand < 850)
            SetSphere("gold");
        else if (rand < 1000)
            SetSphere("Crystal");
    }

    public void SetSphere(string sphereType)
    {
        switch (sphereType)
        {
            case "silver":
                _sphereProperties.ApplySilver();
                timer.text = "3 Hrs";
                randomGold.text = silverMin + "-" + silverMax;
                randomGem.text = silverMinGem + "-" + silverMaxGem;
                break;
            case "gold":
                _sphereProperties.ApplyGold();
                timer.text = "8 Hrs";
                randomGold.text = goldMin + "-" + goldMax;
                randomGem.text = goldMinGem + "-" + goldMaxGem;
                break;
            case "Crystal":
                _sphereProperties.ApplyCrystal();
                timer.text = "24 Hrs";
                randomGold.text = crystalMin + "-" + crystalMax;
                randomGem.text = crystalMinGem + "-" + crystalMaxGem;
                break;
        }

        this.sphereType = sphereType;
        _sphereProperties.Idle();
        sphereRender.SetActive(true);
        tapToOpen.SetActive(true);
        unlockTime = "";

        if (slotNo == 1)
            AccountDetails.instance.Save(slot1: sphereType);
        else if (slotNo == 2)
            AccountDetails.instance.Save(slot2: sphereType);
        else if (slotNo == 3)
            AccountDetails.instance.Save(slot3: sphereType);
        else if (slotNo == 4)
            AccountDetails.instance.Save(slot4: sphereType);


    }

    public void SetFromCloud()
    {
        if (sphereType == "")
        {
            Empty();
            return;
        }


        switch (sphereType)
        {
            case "silver":
                _sphereProperties.ApplySilver();
                timer.text = "3 Hrs";
                randomGold.text = silverMin + "-" + silverMax;
                randomGem.text = silverMinGem + "-" + silverMaxGem;
                break;
            case "gold":
                _sphereProperties.ApplyGold();
                timer.text = "8 Hrs";
                randomGold.text = goldMin + "-" + goldMax;
                randomGem.text = goldMinGem + "-" + goldMaxGem;
                break;
            case "Crystal":
                _sphereProperties.ApplyCrystal();
                timer.text = "24 Hrs";
                randomGold.text = crystalMin + "-" + crystalMax;
                randomGem.text = crystalMinGem + "-" + crystalMaxGem;
                break;
        }

        _sphereProperties.Idle();
        sphereRender.SetActive(true);
        tapToOpen.SetActive(true);



        if (unlockTime != "")
        {
            tapToOpen.SetActive(false);
            openNow.SetActive(true);
            _sphereProperties.StartUnlock();
            SphereSlotManager.instance.activeSlot = this;
        }


    }

    public void StartUnlock()
    {
        tapToOpen.SetActive(false);
        openNow.SetActive(true);

        _sphereProperties.StartUnlock();
        unlockTime = DateTime.Now.ToString();

        SphereSlotManager.instance.activeSlot = this;

        if (slotNo == 1)
            AccountDetails.instance.Save(slot1: sphereType + "," + unlockTime);
        else if (slotNo == 2)
            AccountDetails.instance.Save(slot2: sphereType + "," + unlockTime);
        if (slotNo == 3)
            AccountDetails.instance.Save(slot3: sphereType + "," + unlockTime);
        if (slotNo == 4)
            AccountDetails.instance.Save(slot4: sphereType + "," + unlockTime);


        popupRender.texture = slotPreviews[slotNo - 1];

    }


    public void SetNotificationTime()
    {
        if (Application.isEditor)
            return;

        int targetsecs = 0;
        switch (sphereType)
        {
            case "silver":
                targetsecs = 10800;
                break;
            case "gold":
                targetsecs = 28800;
                break;
            case "Crystal":
                targetsecs = 86400;
                break;
        }

        NotificationManager.instance.Chestunlocked(slotNo.ToString(), sphereType, targetsecs);
    }

    public void CalculateTimings()
    {
        DateTime a = DateTime.Parse(unlockTime);
        TimeSpan b = DateTime.Now - a;
        float secRemaining = (int)b.TotalSeconds;
        int targetsecs = 0;

        switch (sphereType)
        {
            case "silver":
                targetsecs = 10800;
                break;
            case "gold":
                targetsecs = 28800;
                break;
            case "Crystal":
                targetsecs = 86400;
                break;
        }

        b = TimeSpan.FromSeconds(targetsecs - secRemaining);
        secRemaining = (int)b.TotalSeconds;

        if (secRemaining < 1)
        {
            timer.text = "Open !!";
            AdCount.text = "";
            gemCount.text = "";
        }
        else
        {
            timer.text = b.Hours + " : " + b.Minutes.ToString("00") + " : " + b.Seconds.ToString("00") + " ";
            timerWindow.text = b.Hours + " : " + b.Minutes.ToString("00") + " : " + b.Seconds.ToString("00") + " ";
            AdCount.text = Mathf.CeilToInt(secRemaining / 1800).ToString();
            adCountWindow.text = "Watch Ads ( " + Mathf.CeilToInt(secRemaining / 1800).ToString() + " )";
            gemCount.text = Mathf.CeilToInt(secRemaining / 60 * 0.69f).ToString();
            gemCountWindow.text = Mathf.CeilToInt(secRemaining / 60 * 0.69f).ToString() + " Gems";
            requiredGem = Mathf.CeilToInt(secRemaining / 60 * 0.69f);
        }



    }

    public void WatchAd()
    {



        DateTime a = DateTime.Parse(unlockTime).AddMinutes(-30);
        unlockTime = a.ToString();

        if (slotNo == 1)
            AccountDetails.instance.Save(slot1: sphereType + "," + unlockTime);
        else if (slotNo == 2)
            AccountDetails.instance.Save(slot2: sphereType + "," + unlockTime);
        if (slotNo == 3)
            AccountDetails.instance.Save(slot3: sphereType + "," + unlockTime);
        if (slotNo == 4)
            AccountDetails.instance.Save(slot4: sphereType + "," + unlockTime);

    }

    public void Paid()
    {
        //if (AccountDetails.instance.accountDetails.scriptData.Gem < requiredGem)
        //{
        //    return;
        //}

        //AccountDetails.instance.Save(AccountDetails.instance.accountDetails.scriptData.Gem - requiredGem);

        DateTime a = DateTime.Parse(unlockTime).AddDays(-1);
        unlockTime = a.ToString();

        if (slotNo == 1)
            AccountDetails.instance.Save(slot1: sphereType + "," + unlockTime);
        else if (slotNo == 2)
            AccountDetails.instance.Save(slot2: sphereType + "," + unlockTime);
        else if (slotNo == 3)
            AccountDetails.instance.Save(slot3: sphereType + "," + unlockTime);
        else if (slotNo == 4)
            AccountDetails.instance.Save(slot4: sphereType + "," + unlockTime);



    }
    int gold = 0, gem = 0;
    public void OpenSphere()
    {
        
        switch (sphereType)
        {
            case "silver":
                _unlockProperties.ApplySilver();
                gold = UnityEngine.Random.Range(silverMin, silverMax);
                gem = UnityEngine.Random.Range(silverMinGem, silverMaxGem);
                break;
            case "gold":
                _unlockProperties.ApplyGold();
                gold = UnityEngine.Random.Range(goldMin, goldMax);
                gem = UnityEngine.Random.Range(goldMinGem, goldMaxGem);
                break;
            case "Crystal":
                _unlockProperties.ApplyCrystal();
                gold = UnityEngine.Random.Range(crystalMin, crystalMax);
                gem = UnityEngine.Random.Range(crystalMinGem, crystalMaxGem);
                break;
        }


        rewardGold.transform.GetChild(0).GetComponent<Text>().text = "x"+gold.ToString();
        rewardGems.transform.GetChild(0).GetComponent<Text>().text = "x" + gem.ToString();

        _unlockProperties.gameObject.SetActive(true);
        _unlockProperties.OpenSphere();
        sphereUnlock.SetActive(true);
        SphereSlotManager.instance.activeSlot = null;

        Empty();

        if (slotNo == 1)
            AccountDetails.instance.Save(Gold: gold, Gem: gem, slot1: "1");
        else if (slotNo == 2)
            AccountDetails.instance.Save(Gold: gold, Gem: gem, slot2: "1");
        else if (slotNo == 3)
            AccountDetails.instance.Save(Gold: gold, Gem: gem, slot3: "1");
        else if (slotNo == 4)
            AccountDetails.instance.Save(Gold: gold, Gem: gem, slot4: "1");

        Invoke("AssignRewards", 2.5f);

    }

    public void AssignRewards()
    {

        

        
        

        //selectedCard.GetComponent<RectTransform>().DOLocalRotate(new Vector3(0, 1440, 0), 1.5f, RotateMode.LocalAxisAdd);

       // AnimateRewardsa();

        StartCoroutine(AnimateRewards());

    }

    IEnumerator AnimateRewards()
    {
        WeaponsManager.Weapon randomweapon = WeaponsManager.instance.RandomWeapon();
        cardGrid.sprite = randomweapon.icon;
        cardName.text = randomweapon.Name;

        int rand = UnityEngine.Random.Range(1, 25);
        cardCount.text = "x" + rand;

        machineUI.DOLocalMoveY(400, 0.5f);

        selectedCard.transform.SetParent(toPosition);
        goldCard.transform.SetParent(toPosition);
        gemCard.transform.SetParent(toPosition);

        yield return new WaitForEndOfFrame();

        selectedCard.transform.DOLocalRotate(new Vector3(0, 360, 0), 1.5f, RotateMode.LocalAxisAdd);
        selectedCard.transform.DOLocalMove(new Vector3(-2.4f, 0, 0), 1.5f);
        selectedCard.transform.DOScale(Vector3.one*0.65f, 1.5f);
        yield return new WaitForSeconds(0.3f);
        goldCard.transform.DOLocalRotate(new Vector3(0, 360, 0), 1.5f, RotateMode.LocalAxisAdd);
        goldCard.transform.DOLocalMove(new Vector3(0, 0, 0), 1.5f);
        goldCard.transform.DOScale(Vector3.one * 0.65f, 1.5f);
        yield return new WaitForSeconds(0.3f);
        gemCard.transform.DOLocalRotate(new Vector3(0, 360, 0), 1.5f, RotateMode.LocalAxisAdd);
        gemCard.transform.DOLocalMove(new Vector3(2.4f, 0, 0), 1.5f);
        gemCard.transform.DOScale(Vector3.one * 0.65f, 1.5f);

        yield return new WaitForSeconds(1.5f);

        glow.SetActive(true);
        glow.transform.localScale = Vector3.zero;
        glow.transform.GetChild(0).localScale = Vector3.zero;
        glow.transform.DOScale(Vector3.one * 3, 0.5f);
        glow.transform.GetChild(0).DOScale(Vector3.one * 3, 0.5f);
        rewardGold.transform.parent.gameObject.SetActive(true);

        

        cardGrid.DOColor(randomweapon.powerColor, 1);
        cardGrid.transform.parent.GetComponentInParent<SpriteRenderer>().DOColor(randomweapon.powerColor, 1);

       
        cardName.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            cardCount.transform.DOScale(Vector3.one, 0.5f);
        });
        yield return new WaitForSeconds(0.3f);
        rewardGold.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            rewardGold.transform.GetChild(0).DOScale(Vector3.one, 0.5f);
        });
        yield return new WaitForSeconds(0.3f);
        rewardGems.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            rewardGems.transform.GetChild(0).DOScale(Vector3.one, 0.5f);
        });

        Invoke("CloseMachine", 4f);
    }

    void AnimateRewardsa()
    {
        machineUI.DOLocalMoveY(400, 0.5f);
        //cardGrid


        selectedCard.transform.DOLocalRotate(new Vector3(0, 1440, 0), 2f, RotateMode.LocalAxisAdd).OnComplete(() =>
        {
            
            
            cardGrid.DOFade(1, 1);
            cardGrid.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(1, 1).OnComplete(() =>
            {
                cardName.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
                {
                    cardCount.transform.DOScale(Vector3.one, 0.5f);
                });
            });
            
            Invoke("CloseMachine", 5f);
        });
        
        selectedCard.transform.DOMove(toPosition.position, 2);
    }

    public void CloseMachine()
    {
        machineUI.DOLocalMoveY(0, 0.05f);
        glow.SetActive(false);
        sphereUnlock.SetActive(false);


        selectedCard.transform.SetParent(fromPosition);
        selectedCard.transform.localPosition = Vector3.zero;
        selectedCard.transform.localScale = Vector3.zero;
        selectedCard.transform.localRotation = Quaternion.Euler(0,0,0);

        goldCard.transform.SetParent(fromPosition);
        goldCard.transform.localPosition = Vector3.zero;
        goldCard.transform.localScale = Vector3.zero;
        goldCard.transform.localRotation = Quaternion.Euler(0, 0, 0);

        gemCard.transform.SetParent(fromPosition);
        gemCard.transform.localPosition = Vector3.zero;
        gemCard.transform.localScale = Vector3.zero;
        gemCard.transform.localRotation = Quaternion.Euler(0, 0, 0);


        cardGrid.color = Color.white;
        cardGrid.transform.parent.GetComponentInParent<SpriteRenderer>().color = Color.white;

        cardName.transform.localScale = Vector3.zero;
        cardCount.transform.localScale = Vector3.zero;
        rewardGold.transform.localScale = Vector3.zero;
        rewardGold.transform.GetChild(0).localScale = Vector3.zero;
        rewardGems.transform.localScale = Vector3.zero;
        rewardGems.transform.GetChild(0).localScale = Vector3.zero;


        rewardGold.transform.parent.gameObject.SetActive(false);
        _unlockProperties.gameObject.SetActive(false);
    }

   
}