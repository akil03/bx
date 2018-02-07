using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Objects/Friends")]
public class FbFriendsObject : ScriptableObject
{
	public FBFriendsData value;


	public void Reset()
	{
		value = new FBFriendsData ();
	}
}
