using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour afficher des informations sur le jeu
public class Info : MonoBehaviour
{
    public GameObject Character;
    public float lookRadius = 2f;
    private float distance;
    public GameObject info;

    //si le joueur se rapproche de l'objet (le panneau) ça affiche un sprite
    void Update()
    {
        distance = Vector3.Distance(Character.transform.position, transform.position);

        if (distance <= lookRadius)
        {
            info.SetActive(true);
        }
        else
        {
            if (info.activeSelf) info.SetActive(false);
        }
    }
}
