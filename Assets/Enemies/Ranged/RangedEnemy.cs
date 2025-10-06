using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject spawner;
    // [SerializeField] float maxDistanceToPlayer;
    private float health;
    private Rigidbody2D enemyRb;
    private Vector3 playerPos;
    public float distanceToPlayer;
    private AIPath path;


    [SerializeField] private int maxHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] public float enemyDamage;
    [SerializeField] private float range;
    [SerializeField] private float distanceToStop;
    [SerializeField] private float fireRateMax;
    [SerializeField] private float fireRateMin;
    [SerializeField] public float arrowSpeed;
    [SerializeField] private GameObject arrow;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform shootPoint;
    public bool hasLineOfSightWithPlayer;
    private float fireRate;
    private float timer;
    private Animator animator;

    private Vector3 currentVelocity;
    private Vector3 previousPosition;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerPos = player.transform.position;
        enemyRb = GetComponent<Rigidbody2D>();

        timer = 0;

        health = maxHealth;

        gameObject.GetComponent<AIPath>().maxSpeed = movementSpeed;
        gameObject.GetComponent<AIPath>().endReachedDistance = distanceToStop;
        path = GetComponent<AIPath>();

        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    public void Update()
    {
        if (player != null)
        {

            playerPos = player.transform.position;
            distanceToPlayer = Vector2.Distance(transform.position, playerPos);

            //Check for line of sight
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
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
            if (hasLineOfSightWithPlayer)
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

            //calculate velocity
            Vector3 positionDelta = transform.position - previousPosition;
            currentVelocity = positionDelta / Time.deltaTime;
            previousPosition = transform.position;

            //send velocity info to animator
            animator.SetFloat("X Velocity", currentVelocity.x);
            animator.SetFloat("Y Velocity", currentVelocity.y);
            animator.SetBool("Velocity", currentVelocity.magnitude > 0);
        }
    }

    public void Attack()
    {
        //find angle to rotate swing
        Vector3 dir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //instantiate arrow
        GameObject firedArrow = Instantiate(arrow, position: shootPoint.position, rotation: Quaternion.Euler(0, 0, angle - 90));

        // move towards player
        firedArrow.GetComponent<ArrowBehavior>().playerPos = playerPos;
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
