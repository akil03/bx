using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	static bool quit=false;
	static object m_lock = new object();
	public static T instance
	{
		get
		{
			if (quit)
				return null;
			lock(m_lock)
			{
				if (m_instance == null) {
					m_instance = GameObject.FindObjectOfType<T> ();
				}
				if (m_instance == null) {
					m_instance = MyExtensions.Create<T> ();
				}
				return m_instance;
			}
		}
	}

	static T m_instance;

	void Awake()
	{
		if (this != instance)
			Destroy (gameObject);
		
	}

	void OnDestroy()
	{
		quit = true;
	}
}
