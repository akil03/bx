using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Objects/String")]
public class StringObject : ScriptableObject
{
	public string value;
	[SerializeField]string resetValue;

	public void Reset()
	{		
		value = resetValue;
	}
}
