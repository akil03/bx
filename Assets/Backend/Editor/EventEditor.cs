using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventObject))]
public class EventEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
		GUI.enabled = Application.isPlaying;
		if (GUILayout.Button ("Fire")) 
		{
			EventObject e = target as EventObject;
			e.Fire ();
		}
	}
}
