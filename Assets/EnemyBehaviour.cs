using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Pathfinding;
// using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public EnemyTemplate enemyType;
    [SerializeField] public GameObject player;
    // [SerializeField] float maxDistanceToPlayer;
    private float health;
    private Rigidbody2D enemyRb;
    private Vector3 playerPos;
    public float distanceToPlayer;
    private float speed;
    private AIPath path;


    // Start is called before the first frame update
    void Start()
    {
        playerPos = player.transform.position;
        enemyRb = GetComponent<Rigidbody2D>();

        health = enemyType.maxHealth;
        speed = enemyType.movementSpeed;
        gameObject.name = enemyType.enemyName;

        gameObject.GetComponent<AIPath>().maxSpeed = speed;
        path = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Call attack with enemyType.uniqueBehavior.Attack()
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
