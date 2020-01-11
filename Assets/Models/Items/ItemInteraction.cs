using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour intéragir avec les items
public class ItemInteraction : MonoBehaviour
{
    public GameObject Character;
    public float lookRadius = 2f;
    private float distance;
    public GameObject pickup;
    public GameObject door;
    private bool opened = false;
    public GameObject rules;

    //permet d'afficher le bouton E pour indiquer que le joueur peut récuperer l'item, s'il est proche de celui-ci
    // avec gestion de l'interface
    void Update()
    {
        distance = Vector3.Distance(Character.transform.position, transform.position);

        if (distance <= lookRadius && !opened && ((door && Character.GetComponent<Items>().itemNumber == 0) || !door))
        {
            //affiche le bouton E
            pickup.SetActive(true);
            if (door && rules.activeSelf) rules.SetActive(false);

            //si le joueur appuie sur E
            if (Input.GetKey(KeyCode.E))
            {
                //si c'est la porte elle s'ouvre
                if (door != null)
                {
                    door.GetComponent<doorAnimator>().open();
                    opened = true;
                    StartCoroutine("pickupActivate");
                }
                //ajoute l'item dans l'inventaire du joueur
                else
                {
                    Character.GetComponent<Items>().addItem();
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            //gestion du texte sur la porte
            if (door && Character.GetComponent<Items>().itemNumber > 0 && !rules.activeSelf) rules.SetActive(true);
            if (door && Character.GetComponent<Items>().itemNumber == 0 && rules.activeSelf) rules.SetActive(false);
            if (pickup.activeSelf) pickup.SetActive(false);
        }
    }

    //peut reafficher le bouton E pour la porte, quand elle est fermée de nouveau
    IEnumerator pickupActivate()
    {
        yield return new WaitForSeconds(6f);
        opened = false;
    }
}
