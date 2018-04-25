using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using DG.Tweening;
public class InstantReplayManager : MonoBehaviour {
    public static InstantReplayManager instance;
    AnimatedClip recordedClip1,recordedClip2,currentClip;

    [Header("GIF Settings")]
    public Recorder[] recorder;
    public string gifFilename = "XonixHit";
    public int loop = 0;
    [Range(1, 100)]
    public int quality = 80;
    public System.Threading.ThreadPriority exportThreadPriority;

    [Header("UI Stuff")]
    public GameObject playbackPanel;
    public ClipPlayerUI clipPlayer;

    private void Awake()
    {
        instance = this;
    }

    void OnDisable()
    {
        // Dispose the used clip if needed
        //if (recordedClip != null)
        //    recordedClip.Dispose();
    }

    void OnEnable()
    {
        if (recordedClip1 != null)
                recordedClip1.Dispose();
        
        if (recordedClip2 != null)
                recordedClip2.Dispose();
        
    }

  

    public void StartRecording(int playerID)
    {
        // Dispose the old clip

        if (recordedClip1 != null)
            recordedClip1.Dispose();

        if (recordedClip2 != null)
            recordedClip2.Dispose();

        Gif.StartRecording(recorder[playerID]);

       // StartCoroutine(CheckAndRecord(playerID));
        print("start record");
    }

    IEnumerator CheckAndRecord(int playerID)
    {
        while (isPlaying)
            yield return null;

        OnEnable();
        Gif.StartRecording(recorder[playerID]);
    }

    public void StopRecording(int playerID)
    {

      


        if (playerID == 0)
        {
            
            recordedClip1 = Gif.StopRecording(recorder[playerID]);
            if (recordedClip1 == null)
                return;
        }
        else
        {
           
            recordedClip2 = Gif.StopRecording(recorder[playerID]);
            if (recordedClip2 == null)
                return;
        }


        

        //
        OpenPlaybackPanel(playerID);
    }

    bool isPlaying;
    public void OpenPlaybackPanel(int playerID)
    {

        if (currentClip != null)
            currentClip.Dispose();

        if (isPlaying)
            CancelInvoke();

        if (playerID == 0)
        {
            if (recordedClip1 != null)
            {
                playbackPanel.SetActive(true);
                playbackPanel.transform.DOScale(Vector3.one, 0.5f);
                currentClip = recordedClip1;
                Gif.PlayClip(clipPlayer, currentClip, 0.25f, false);
                Invoke("ClosePlaybackPanel", currentClip.Length + 0.5f);
                isPlaying = true;
            }
            else
            {
                NativeUI.Alert("Nothing Recorded", "Please finish recording first.");
            }
        }
        else
        {
            if (recordedClip2 != null)
            {
                playbackPanel.SetActive(true);
                playbackPanel.transform.DOScale(Vector3.one, 0.5f);
                currentClip = recordedClip2;
                Gif.PlayClip(clipPlayer, currentClip, 0.25f, false);
                Invoke("ClosePlaybackPanel", currentClip.Length+0.5f);
                isPlaying = true;
            }
            else
            {
                NativeUI.Alert("Nothing Recorded", "Please finish recording first.");
            }
        }

        
    }

    public void ClosePlaybackPanel()
    {
        clipPlayer.Stop();
        playbackPanel.transform.DOScale(Vector3.zero, 0.5f);
        playbackPanel.SetActive(false);
        isPlaying = false;
        
    }

}
