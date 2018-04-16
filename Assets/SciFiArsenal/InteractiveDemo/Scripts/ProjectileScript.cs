using System.Linq;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal;

    public Snake target;
    public float speed, launchTime;
    public int baseDamage;
    public int multiplier;
    public int level;
    public int damage;
    public bool isLaunched;
    public bool isFreeze;
    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (PhotonNetwork.inRoom)
        {
            level = (int)gameObject.GetPhotonView().instantiationData[0];
            Launch();
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = true;
#endif
    }

    public void Launch(Snake targetSnake)
    {
        damage = baseDamage + (level * multiplier);
        target = targetSnake;
        isLaunched = true;
    }

    public void Launch()
    {
        damage = baseDamage + (level * multiplier);
        if (gameObject.GetComponent<PhotonView>().isMine)
        {
            PlayerInfo enemy = GameObject.FindObjectsOfType<PlayerInfo>().Where(a => !a.GetComponent<PhotonView>().isMine).First();
            target = enemy.Player;
        }
        else
        {
            PlayerInfo enemy = GameObject.FindObjectsOfType<PlayerInfo>().Where(a => a.GetComponent<PhotonView>().isMine).First();
            target = enemy.Player;
        }
        isLaunched = true;
    }


    void Update()
    {
        if (isLaunched)
        {
            if (!target)
            {
                if (!PhotonNetwork.inRoom)
                    Destroy(gameObject);
                else
                    PhotonNetwork.Destroy(gameObject);
                return;
            }
            transform.LookAt(target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(target.transform.position, transform.position) < 0.5f)
            {
                isLaunched = false;
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
                target.TakeDamage(damage);
                if (isFreeze)
                    target.EnableFreezeHit();

                Destroy(projectileParticle, 3f);
                Destroy(impactParticle, 5f);
                Destroy(gameObject);

            }

        }
    }

    void OnCollisionEnter(Collision hit)
    {
        impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
        if (hit.gameObject.tag == "Destructible")
        {
            Destroy(hit.gameObject);
        }
        Destroy(projectileParticle, 3f);
        Destroy(impactParticle, 5f);
        Destroy(gameObject);
    }
}