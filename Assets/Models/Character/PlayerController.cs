using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour controller le personnage du joueur
public class PlayerController : MonoBehaviour
{
    //attributs de vitesse
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float gravity = -12;
    private bool grounded = false;
    //fluidité des mouvements
    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;
    public float speedSmoothTime = 0.1f;
    private float speedSmoothVelocity;
    private float currentSpeed;
    private float velocityY;
    private float oldTargetRotation = 0f;
    //autres
    public Animator animator;
    public Camera cam;
    public CharacterController controller;
    public AudioSource footsteps;

    //regarde le input et fait bouger le personnage en fonction de celui-ci
    void Update()
    {
        float targetRotation, targetSpeed, animationSpeedPercentage;
        Vector2 input, inputDir;
        Vector3 velocity;
        bool running;
        bool lookAround;

        //récupére le input
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;
        running = Input.GetKey(KeyCode.LeftShift);
        lookAround = Input.GetKey(KeyCode.C);

        //calcule les animations et la vitesse
        targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        animationSpeedPercentage = ((running) ? 2 : 1) * inputDir.magnitude;

        //si le joueur n'appuis pas sur la touche C qui désactive le mouvement
        if (!lookAround)
        {
            //calcule de la direction du personnage
            targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            if ((inputDir.magnitude == 0) && (Mathf.Abs(targetRotation - oldTargetRotation) >= 0.5f))
            {
                animationSpeedPercentage = 0.5f;
            }
            oldTargetRotation = targetRotation;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        //si le personnage ne touche plus le sol il ne peut pas bouger
        velocityY += Time.deltaTime * gravity;
        if (grounded)
        {
            velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
            animator.SetFloat("speedPercentage", animationSpeedPercentage, speedSmoothTime, Time.deltaTime);
            if (inputDir.magnitude != 0)
            {
                if (!footsteps.isPlaying) footsteps.Play();
            }
            else
            {
                footsteps.Stop();
            }
        }
        else
        {
            velocity = Vector3.up * velocityY;
            animator.SetFloat("speedPercentage", 0, speedSmoothTime, Time.deltaTime);
        }
        
        //fair bouger le personnage
        controller.Move(velocity * Time.deltaTime);

        //le personnage touche le sol
        if (controller.isGrounded)
        {
            velocityY = 0;
            grounded = true;
        }
    }
}
