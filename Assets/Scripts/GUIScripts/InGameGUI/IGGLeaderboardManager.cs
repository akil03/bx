using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGGLeaderboardManager : MonoBehaviour
{

	public static IGGLeaderboardManager instance;

	public float updateRate;
	public List<Snake> snakes;

	void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public IEnumerator UpdateLeaderboard ()
	{

		while (true) {
			try {
				snakes = new List<Snake> (SnakesSpawner.instance.spawnedSnakes);
				snakes.Sort ((s2, s1) => s1.ownedGroundPieces.Count.CompareTo (s2.ownedGroundPieces.Count));
			} catch {

			}
			yield return new WaitForSeconds (updateRate);	
		}
	}


	public float ScoreToPercentage (float score)
	{
		return (100 * score) / (float)GroundSpawner.instance.spawnedGroundPieces.Count;
	}

	void OnEnable(){
		StopAllCoroutines ();
		StartCoroutine (UpdateLeaderboard ());

	}

}
