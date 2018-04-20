using GameSparks.Core;
using System.Collections;
using UnityEngine;

public class UnityDeeplinks : MonoBehaviour
{

#if UNITY_IOS
	[DllImport("__Internal")]
	private static extern void UnityDeeplinks_init(string gameObject = null, string deeplinkMethod = null);
#endif
    public GameSparksActor gameSparksActor;
    // Use this for initialization
    void Start()
    {
#if UNITY_IOS
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			UnityDeeplinks_init(gameObject.name);
		}
#endif
    }


    public void onDeeplink(string deeplink)
    {
        StartCoroutine(WaitAndAdd(deeplink));
    }

    IEnumerator WaitAndAdd(string deeplink)
    {
        while (!GS.Authenticated || !GS.Available)
        {
            yield return null;
        }
        deeplink = deeplink.Replace("https://www.battlexonix.com/", string.Empty);
        gameSparksActor.AddGoogleFriend(deeplink);
    }

}
