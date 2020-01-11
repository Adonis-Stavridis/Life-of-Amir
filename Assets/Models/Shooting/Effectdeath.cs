using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script qui tue l'objet après time secondes
//utile pour les particules surtout
public class Effectdeath : MonoBehaviour
{
    public float time;

    void Start()
    {
        Destroy(gameObject, time);
    }
}
