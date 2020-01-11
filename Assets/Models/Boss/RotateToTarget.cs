using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour tirer sur le joueur
public class RotateToTarget : MonoBehaviour
{
    public Transform target;

    //redirige la rotation vers le joueur
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
