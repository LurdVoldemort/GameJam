using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : MonoBehaviour, IUniqueBehavior
{
    [SerializeField] private float range;
    [SerializeField] private float distanceToStop;
    [SerializeField] private float fireRateMax;
    [SerializeField] private float fireRateMin;
    private float fireRate;
    private float timer;

    EnemyBehaviour enemyBehaviour;

    public void Start()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        timer = 0;
    }

    public void Update()
    {
        //if within range of player, disable movement in pathfinder
        if (enemyBehaviour.distanceToPlayer <= distanceToStop)
        {
            enemyBehaviour.gameObject.GetComponent<AIPath>().canMove = false;

            timer += Time.deltaTime;
            if (timer >= fireRate)
            {
                Attack();
                timer = 0;
                fireRate = Random.Range(fireRateMin, fireRateMax);
            }
        } 
    }

    public void Attack()
    {
        Debug.Log("Shoot!");
    }
}
