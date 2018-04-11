using UnityEngine;
using UnityEngine.Advertisements;

public class SphereSlotManager : MonoBehaviour
{
    public SphereSlotProperties slot1, slot2, slot3, slot4, activeSlot;
    public GameObject OpenPopup;
    //public Text 

    public static SphereSlotManager instance;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {

    }

    private void OnEnable()
    {
        slot1._sphereProperties.gameObject.SetActive(true);
        slot2._sphereProperties.gameObject.SetActive(true);
        slot3._sphereProperties.gameObject.SetActive(true);
        slot4._sphereProperties.gameObject.SetActive(true);

    }

    private void OnDisable()
    {
        if (slot1._sphereProperties != null)
        {
            slot1._sphereProperties.gameObject.SetActive(false);
        }
        if (slot2._sphereProperties != null)
        {
            slot2._sphereProperties.gameObject.SetActive(false);
        }
        if (slot3._sphereProperties != null)
        {
            slot3._sphereProperties.gameObject.SetActive(false);
        }
        if (slot4._sphereProperties != null)
        {
            slot4._sphereProperties.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
