using System.Collections;
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
    public bool debug;

    void Awake()
    {
        instance = this;
    }

    public void StartSpawn()
    {
        return;
        ClearPowerUps();
        if (debug)
        {
            StartCoroutine(Counter());
        }
        StartCoroutine(StartSpawnTimer(5));
    }

    IEnumerator Counter()
    {
        int i = 0;
        while (true)
        {
            i++;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator StartSpawnTimer(float time)
    {
        yield return new WaitForSeconds(time);
        dontSpawn = false;
        if (!PhotonNetwork.inRoom)
            StartCoroutine(SpawnRunePowerup());
        else
            StartCoroutine(SpawnNetworkRunePowerup());
    }

    public void StopSpawn()
    {
        dontSpawn = true;
        this.StopAllCoroutines();
    }

    public IEnumerator SpawnRunePowerup()
    {
        yield break;
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
                yield return new WaitForSeconds(15);
                SoftClear();
                StartCoroutine(StartSpawnTimer(5));
            }
        }
    }

    public IEnumerator SpawnNetworkRunePowerup()
    {
        yield break;
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
                yield return new WaitForSeconds(15);
                SoftClear();
                StartCoroutine(StartSpawnTimer(5));
            }
        }
    }

    public void SoftClear()
    {
        if (spawnedPowerups.Count < 1)
            return;
        foreach (var powerup in spawnedPowerups.ToList())
        {
            if (PhotonNetwork.inRoom && PhotonNetwork.isMasterClient)
                RemovePowerUpNetwork(powerup);
            else
                RemovePowerUp(powerup);
        }
    }

    public void ClearPowerUps()
    {
        if (debug)
        {
            StopCoroutine(Counter());
        }
        StopSpawn();
        SoftClear();
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
}
