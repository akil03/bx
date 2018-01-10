using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;


public class SwipeHandler : MonoBehaviour {

	public static SwipeHandler instance;

//	public  delegate TapEvent;
//	public static event TapEvent OnDoubleTap;

	public float swipeSensibility;
	Vector2 lastMousePos;
	public delegate void InputEvent();
	public static InputEvent OnDoubleTap;


	public enum SwipeDirection {up,down,left,right};

	public SwipeDirection lastSwipeDirection;
	public bool isSwiped;

	public float TapInterval;

	public int InputType;

	public GameObject Joystick, Dpad;

	float lastTap;
	void Awake(){
		instance = this;
		lastSwipeDirection = SwipeDirection.up;
		AssignInput ();
	}

	public void AssignInput(){
		
		if (InputType == 1) {
			Joystick.SetActive (false);
		//	Dpad.SetActive (false);
		} else if (InputType == 2) {
			Joystick.SetActive (true);
		//	Dpad.SetActive (false);
		} else {
			Joystick.SetActive (false);
		//	Dpad.SetActive (true);
		}

	}

	public void SetInput(int Type){
		InputType = Type;
		AssignInput ();
	}

	// Use this for initialization
	void Start () {
		
	}


	void OnEnable(){
		EasyTouch.On_SwipeStart += On_SwipeStart;
		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;		
		EasyTouch.On_DoubleTap += On_DoubleTap;

		OnDoubleTap += OnDoubleTapTest;
	}

	void OnDisable(){
		UnsubscribeEvent();
		enabled = true;


	}

	void OnDestroy(){
		UnsubscribeEvent();
	}

	void UnsubscribeEvent(){
		EasyTouch.On_SwipeStart -= On_SwipeStart;
		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;	
		EasyTouch.On_DoubleTap -= On_DoubleTap;

		OnDoubleTap -= OnDoubleTapTest;
	}


	// At the swipe beginning 
	private void On_SwipeStart( Gesture gesture){




	}

	// During the swipe
	private void On_Swipe(Gesture gesture){
		if (InputType > 1)
			return;


		if (isSwiped)
			return;

		switch (gesture.swipe){

		case EasyTouch.SwipeDirection.Left:
			lastSwipeDirection = SwipeDirection.left;
			isSwiped = true;
			break;

		case EasyTouch.SwipeDirection.Right:
			lastSwipeDirection = SwipeDirection.right;
			isSwiped = true;
			break;
		case EasyTouch.SwipeDirection.Up:
			lastSwipeDirection = SwipeDirection.up;
			isSwiped = true;
			break;
		case EasyTouch.SwipeDirection.Down:
			lastSwipeDirection = SwipeDirection.down;
			isSwiped = true;
			break;

		}

	}

	public void OnLeft(){
		lastSwipeDirection = SwipeDirection.left;
	}

	public void OnRight(){
		lastSwipeDirection = SwipeDirection.right;
	}

	public void OnUp(){
		lastSwipeDirection = SwipeDirection.up;
	}

	public void OnDown(){
		lastSwipeDirection = SwipeDirection.down;
	}

	void On_DoubleTap (Gesture gesture)
	{
		OnDoubleTap ();
	}

	// At the swipe end 
	private void On_SwipeEnd(Gesture gesture){

		isSwiped = false;

	}



	
	// Update is called once per frame
//	void Update () {
//
//		if (Input.GetMouseButtonDown (0)) {
//			lastMousePos = Input.mousePosition;
//			isSwiped = false;
//			
//
//			
//		}
//
//
//		if (Input.GetMouseButton(0)) {
//			Vector2 currentMousePos = Input.mousePosition;
//
//			if (Vector2.Distance (lastMousePos, currentMousePos) > swipeSensibility) {
//				if (Mathf.Abs (currentMousePos.x - lastMousePos.x) > Mathf.Abs (currentMousePos.y - lastMousePos.y)) {
//				
//				
//					if (currentMousePos.x > lastMousePos.x) {
//						if (!isSwiped) {
//							lastSwipeDirection = SwipeDirection.right;
//							isSwiped = true;
//						}
//
//					} else {
//						if (!isSwiped) {
//							lastSwipeDirection = SwipeDirection.left;
//							isSwiped = true;
//						}
//					}
//
//
//				} else {
//					if (currentMousePos.y > lastMousePos.y) {
//						if (!isSwiped) {
//							lastSwipeDirection = SwipeDirection.up;
//							isSwiped = true;
//						}
//					} else {
//						if (!isSwiped) {
//							lastSwipeDirection = SwipeDirection.down;
//							isSwiped = true;
//						}
//					}
//				}
//
//			}
//
//			lastMousePos = currentMousePos;
//		}
//
//	}
//
//	void LateUpdate(){
//		if (isSwiped)
//			return;
//		
//		if (Input.GetMouseButtonDown (0)) {
//
//			if(Time.time<lastTap+TapInterval)
//				OnDoubleTap ();
//
//			lastTap = Time.time;
//		}
//	}
//
	void OnDoubleTapTest(){
		print ("OnDoubleTap");
	}
}
