using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour faire bouger les missiles & méteorites
//vérifie les collisions aussi
public class MoveProjectile : MonoBehaviour
{
    //attributs du missile
    public float speed = 25f;
    public float damage = 1f;
    public float fireRate = 2f;
    public float force = 100f;
    //autres
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public bool isFireball = false;

    //fait apparaitre un effet de muzzle s'il existe
    void Start()
    {
        GameObject muzzleVFX;
        ParticleSystem psMuzzle, psChild;

        if (muzzlePrefab != null)
        {
            muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;

            psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null)
            {
                Destroy(muzzleVFX, psMuzzle.main.duration);
            }
            else
            {
                psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    //fait bouger l'objet dans la bonne direction
    //il le détruit après 5 secondes s'il ne touche rien
    void Update()
    {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        Destroy(gameObject, 5);
    }

    //agir en fonction de la collision
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject obj = collision.gameObject;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        //les missiles du joueur peuvent faire bouger des objets dans le jeu
        if (!isFireball && obj.tag == "Object")
        {
            if (obj.GetComponent<Rigidbody>() != null)
            {
                obj.GetComponent<Rigidbody>().AddForce(-contact.normal * force);
            }
        }

        //le missile du joueur fait des dégâts aux bots et au boss
        if (!isFireball && (obj.tag == "Bot" || obj.tag == "Boss"))
        {
            Target target;

            target = obj.GetComponent<Target>();
            if (target != null)
            {
                if (target.enabled) target.TakeDamage(damage);
            }
        }

        //la méteorite du boss fait des dégâts au joueur
        if (isFireball && obj.tag == "Player")
        {
            Target target;

            target = obj.GetComponent<Target>();
            if (target != null)
            {
                if (target.enabled) target.TakeDamage(damage);
            }
        }

        //le boss ne se fait pas de dégâts à lui-même
        if (!(isFireball && obj.tag == "Boss"))
        {
            //fait apparaitre un effect lors de la collision
            //puis détruit le missile
            if (hitPrefab != null)
            {
                GameObject hitVFX;
                ParticleSystem psHit, psChild;

                hitVFX = Instantiate(hitPrefab, pos, rot);
                psHit = hitVFX.GetComponent<ParticleSystem>();
                if (psHit != null)
                {
                    Destroy(hitVFX, psHit.main.duration);
                }
                else
                {
                    psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
            }

            Destroy(gameObject);
        }
    }
}
