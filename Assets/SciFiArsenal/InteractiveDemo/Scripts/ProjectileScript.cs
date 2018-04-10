using System.Linq;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    public Snake target;
    public float speed, launchTime, damage = 300.0f;
    public bool isLaunched;
    public bool isFreeze;
    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (PhotonNetwork.inRoom)
            Launch();
        if (gameObject.GetPhotonView().photonView.instantiationData != null)
        {
            print(((float)gameObject.GetPhotonView().instantiationData[0]).ToString());
        }
    }

    public void Launch(Snake targetSnake)
    {
        target = targetSnake;
        isLaunched = true;
    }

    public void Launch()
    {
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

                //				foreach (GameObject trail in trailParticles)
                //				{
                //					GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                //					curTrail.transform.parent = null;
                //					Destroy(curTrail, 3f); 
                //				}
                //
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

        //transform.DetachChildren();
        impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
        //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

        if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
        {
            Destroy(hit.gameObject);
        }


        //yield WaitForSeconds (0.05);
        //        foreach (GameObject trail in trailParticles)
        //	    {
        //            GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
        //            curTrail.transform.parent = null;
        //            Destroy(curTrail, 3f); 
        //	    }
        Destroy(projectileParticle, 3f);
        Destroy(impactParticle, 5f);
        Destroy(gameObject);
        //projectileParticle.Stop();

    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.photonView.viewID == PhotonView.Get(this).viewID)
        {

        }
    }
}