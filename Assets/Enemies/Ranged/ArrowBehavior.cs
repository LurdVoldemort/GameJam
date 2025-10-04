using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArrowBehavior : MonoBehaviour
{
    // [SerializeField] public Player_Movement playerMovement;
    [SerializeField] public RangedEnemy rangedEnemy;
    [SerializeField] private float ArrowMaxTime = 2f;
    private float arrowAirTime;
    public Vector2 playerPos;


    void Start()
    {
        Vector2 arrowPos = new Vector2(transform.position.x, transform.position.y);
        //give velocity in direction of player
        Vector2 directionToPlayer = (playerPos - arrowPos).normalized;
        gameObject.GetComponent<Rigidbody2D>().AddForce(directionToPlayer * rangedEnemy.arrowSpeed, ForceMode2D.Impulse);
        arrowAirTime = 0;
    }

    void Update()
    {
        arrowAirTime += Time.deltaTime;
        if (arrowAirTime >= ArrowMaxTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //playerMovement.take_damage(rangedEnemy.enemyDamage);
        }
        //play hit sound effect
        Destroy(gameObject);
    }
}
