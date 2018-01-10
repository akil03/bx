using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}	
	
	// Update is called once per frame
	void Update () {
		
	}

	[PunRPC]
	public void Destroy()
	{
		if (PhotonView.Get(gameObject).isMine) {
		
			PowerUpManager.instance.spawnedPowerups.Remove (gameObject);
			PhotonNetwork.Destroy (gameObject);
		}
	}
}
