using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script de gestion de la caméra à la 3e personne
public class ThirdPersonCamera : MonoBehaviour
{
    //attributs de la caméra
    private float yaw;
    private float pitch;
    public bool lockCursor;
    public float mouseSensitivity = 1f;
    public Transform target;
    public float distanceFromTarget = 2;
    //fluidité
    public float rotationSmoothTime = 0.25f;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    //bloque le curseur et le rend invisible
    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //fait bouger la caméra avec quelques restrictions (clamp)
    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
