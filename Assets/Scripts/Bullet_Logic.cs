using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Logic : MonoBehaviour
{
    public RangedEnemy rangedEnemy;
    public MeleeEnemy meleeEnemy;
    public Player_Movement player_Movement;
    public float lifetime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.CompareTag("RangedEnemy"))
        {
            collision.collider.GetComponent<RangedEnemy>().TakeDamage(player_Movement.damage);
        }
        if (collision.collider.CompareTag("MeleeEnemy"))
        {
            collision.collider.GetComponent<MeleeEnemy>().TakeDamage(player_Movement.damage);
        }
        if (collision.collider.CompareTag("Spawner"))
        {
            collision.collider.GetComponent<Spawner>().TakeDamage(player_Movement.damage);
        }

        Destroy(gameObject, 0.1f);
    }
}
