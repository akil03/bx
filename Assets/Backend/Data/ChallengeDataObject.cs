using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Events/ChallengeData")]
public class ChallengeDataObject : ScriptableObject
{
	public ChallengeData data;

	public void Reset()
	{
		data = new ChallengeData ();
	}
}
