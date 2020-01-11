using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script qui permet au joueur de viser la où la caméra pointe
public class PointToDirection : MonoBehaviour
{
    public Camera cam;
    public float maxLength = 100;
    private Vector3 pos;
    private Vector3 dir;
    private Quaternion rot;

    //met à jour la rotation de l'objet pour pointer au même endroit que la caméra
    //avec un raycast, qui ignore des layers aussi
    void Update()
    {
        RaycastHit hit;
        int layerMask = 1 << 8;
        bool lookAround;

        layerMask = ~layerMask;
        lookAround = Input.GetKey(KeyCode.C);

        if (cam != null)
        {
            if (!lookAround)
            {
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxLength, layerMask))
                {
                    RotateToDirection(gameObject, hit.point);
                }
            }
        }
        else
        {
            Debug.Log("No camera");
        }
    }

    //mettre la bonne direction à l'objet
    void RotateToDirection(GameObject obj, Vector3 dest)
    {
        dir = dest - obj.transform.position;
        rot = Quaternion.LookRotation(dir);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rot, 1);
    }

    //permet de mettre la bonne direction au missile quand il apparait
    public Quaternion GetRotation()
    {
        return rot;
    }
}
