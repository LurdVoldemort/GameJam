using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

//TEMPLATE CLASS FOR ENEMY TYPES

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] public GameObject player;
    // [SerializeField] float maxDistanceToPlayer;
    private float health;
    private Rigidbody2D enemyRb;
    private Vector3 playerPos;
    private float distanceToPlayer;
    private float speed;
    private AIPath path;


    [SerializeField] private int maxHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float distanceToStop;
    [SerializeField] private float fireRateMax;
    [SerializeField] private float fireRateMin;
    private float fireRate;
    private float timer;

    EnemyBehaviour enemyBehaviour;

    public void Start()
    {
        playerPos = player.transform.position;
        enemyRb = GetComponent<Rigidbody2D>();

        timer = 0;

        health = maxHealth;

        gameObject.GetComponent<AIPath>().maxSpeed = speed;
        path = GetComponent<AIPath>();
    }

    public void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerPos);

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
