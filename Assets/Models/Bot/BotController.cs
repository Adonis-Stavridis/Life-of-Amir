using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//script pour controller un bot
public class BotController : MonoBehaviour
{
    public Animator animator;

    //attributs du bot & détection du joueur
    public GameObject Character;
    public float lookRadius = 10f;
    private float distance;
    public float damage = 1f;
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;
    NavMeshAgent agent;

    //états d'animations
    public enum State
    {
        Idle,
        Attack
    }

    public State Current_state
    {
        get { return current_state; }
        set
        {
            current_state = value;
            switch (current_state)
            {
                case State.Idle:
                    animator.SetTrigger("idle");
                    break;

                case State.Attack:
                    animator.SetTrigger("attack");
                    break;
            }
        }
    }
    public State current_state = State.Idle;
    //autres
    private bool discovered = false;
    public ParticleSystem spawn;
    public AudioSource loop;
    public AudioSource sound;

    //récupère le navmeshagent et rend le bot très petit
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        transform.localScale = new Vector3(0, 0, 0);
    }

    //verifie si le joueur est dans la zone d'attaque
    //si oui, il apparait avec les bonnes animations & sons
    //puis il attaque après un intervalle de temps
    //en suivant le joueur
    void Update()
    {
        distance = Vector3.Distance(Character.transform.position, transform.position);
        if (discovered && transform.localScale != new Vector3(1, 1, 1))
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), 2 * Time.deltaTime);
        }
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
        switch (Current_state)
        {
            case State.Idle:
                if (distance <= lookRadius && Character.transform.GetComponent<Target>().health > 0)
                {
                    if (!loop.isPlaying) loop.Play();
                    agent.SetDestination(Character.transform.position);
                    if (distance <= agent.stoppingDistance && Character.transform.GetComponent<Target>().health > 0)
                    {
                        Current_state = State.Attack;
                    }
                    else
                    {
                        Current_state = State.Idle;
                    }
                    if (!discovered)
                    {
                        discovered = true;
                        Instantiate(spawn, transform.position, Quaternion.LookRotation(Vector3.up));
                    }
                }
                else
                {
                    if (loop.isPlaying) loop.Stop();
                    Current_state = State.Idle;
                }
                break;
            case State.Attack:
                if (distance >= agent.stoppingDistance)
                {
                    Current_state = State.Idle;
                }
                else
                {
                    agent.SetDestination(Character.transform.position);
                    FaceTarget();
                    if (attackCooldown <= 0f)
                    {
                        Attack_player();
                        attackCooldown = 1f / attackSpeed;                        
                    }
                    else
                    {
                        Current_state = State.Idle;
                    }
                }
                break;
        }
    }

    //tourne vers le joueur
    void FaceTarget()
    {
        Vector3 direction = (Character.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    //infliger des dégâts au joueur
    void Attack_player()
    {
        Target playerStats = Character.transform.GetComponent<Target>();
        if (playerStats.health > 0)
        {
            playerStats.TakeDamage(damage);
            sound.Play();
        }
        else
        {
            Current_state = State.Idle;
            loop.Stop();
        }
    }

    //visualiser la zone d'attaque dans la scène
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}