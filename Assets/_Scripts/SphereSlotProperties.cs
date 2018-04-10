using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SphereSlotProperties : MonoBehaviour {
	public GameObject sphereSlot, sphereRender, tapToOpen, openNow, sphereUnlock, glow;
	public SphereProperties _sphereProperties,_unlockProperties;
	public Text timer, gemCount, gemCountWindow, AdCount, timerWindow, adCountWindow, randomGold, randomGem, rewardGold, rewardGems;
	public string sphereType,unlockTime;
    public int slotNo;
    public int silverMin, silverMax, goldMin, goldMax, crystalMin, crystalMax;
    public int silverMinGem, silverMaxGem, goldMinGem, goldMaxGem, crystalMinGem, crystalMaxGem;
    public int requiredGem, requiredAds;

    public RawImage popupRender;
    public RenderTexture[] slotPreviews;
    // Use this for initialization
    void Start () {
		Empty();

	}
	
	// Update is called once per frame
	void Update () {
		if (unlockTime != "")
			CalculateTimings();
	}

	public void OnClick()
	{

        if (timer.text == "Open !!")
        {
            OpenSphere();

        }
        else if (!SphereSlotManager.instance.activeSlot && unlockTime=="")
        {
            StartUnlock();
        }
        else
        {
            SphereSlotManager.instance.OnClick();
        }

        return;

        if (sphereType == "") {
            RandomSphere();
            return;
		}
		if (unlockTime == "") {
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

	public void CalculateTimings()
	{
		DateTime a = DateTime.Parse(unlockTime);
		TimeSpan b = DateTime.Now - a;
		float secRemaining = (int)b.TotalSeconds;
		int targetsecs=0;

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

		b = TimeSpan.FromSeconds(targetsecs-secRemaining);
        secRemaining = (int)b.TotalSeconds;

        if (secRemaining < 1)
		{
			timer.text = "Open !!";
			AdCount.text = "";
			gemCount.text = "";
		}            
		else {
			timer.text = b.Hours + " : " + b.Minutes.ToString("00") + " : " + b.Seconds.ToString("00") + " ";
            timerWindow.text = b.Hours + " : " + b.Minutes.ToString("00") + " : " + b.Seconds.ToString("00") + " ";
            AdCount.text = Mathf.CeilToInt(secRemaining / 1800).ToString();
            adCountWindow.text = "Watch Ads ( " + Mathf.CeilToInt(secRemaining / 1800).ToString() + " )";
            gemCount.text = Mathf.CeilToInt(secRemaining/60*0.69f).ToString();
            gemCountWindow.text = Mathf.CeilToInt(secRemaining / 60 * 0.69f).ToString()+ " Gems";
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

	public void OpenSphere()
	{
        int gold = 0, gem = 0;
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


        rewardGold.text = gold.ToString();
        rewardGems.text = gem.ToString();

        AccountDetails.instance.Save(Gold: gold);
        AccountDetails.instance.Save(Gem: gem);

        _unlockProperties.gameObject.SetActive(true);
        _unlockProperties.OpenSphere();
		sphereUnlock.SetActive(true);
        SphereSlotManager.instance.activeSlot = null;
		
        Empty();

        if (slotNo == 1)
            AccountDetails.instance.Save(slot1: "0");
        else if (slotNo == 2)
            AccountDetails.instance.Save(slot2: "0");
        else if (slotNo == 3)
            AccountDetails.instance.Save(slot3: "0");
        else if (slotNo == 4)
            AccountDetails.instance.Save(slot4: "0");

        Invoke("AssignRewards", 2.5f);

	}

	public void AssignRewards()
	{
        
        glow.SetActive(true);
        glow.transform.localScale = Vector3.zero;
        glow.transform.DOScale(Vector3.one * 2, 0.5f);
        rewardGold.transform.parent.gameObject.SetActive(true);

        Invoke("CloseMachine", 2f);

	}

	public void CloseMachine()
	{
		glow.SetActive(false);
		sphereUnlock.SetActive(false);
        rewardGold.transform.parent.gameObject.SetActive(false);
        _unlockProperties.gameObject.SetActive(false);
    }
}
