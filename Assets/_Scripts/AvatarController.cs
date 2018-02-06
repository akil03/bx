using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour {
	public AnimationStates AnimStates;
	public Animator _animator;
	public GameObject Ball,GameBall;
	// Use this for initialization
	void Awake () {
		Idle ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Idle(){
		_animator.Play (AnimStates.Idle);
		if (Ball)
			Ball.SetActive (true);

		GameBall.SetActive (false);
	}

	public void Run(){
		print ("run");
		_animator.Play (AnimStates.Run);
		if (Ball)
			Ball.SetActive (false);

		GameBall.SetActive (true);
	}


	public void GoLeft(){
		//_animator.Play (AnimStates.TurnLeft);
	}

	public void GoRight(){
		//_animator.Play (AnimStates.TurnRight);
	}

	public void Kill(){
		int Rand = Random.Range (0, 50);
	//	if(Rand<25)
		_animator.Play (AnimStates.Spin);
	//	else
	//		_animator.Play (AnimStates.Spin);
	}

	public void GoSpin(){
		_animator.Play (AnimStates.Spin);
	}

	public void DeathAnim(){
		_animator.Play (AnimStates.Death);
	}


	[System.Serializable]
	public class AnimationStates{
		public string Idle = "Idle";
		public string Run = "MediumRun";
		public string FastRun = "FastRun";
		public string Spin = "Spin";
		public string TurnLeft = "PassLeft";
		public string TurnRight = "PassRight";
		public string Kill = "Kill";
		public string Death = "Death";
	}
}
