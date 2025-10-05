using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
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
    [SerializeField] private LayerMask wallLayer;
    public bool hasLineOfSightWithPlayer;
    private float fireRate;
    private float timer;

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
        }
    }

    public void Attack()
    {
        Debug.Log("Enemy Melee Attack");
    }
    
    private void Die()
    {
        GameManager.Instance.enemiesInPlay -= 1;
        Debug.Log("Enemy Killed");
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Took damage");
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
}
