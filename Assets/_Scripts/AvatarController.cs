using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour {
	public AnimationStates AnimStates;
	public Animator _animator;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Run(){
		_animator.Play (AnimStates.Run);
	}


	public void GoLeft(){
	//	_animator.Play (AnimStates.TurnLeft);
	}

	public void GoRight(){
	//	_animator.Play (AnimStates.TurnRight);
	}

	public void Kill(){
		int Rand = Random.Range (0, 50);
	//	if(Rand<25)
			_animator.Play (AnimStates.Kill);
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
