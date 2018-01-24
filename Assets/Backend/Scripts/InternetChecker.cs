using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DoozyUI;

public class InternetChecker : MonoBehaviour {
	
	public static InternetChecker instance;
	public int InternetReloadTime = 5;
	public Text ServerText, RoomText;
	public bool reconnect;
	public UIElement element;
	// Use this for initialization
	void Awake () {
		instance = this;
		CheckConnection ();
	}
	
	public IEnumerator IsConnected()
	{
		WWW request = new WWW ("www.google.com");
		yield return request;

	}

	void CheckConnection(){

		StartCoroutine (ReloadConnection ());
		//Invoke ("CheckConnection", InternetReloadTime);
	}


	IEnumerator ReloadConnection(){
//		if(element.isActiveAndEnabled)
//			reconnect = true;
		if (reconnect && !PhotonManagerAdvanced.instance.IsInGame ()) 
		{
			CoroutineWithData routine = new CoroutineWithData (this,InternetChecker.instance.IsConnected());
			yield return routine;
			if (!((WWW)routine.result).isDone) 
			{

				//			print ("Internet working");
				if (PhotonManagerAdvanced.instance.serverStatus == ConnectionStatus.disconnected)
					StartCoroutine (PhotonManagerAdvanced.instance._ConnectToMaster ());
			}
			else 
			{

				GUIManager.instance.OpenConnectionPopup ();
				PhotonManagerAdvanced.instance.serverStatus = ConnectionStatus.disconnected;
				PhotonManagerAdvanced.instance.roomStatus= ConnectionStatus.disconnected;
//				ServerText.text = "Server status: " + PhotonManagerAdvanced.instance.serverStatus.ToString ();
//				RoomText.text = "Room status: " + PhotonManagerAdvanced.instance.roomStatus.ToString ();
//				GUIManager.instance.OpenPage (3);
			}
		}
		Invoke ("CheckConnection", InternetReloadTime);
	}




}



public class CoroutineWithData
{
	public Coroutine coroutine;
	public object result;
	public IEnumerator target;

	public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
	{
		this.target = target;
		this.coroutine = owner.StartCoroutine (Run());
	}

	IEnumerator Run()
	{
		while(target.MoveNext()) {
			result = target.Current;
			yield return result;
		}
	}
}