using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListner : MonoBehaviour {

	public EventObject _event;
	public UnityEvent response;

	void OnEnable()
	{
		_event.Add (this);
	}

	void OnDisable()
	{
		_event.Remove (this);
	}

	public void Fire()
	{
		response.Invoke ();
	}
}
