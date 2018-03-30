using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;
    public GameObject[] Powerups;
    public List<GameObject> spawnedPowerups;
    public float minInterval, maxInterval;
    public bool dontSpawn;

    void Awake()
    {
        instance = this;
    }

    public void StartSpawn()
    {
        ClearPowerUps();
        NetworkClear();
        Invoke("StartSpawnTimer", 5);
    }

    public void StartNetworkPower()
    {
        ClearPowerUps();
        NetworkClear();
        Invoke("StartSpawnTimer", 5);
    }

    void StartSpawnTimer()
    {
        dontSpawn = false;
        if (!PhotonNetwork.inRoom)
            SpawnRunePowerup();
        else
            SpawnNetworkRunePowerup();
    }

    public void StopSpawn()
    {
        dontSpawn = true;
        this.StopAllCoroutines();
    }

    public void SpawnRunePowerup()
    {
        if (!PhotonNetwork.inRoom)
        {
            if (!dontSpawn)
            {
                int Rand;
                Rand = Random.Range(0, Powerups.Length);
                GameObject Go;
                Go = Instantiate(Powerups[Rand], transform);
                Go.transform.localPosition = new Vector3(-4, 0, 0);
                spawnedPowerups.Add(Go);
                Rand = Random.Range(0, Powerups.Length);
                Go = Instantiate(Powerups[Rand], transform);
                Go.transform.localPosition = new Vector3(4, 0, 0);
                spawnedPowerups.Add(Go);
                Invoke("SpawnRunePowerup", 20);
                Invoke("ClearPowerUps", 15);
            }
        }
    }

    public void SpawnNetworkRunePowerup()
    {
        if (PhotonNetwork.inRoom && PhotonNetwork.isMasterClient)
        {
            if (!dontSpawn)
            {
                int Rand;
                Rand = Random.Range(0, Powerups.Length);
                GameObject Go1;
                GameObject Go2;
                Go1 = PhotonNetwork.Instantiate("Powerups/" + Powerups[Rand].name, new Vector3(1.5f, -8, 0), Quaternion.identity, new byte());
                spawnedPowerups.Add(Go1);
                Rand = Random.Range(0, Powerups.Length);
                Go2 = PhotonNetwork.Instantiate("Powerups/" + Powerups[Rand].name, new Vector3(14.5f, -8, 0), Quaternion.identity, new byte());
                spawnedPowerups.Add(Go2);
                Invoke("SpawnNetworkRunePowerup", 20);
                Invoke("NetworkClear", 15);
            }
        }
    }

    public void ClearPowerUps()
    {
        if (spawnedPowerups.Count < 1)
            return;


        foreach (var powerup in spawnedPowerups.ToList())
        {
            RemovePowerUp(powerup);
        }
    }

    public void RemovePowerUp(GameObject GO)
    {
        spawnedPowerups.Remove(GO);
        Destroy(GO);
    }

    public void RemovePowerUpNetwork(GameObject GO)
    {

        spawnedPowerups.Remove(GO);
        PhotonView.Get(GO).RPC("Destroy", PhotonTargets.MasterClient);
    }

    public void NetworkClear()
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        foreach (var powerup in spawnedPowerups.ToList())
        {
            if (spawnedPowerups.Count < 1)
                return;
            RemovePowerUpNetwork(powerup);
        }
    }

}
