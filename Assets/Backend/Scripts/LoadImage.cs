using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadImage : MonoBehaviour {

	public Image profile; 

	IEnumerator _Load(string fbId)
	{
		WWW request = new WWW (("https://graph.facebook.com/"+fbId+"/picture?width=100&height=100"));
		yield return request;
		profile.sprite = Sprite.Create (request.texture, new Rect (0, 0, 100, 100), Vector2.zero);
	}

	public void Load(string fbId)
	{
		StartCoroutine (_Load(fbId));
	}


}
