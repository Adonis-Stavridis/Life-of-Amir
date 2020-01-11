using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script de l'explosion d'une meteorite
public class Explosion : MonoBehaviour
{
    private GameObject character;
    public float radius = 1f;
    private float distance;
    public float damage = 1f;
    public float attackCooldown = 0f;

    //prend le joueur de la scène
    void Start()
    {
        character = GameObject.Find("Character");
    }

    //verifie si le joueur est dans la zone de dégâts de l'explosion
    //si oui, il lui inflige des dégâts
    void Update()
    {
        distance = Vector3.Distance(character.transform.position, transform.position);
        StartCoroutine("disable");

        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (distance <= radius)
        {
            if (attackCooldown <= 0f)
            {
                Target playerStats = character.GetComponent<Target>();
                playerStats.TakeDamage(damage);
                attackCooldown = 2f;
            }
        }
    }

    //désactive ce script après 1.5 secondes
    //l'objet reste vivant pour 3 secondes
    IEnumerator disable()
    {
        yield return new WaitForSeconds(1.5f);
        this.enabled = false;
    }
}
