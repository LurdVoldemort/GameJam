using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
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
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attackRateMax;
    [SerializeField] private float attackRateMin;
    [SerializeField] private LayerMask wallLayer;
    private float attackRate;
    private float timer;

    EnemyBehaviour enemyBehaviour;

    public void Start()
    {
        playerPos = player.transform.position;
        enemyRb = GetComponent<Rigidbody2D>();

        timer = 0;

        health = maxHealth;

        gameObject.GetComponent<AIPath>().maxSpeed = movementSpeed;
        path = GetComponent<AIPath>();
    }

    public void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerPos);

        //Check for line of sight
        Vector2 directionToPlayer = (transform.position - player.transform.position).normalized;
        RaycastHit2D sightRay = Physics2D.Raycast(transform.position, directionToPlayer, wallLayer);
        //(sightRay.collider == null || sightRay.collider.gameObject == player)
    }

    public void Attack()
    {
        Debug.Log("Shoot!");
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
