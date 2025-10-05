using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] public bool spawnerInPlay;
    [SerializeField] private float spawnTimerMax;
    [SerializeField] private float spawnTimerMin;
    [SerializeField] private float chanceToSpawn;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> enemyTypes = new List<GameObject>();
    [SerializeField] private float maxEnemiesInPlay;
    public float enemiesInPlay;

    private float timeUntilSpawn;
    private float spawnTimer;
    private float health;


    // Start is called before the first frame update
    void Start()
    {
        timeUntilSpawn = 0;
        spawnTimer = spawnTimerMax;
        spawnPos = GetComponentInChildren<Transform>();
        enemiesInPlay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //every random few seconds spawn enemy
        timeUntilSpawn += Time.deltaTime;
        if (timeUntilSpawn >= spawnTimer && enemiesInPlay < maxEnemiesInPlay)
        {
            SpawnEnemy(spawnPos);
            timeUntilSpawn = 0;
            enemiesInPlay += 1;
            spawnTimer = Random.Range(spawnTimerMin, spawnTimerMax);
        }
    }

    public void SpawnEnemy(Transform spawnPos)
    {
        GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Count)];
        GameObject newEnemy = Instantiate(enemyType);
        newEnemy.transform.position = spawnPos.position;
        //give player object to new enemy
        newEnemy.GetComponent<AIDestinationSetter>().target = player.transform;

        if (newEnemy.CompareTag("MeleeEnemy")) { newEnemy.GetComponent<MeleeEnemy>().player = player; }
        else if (newEnemy.CompareTag("RangedEnemy")) { newEnemy.GetComponent<RangedEnemy>().player = player; }
        //play sound effect and do animation
    }

    private void Die()
    {
        Debug.Log("Spawner Destroyed");
        //contact level manager and tell it you died
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (spawnerInPlay)
        {
            Debug.Log("Took damage");
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }
}
