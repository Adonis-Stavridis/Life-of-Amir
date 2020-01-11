using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//script pour afficher les fps ou ips (images par seconde)
public class FPS : MonoBehaviour
{
    public TextMeshProUGUI fps;

    //indique de répeter cette fonction toutes les 1 secondes
    void Start()
    {
        InvokeRepeating("updtFPS", 0, 1f);
    }

    //met à jour le nombre de fps
    void updtFPS()
    {
        fps.text = "FPS: " + Mathf.RoundToInt(1 / Time.deltaTime);
    }
}
