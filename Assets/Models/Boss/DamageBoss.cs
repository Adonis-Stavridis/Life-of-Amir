using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script pour que le boss fasse des dégâts
public class DamageBoss : MonoBehaviour
{
    public GameObject Character;
    public float damage = 2f;
    public float attackCooldown = 0f;
    public float DamageRadius = 2f;
    private float distance;

    //active ou desactive les degats (pour eviter de tuer le joueur trop rapidement)
    void Update()
    {
        if(attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
        else
        {
            DetectDamage();
        }
    }

    //fait des dégâts au joueur s'il est dans la zone
    public void DetectDamage()
    {
        distance = Vector3.Distance(Character.transform.position, transform.position);
        if(distance <= DamageRadius)
        {
            Target playerStats = Character.GetComponent<Target>();
            if (playerStats != null)
            {
                if (attackCooldown <= 0f)
                {
                    playerStats.TakeDamage(damage);
                    attackCooldown = 2f;
                }
            }
        }

    }

    //visualiser la zone de dégâts dans la scène
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DamageRadius);
    }
}
