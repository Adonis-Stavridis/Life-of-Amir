using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script qui permet à l'objet de pointer vers la caméra
//utile pour l'affichage de sprites
public class RotateToCamera : MonoBehaviour
{
    public Camera cam;

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-cam.transform.forward, cam.transform.up);
    }
}
