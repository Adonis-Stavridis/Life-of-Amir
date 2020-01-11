using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour gérer l'intéraction avec la porte
public class doorAnimator : MonoBehaviour
{
    //états d'animations
    public enum State
    {
        still,
        opening,
        closing
    }
    public State current_state = State.still;
    public AudioSource sound;

    private Vector3 init;
    private Vector3 up;

    //initialise la bonne position de la porte
    void Start()
    {
        init = transform.position;
        up = transform.position + new Vector3(0, 5, 0);
    }

    //si le joueur est proche de la porte, ca affiche le bouton E
    //s'il appuie sur le bouton, la porte s'ouvre, puis referme après 5 secondes
    void Update()
    {
        switch(current_state)
        {
            case State.opening:
                transform.position = Vector3.Lerp(transform.position, up, 2 * Time.deltaTime);
                if (transform.position == up)
                {
                    current_state = State.still;
                }
                break;
            case State.closing:
                transform.position = Vector3.Lerp(transform.position, init, 2 * Time.deltaTime);
                if (transform.position == init)
                {
                    current_state = State.still;
                }
                break;
        }
    }

    //fait bouger la porte vers le haut
    public void open()
    {
        sound.Play();
        current_state = State.opening;
        StartCoroutine("close");
    }

    //fait redescendre la porte à sa position initiale
    IEnumerator close()
    {
        yield return new WaitForSeconds(5f);
        current_state = State.closing;
    }
}
