using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script qui permet de recevoir des dégâts et gére la vie des objets
public class Target : MonoBehaviour
{
    public float startHealth;
    public float health;
    public ParticleSystem particles;
    public Image HealthBarFill;
    public EndMenu endGame;
    public GameObject Boss;
    public GameObject bossPart;
    public Collider capsCollider;
    public GameObject canvas;
    public DamageBoss radius;
    public GameObject character;

    //initialise la vie
    void Start()
    {
        health = startHealth;
    }

    //prend des dégâts et peut mourir aussi
    public void TakeDamage(float amount)
    {
        health -= amount;
        HealthBarFill.fillAmount = health / startHealth;
        if (health <= 0f)
        {
            Die();
        }
    }

    //regagne de la vie
    public void heal(float amount)
    {
        health += amount;
        if (health > startHealth) health = startHealth;
        HealthBarFill.fillAmount = health / startHealth;
    }

    //mourir
    public void Die()
    {
        //joueur
        if (gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            endGame.win = false;
            endGame.endGame();
        }
        //boss (partie du boss)
        else if (gameObject.tag == "Boss")
        {
            character.GetComponent<Target>().heal(5f);
            if (particles != null) Instantiate(particles, capsCollider.bounds.center, Quaternion.LookRotation(Vector3.up));
            if (bossPart != null) Destroy(bossPart);
            capsCollider.enabled = false;
            radius.enabled = false;
            if (canvas != null) Destroy(canvas);
            if (Boss != null) Boss.GetComponent<BossParts>().remove();
        }
        //bot
        else
        {
            character.GetComponent<Target>().heal(1f);
            if (particles != null) Instantiate(particles, transform.position, Quaternion.LookRotation(Vector3.up));
            Destroy(gameObject);
        }
    }
}
