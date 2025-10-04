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
    // [SerializeField] float maxDistanceToPlayer;
    private float health;
    private Rigidbody2D enemyRb;
    private Vector3 playerPos;
    private float distanceToPlayer;
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
    // private bool hasLineOfSightWithPlayer;
    private float fireRate;
    private float timer;

    public void Start()
    {
        playerPos = player.transform.position;
        enemyRb = GetComponent<Rigidbody2D>();

        //or find player

        timer = 0;

        health = maxHealth;

        gameObject.GetComponent<AIPath>().maxSpeed = movementSpeed;
        gameObject.GetComponent<AIPath>().endReachedDistance = distanceToStop;
        path = GetComponent<AIPath>();
    }

    public void Update()
    {
        playerPos = player.transform.position;
        distanceToPlayer = Vector2.Distance(transform.position, playerPos);

        //Check for line of sight
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        RaycastHit2D sightRay = Physics2D.Raycast(transform.position, directionToPlayer, distance: range +2, layerMask: wallLayer);
        // if (sightRay.collider == null || sightRay.collider.gameObject == player)
        // {
        //     hasLineOfSightWithPlayer = true;
        // }
        // else
        // {
        //     hasLineOfSightWithPlayer = false;
        // }

        //if within range of player, disable movement in pathfinder
        if (distanceToPlayer <= distanceToStop && (sightRay.collider == null || sightRay.collider.gameObject == player)) //and can see the player
        {//should change to include shooting range
            gameObject.GetComponent<AIPath>().canMove = false;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;

            timer += Time.deltaTime;
            if (timer >= fireRate)
            {
                Attack();
                timer = 0;
                fireRate = UnityEngine.Random.Range(fireRateMin, fireRateMax);
            }
        }
        else if (!(sightRay.collider == null || sightRay.collider.gameObject == player)) //if can't see the player, continue moving
        {
            gameObject.GetComponent<AIPath>().canMove = true;
            enemyRb.constraints = RigidbodyConstraints2D.None;
            enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void Attack()
    {
        Debug.Log("shooting");
        //instantiate arrow
        GameObject firedArrow = Instantiate(arrow, position: shootPoint.position, rotation: transform.rotation);

        // move towards player
        firedArrow.GetComponent<ArrowBehavior>().playerPos = playerPos;
    }
    
    private void Die()
    {
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
