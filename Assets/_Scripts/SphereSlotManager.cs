using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class SphereSlotManager : MonoBehaviour {
    public SphereSlotProperties slot1, slot2, slot3, slot4, activeSlot;
    public GameObject OpenPopup;
    //public Text 

    public static SphereSlotManager instance;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int AssignSphere()
    {
        if (slot1.sphereType == "")
        {
            slot1.RandomSphere();
            return slot1.slotNo;
        }
        else if (slot2.sphereType == "")
        {
            slot2.RandomSphere();
            return slot2.slotNo;
        }
        else if (slot3.sphereType == "")
        {
            slot3.RandomSphere();
            return slot3.slotNo;
        }
        else if (slot4.sphereType == "")
        {
            slot4.RandomSphere();
            return slot4.slotNo;
        }
        else
        {
            return 0;
        }


    }

    public void OnClick()
    {
        if (activeSlot)
            OpenPopup.SetActive(true);
    }


    public void OnPayGem()
    {
        activeSlot.Paid();
        OpenPopup.SetActive(false);

    }

    public void ShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = AdsCallback;

        Advertisement.Show("rewardedVideo", options);
    }


    

    void AdsCallback(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            //Debug.Log("Video completed - Offer a reward to the player");
            activeSlot.WatchAd();
            OpenPopup.SetActive(false);

        }
        else if (result == ShowResult.Skipped)
        {
            //Debug.LogWarning("Video was skipped - Do NOT reward the player");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }



}
