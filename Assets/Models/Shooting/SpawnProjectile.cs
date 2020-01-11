using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour faire apparaitre un missile
public class SpawnProjectile : MonoBehaviour
{
    public PointToDirection direction;

    public GameObject firePoint;
    public GameObject effect;

    private float timeToFire = 0;
    public AudioSource sound;

    //verifie si le joueur appuie sur la souris avec une limite de temps de tir
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            sound.Play();
            timeToFire = Time.time + 1 / effect.GetComponent<MoveProjectile>().fireRate;
            SpawnVFX();
        }
    }

    //fait apparaitre le missile
    void SpawnVFX()
    {
        GameObject vfx;

        if (firePoint != null)
        {
            vfx = Instantiate(effect, firePoint.transform.position, Quaternion.identity);
            if (direction != null)
            {
                vfx.transform.localRotation = direction.GetRotation();
            }
        }
        else
        {
            Debug.Log("No fire point");
        }
    }
}
