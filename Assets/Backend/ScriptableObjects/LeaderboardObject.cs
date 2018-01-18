using UnityEngine;
using System;

[CreateAssetMenu(menuName="Objects/LeaderboardData")]
public class LeaderboardObject : ScriptableObject
{
	public GSLeaderboardData value;

	public void Reset()
	{
		value = new GSLeaderboardData ();
	}
}