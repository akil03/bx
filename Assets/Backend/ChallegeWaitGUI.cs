using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;

public class ChallegeWaitGUI : Singleton<ChallegeWaitGUI> 
{
	public CreateChallengeData data;
	public WithdrawChallenge withdraw;
	public UIElement element;
	public bool isBusy;

	void OnEnable()
	{
		EventManager.instance.withdrawChallenge += Withdraw;
	}

	void OnDisable()
	{
		if(EventManager.instance!=null)
		EventManager.instance.withdrawChallenge -= Withdraw;
	}

	public void Set(CreateChallengeData data)
	{
		this.data = data;
		element.gameObject.SetActive (true);
		element.Show (false);
	}

	public void Withdraw()
	{
		if (data!=null) 
		{
			withdraw.Withdraw (data.challengeInstanceId);
			Hide ();
			isBusy = false;
			data = null;	
		}
	}

	public void Hide()
	{
		element.Hide (false);
	}
}