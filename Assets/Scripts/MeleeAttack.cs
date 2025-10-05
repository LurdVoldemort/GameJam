using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Player_Movement player_Movement;
    void Start()
    {
        player_Movement = FindObjectOfType<Player_Movement>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log(other.name);
        if (other.CompareTag("RangedEnemy"))
        {
            other.GetComponent<RangedEnemy>().TakeDamage(player_Movement.damage);
        }
        if (other.CompareTag("MeleeEnemy"))
        {
            other.GetComponent<MeleeEnemy>().TakeDamage(player_Movement.damage);
        }
        if (other.CompareTag("Spawner"))
        {
            other.GetComponent<Spawner>().TakeDamage(player_Movement.damage);
        }
    }
}
