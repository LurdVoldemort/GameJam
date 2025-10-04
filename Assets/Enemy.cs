using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Pathfinding;
// using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] GameObject player;
    // [SerializeField] float enemyMoveSpeed;
    // [SerializeField] float maxDistanceToPlayer;
    [SerializeField] float maxHealth;
    float health;
    Rigidbody2D enemyRb;
    Vector3 playerPos;
    float distanceToPlayer;

    private AIPath path;


    // Start is called before the first frame update
    void Start()
    {
        playerPos = player.transform.position;
        enemyRb = GetComponent<Rigidbody2D>();
        health = maxHealth;

        path = GetComponent<AIPath>();

        //set player to dummy player game object

        // agent = GetComponent<NavMeshAgent>();
        // agent.updateRotation = false;
        // agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // if (distanceToPlayer > maxDistanceToPlayer)
        // {
        //     //transform.position = Vector2.MoveTowards(transform.position, playerPos, enemyMoveSpeed * Time.deltaTime);
        // }
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
