using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PowerUpManager : MonoBehaviour {

	public static PowerUpManager instance;
	public GameObject[] Powerups;
	public List<GameObject> spawnedPowerups;
	public float minInterval,maxInterval;
	public bool dontSpawn;
	void Awake(){
		instance = this;
	}

	public void StartSpawn () {
		ClearPowerUps ();
		NetworkClear ();
		//SpawnRunePowerup ();
		CancelInvoke ();
		ClearPowerUpsLocal ();
		Invoke ("SpawnPowerupLocal",5);
	}

	public void StartNetworkPower(){
		ClearPowerUps ();
		NetworkClear ();
		SpawnNetworkRunePowerup ();
		Invoke ("StartSpawnTimer",5);
	}

	void StartSpawnTimer()
	{
		dontSpawn = false;
	}

	public void StopSpawn(){
		dontSpawn = true;
		CancelInvoke ();
	}

	// Update is called once per frame
	void Update () {
		
	}

//	public void SpawnPowerup(){
//
//		ClearPowerUps ();
//
//		int Rand = Random.Range (0, Powerups.Length);
//		GameObject Go = Instantiate (Powerups [Rand], transform);
//		Go.transform.localPosition = new Vector3 (Random.Range (-4.5f, 4.5f), 0, Random.Range (-4.5f, 4.5f));
//
//		spawnedPowerups.Add (Go);
//		Invoke ("SpawnPowerup",Random.Range (minInterval,maxInterval));
//	}


	public void SpawnPowerupLocal(){
		int Rand;

		Rand = Random.Range (0, Powerups.Length);
		GameObject Go;
		Go = Instantiate (Powerups [Rand], transform);
		Go.transform.localPosition = new Vector3 (-4, 0, 0);
		spawnedPowerups.Add (Go);

		Rand = Random.Range (0, Powerups.Length);
		Go = Instantiate (Powerups [Rand], transform);
		Go.transform.localPosition = new Vector3 (4, 0, 0);

		spawnedPowerups.Add (Go);

		Invoke ("SpawnPowerupLocal", 20);
		Invoke ("ClearPowerUpsLocal", 15);
	}

	public void ClearPowerUpsLocal(){
		if (spawnedPowerups.Count<1)
			return;


		foreach (GameObject GO in spawnedPowerups.ToArray()) {
			try {
				spawnedPowerups.Remove (GO);
				GO.transform.DOScale (Vector3.zero,0.4f).OnComplete (()=>{
					Destroy (GO);
				});

			} catch {
				print ("stupid bug");
			}
		}
	}



	public void SpawnRunePowerup(){
		if (!PhotonNetwork.inRoom) {
			if (!dontSpawn) {
				ClearPowerUps ();
				int Rand;

				Rand = Random.Range (0, Powerups.Length);
				GameObject Go;
				Go = Instantiate (Powerups [Rand], transform);
				Go.transform.localPosition = new Vector3 (-4, 0, 0);
				spawnedPowerups.Add (Go);

				Rand = Random.Range (0, Powerups.Length);
				Go = Instantiate (Powerups [Rand], transform);
				Go.transform.localPosition = new Vector3 (4, 0, 0);

				spawnedPowerups.Add (Go);
			}
			Invoke ("SpawnRunePowerup",20);
			Invoke ("ClearPowerUps",15);
		}
	}





	public void SpawnNetworkRunePowerup(){
		if (PhotonNetwork.inRoom && PhotonNetwork.isMasterClient) 
		{
			if (!dontSpawn) 
			{
				NetworkClear ();
				int Rand;
				Rand = Random.Range (0, Powerups.Length);
				GameObject Go1;
				GameObject Go2;
				if (PhotonNetwork.inRoom) {
					Go1 = PhotonNetwork.Instantiate("Powerups/"+Powerups [Rand].name,new Vector3 (1.5f, -8, 0), Quaternion.identity, new byte());
					spawnedPowerups.Add (Go1);
					Rand = Random.Range (0, Powerups.Length);
					Go2 = PhotonNetwork.Instantiate("Powerups/"+Powerups [Rand].name, new Vector3 (14.5f, -8, 0), Quaternion.identity, new byte());
					spawnedPowerups.Add (Go2);
				}
			}
			Invoke ("SpawnNetworkRunePowerup",20);
			Invoke ("NetworkClear",15);
		}
	}

	public void ClearPowerUps(){

		if (spawnedPowerups.Count<1)
			return;


		foreach (GameObject GO in spawnedPowerups.ToArray()) {
			try {
				spawnedPowerups.Remove (GO);
				Destroy (GO);
			
			} catch {
				print ("stupid bug");
			}
		}
	}

	public void RemovePowerUp(GameObject GO){


		spawnedPowerups.Remove (GO);
		Destroy (GO);

	}

	public void RemovePowerUpNetwork(GameObject GO){

		spawnedPowerups.Remove (GO);
		PhotonView.Get (GO).RPC ("Destroy",PhotonTargets.MasterClient);
	}

	public void NetworkClear(){
		PowerUpDestroy[] powerups = GameObject.FindObjectsOfType<PowerUpDestroy> ();
		foreach(var p in powerups)
			PhotonView.Get (p.gameObject).RPC ("Destroy",PhotonTargets.MasterClient);
	}

}
