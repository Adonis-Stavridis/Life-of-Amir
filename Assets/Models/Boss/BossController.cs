using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//script pour controller le Boss
public class BossController : MonoBehaviour
{
    //attributs du boss & détection du joueur
    public Animator animator;
    public GameObject Character;
    public float lookRadius = 20f;
    private float distance;
    public float attackSpeed = 0.5f;
    private float attackCooldown = 0f;
    private int compteur = 0;
    private bool scale = false;
    private bool ready = false;
    private bool turn = false;
    private Vector3 initScale;
    //la cible
    private Transform target;
    //projectile
    public GameObject firePoint;
    public GameObject effect;
    //sons
    public AudioSource disable;
    public AudioSource music;
    public AudioSource enter;
    public AudioSource range;
    public AudioSource close;

    //états d'animations
    public enum State
    {
        Enter,
        Idle,
        Attack,
        RangeAttack
    }

    public State current_state = State.Idle;

    public State Current_state
    {
        get { return current_state; }
        set
        {
            current_state = value;
            switch (current_state)
            {
                case State.Enter:
                    if (!enter.isPlaying) enter.Play();
                    animator.SetTrigger("enter");
                    break;
                case State.Idle:
                    if (!turn) StartCoroutine("bTurn");
                    animator.SetTrigger("idle");
                    break;

                case State.Attack:
                    turn = false;
                    animator.SetTrigger("attack");
                    break;

                case State.RangeAttack:
                    turn = false;
                    animator.SetTrigger("range_attack");
                    break;
            }
        }
    }

    void Start()
    {
        target = Character.transform;
        initScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
    }

    //verifie si le joueur est dans la zone d'attaque
    //si oui, il apparait avec les bonnes animations & sons
    //puis il attaque après un intervalle de temps
    // -> 3 attaques de meteorites et 1 morsure
    void Update()
    {
        distance = Vector3.Distance(target.position, transform.position);
        Target playerStats = target.GetComponent<Target>();
        if (scale && transform.localScale != initScale) transform.localScale = Vector3.Lerp(transform.localScale, initScale, Time.deltaTime);

        if (distance <= lookRadius && !ready)
        {
            if (disable.isPlaying) disable.Stop();
            if (!music.isPlaying) music.Play();
            Current_state = State.Enter;
            StartCoroutine("bScale");
            Current_state = State.Idle;
            if (!ready)
            {
                StartCoroutine("bReady");
            }
        }

        if (ready)
        {
            switch (current_state)
            {
                case State.Idle:
                    if (distance <= lookRadius)
                    {
                        attackCooldown -= Time.deltaTime;
                        if (turn) FaceTarget();
                        if (attackCooldown <= -3f)
                        {
                            if (target != null)
                            {
                                if (playerStats.health > 0)
                                {
                                    if (compteur == 3)
                                    {
                                        Current_state = State.Attack;
                                        attackCooldown = 1f / attackSpeed;
                                        compteur = 0;
                                        close.Play();
                                    }
                                    else
                                    {
                                        Current_state = State.RangeAttack;
                                        StartCoroutine("SpawnVFX");
                                        attackCooldown = 1f / attackSpeed;
                                        compteur++;
                                        range.PlayDelayed(0.5f);
                                    }
                                }
                            }
                        }
                    }
                    break;
                case State.Attack:
                    Current_state = State.Idle;
                    break;
                case State.RangeAttack:
                    Current_state = State.Idle;
                    break;
            }
        }
    }

    //tourne vers le joueur
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    //fait apparaitre une meteorite après 1 secondes
    IEnumerator SpawnVFX()
    {
        yield return new WaitForSeconds(1f);
        GameObject vfx;

        if (firePoint != null)
        {
            vfx = Instantiate(effect, firePoint.transform.position, Quaternion.identity);
            vfx.transform.localRotation = firePoint.transform.rotation;
        }
        else
        {
            Debug.Log("No fire point");
        }
    }

    //indique que le boss peut tourner après 2 secondes
    IEnumerator bTurn()
    {
        yield return new WaitForSeconds(2f);
        turn = true;
    }

    //indique que le boss est prêt à attaquer
    IEnumerator bReady()
    {
        yield return new WaitForSeconds(5f);
        ready = true;
        turn = true;
    }

    //indique que le boss grandit (il est scale à zero au départ)
    //quand le joueur rentre dans sa zone pour la premiere fois, il grandit
    IEnumerator bScale()
    {
        yield return new WaitForSeconds(0.75f);
        scale = true;
    }

    //visualiser la zone d'attaque dans la scène
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}