using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//script pour gérer les items
public class Items : MonoBehaviour
{
    public int itemNumber = 4;
    public GameObject key;
    public AudioSource sound;
    public GameObject uiCcs;
    public TextMeshProUGUI text;
    public GameObject uiKey;

    //le joueur récupère un item
    public void addItem()
    {
        if (itemNumber > 0)
        {
            itemNumber--;
            sound.Play();
            if (!uiCcs.activeSelf) uiCcs.SetActive(true);
            if (itemNumber > 0) text.text = 4 - itemNumber + "";

            //le joueur a passé 3 examens
            if (itemNumber == 1 && key != null)
            {
                key.SetActive(true);
            }

            //le joueur a pris la clé
            if (itemNumber == 0)
            {
                uiCcs.SetActive(false);
                uiKey.SetActive(true);
            }
        }
    }
}
