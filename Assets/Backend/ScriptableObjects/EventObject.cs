using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objects/Event")]
public class EventObject : ScriptableObject 
{
	List<EventListner> events = new List<EventListner>();

	public void Add(EventListner listner)
	{
		events.Add (listner);
	}

	public void Remove(EventListner listner)
	{
		events.Remove (listner);
	}

	public void Fire()
	{
		foreach (var e in events) 
		{
			e.Fire ();
		}
	}
}
