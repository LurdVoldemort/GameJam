using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Pathfinding;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject spawner;
    // [SerializeField] float maxDistanceToPlayer;
    private float health;
    private Rigidbody2D enemyRb;
    private UnityEngine.Vector3 playerPos;
    public float distanceToPlayer;
    private AIPath path;


    [SerializeField] private int maxHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] public float enemyDamage;
    [SerializeField] private float range;
    [SerializeField] private float distanceToStop;
    [SerializeField] private float fireRateMax;
    [SerializeField] private float fireRateMin;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float swingTime;
    public bool hasLineOfSightWithPlayer;
    private float fireRate;
    private float timer;
    private Animator animator;
    [SerializeField] private GameObject swing;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        swing.SetActive(false);

        playerPos = player.transform.position;
        enemyRb = GetComponent<Rigidbody2D>();

        timer = 0;

        health = maxHealth;

        gameObject.GetComponent<AIPath>().maxSpeed = movementSpeed;
        gameObject.GetComponent<AIPath>().endReachedDistance = distanceToStop;
        path = GetComponent<AIPath>();

        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (player != null)
        {
            playerPos = player.transform.position;
            distanceToPlayer = UnityEngine.Vector2.Distance(transform.position, playerPos);

            //Check for line of sight
            UnityEngine.Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            RaycastHit2D sightRay = Physics2D.Raycast(transform.position, directionToPlayer, distance: distanceToPlayer, layerMask: wallLayer);


            if (sightRay.collider == null || sightRay.collider.gameObject == player)
            {
                hasLineOfSightWithPlayer = true;
            }
            else
            {
                hasLineOfSightWithPlayer = false;
            }

            if (distanceToPlayer <= distanceToStop && hasLineOfSightWithPlayer)
            {
                gameObject.GetComponent<AIPath>().canMove = false;
                enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            //if you have the shot, take the shot
            if (hasLineOfSightWithPlayer && distanceToPlayer <= range)
            {
                timer += Time.deltaTime;
                if (timer >= fireRate)
                {
                    Attack();
                    timer = 0;
                    fireRate = UnityEngine.Random.Range(fireRateMin, fireRateMax);
                }
            }
            if (!hasLineOfSightWithPlayer || distanceToPlayer >= distanceToStop) //if can't see the player, continue moving
            {
                gameObject.GetComponent<AIPath>().canMove = true;
                enemyRb.constraints = RigidbodyConstraints2D.None;
                enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            //send velocity info to animator
            animator.SetFloat("X Velocity", enemyRb.velocity.x);
            animator.SetFloat("Y Velocity", enemyRb.velocity.y);
        }
    }

    public void Attack()
    {
        //deal damage
        player.GetComponent<Player_Movement>().take_damage(enemyDamage);
        Debug.Log("melee attack deals " + enemyDamage + " damage");

        //find angle to rotate swing
        UnityEngine.Vector3 dir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        //do animation
        StartCoroutine(SwingRoutine(angle));
    }

    private IEnumerator SwingRoutine(float angle)
    {
        swing.SetActive(true);
        swing.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, angle + 90);
        // Wait for n seconds
        yield return new WaitForSeconds(swingTime);

        swing.SetActive(false);
    }
    
    private void Die()
    {
        GameManager.Instance.enemiesInPlay -= 1;
        Debug.Log("Enemy Killed");
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
}
