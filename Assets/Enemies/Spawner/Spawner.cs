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
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> enemyTypes = new List<GameObject>();

    private float timeUntilSpawn;
    private float spawnTimer;
    private float health;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        timeUntilSpawn = 0;
        spawnTimer = spawnTimerMax;
        spawnPos = transform.Find("SpawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        //every random few seconds spawn enemy
        timeUntilSpawn += Time.deltaTime;
        if (timeUntilSpawn >= spawnTimer && GameManager.Instance.enemiesInPlay < GameManager.Instance.maxEnemiesInPlay)
        {
            SpawnEnemy(spawnPos);
            timeUntilSpawn = 0;
            GameManager.Instance.enemiesInPlay += 1;
            spawnTimer = Random.Range(spawnTimerMin, spawnTimerMax);
        }
    }

    public void SpawnEnemy(Transform spawnPos)
    {
        GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Count)];
        GameObject newEnemy = Instantiate(enemyType, spawnPos.position, UnityEngine.Quaternion.identity);
        //give player object to new enemy
        newEnemy.GetComponent<AIDestinationSetter>().target = player.transform;

        if (newEnemy.CompareTag("MeleeEnemy")) { newEnemy.GetComponent<MeleeEnemy>().player = player; newEnemy.GetComponent<MeleeEnemy>().spawner = gameObject; }
        else if (newEnemy.CompareTag("RangedEnemy")) { newEnemy.GetComponent<RangedEnemy>().player = player; newEnemy.GetComponent<RangedEnemy>().spawner = gameObject;}
        //play sound effect and do animation
    }

    private void Die()
    {
        //contact level manager and tell it you died
        GameManager.Instance.spawnersInPlay -= 1;
        Destroy(gameObject, 0.5f);
    }

    public void TakeDamage(float damage)
    {
        if (spawnerInPlay)
        {
            Debug.Log("Spawner health: " + health);
            Debug.Log("spawner took " + damage + " damage");
            health -= damage;
            Debug.Log("Spawner health: " + health);
            if (health <= 0)
            {
                Die();
            }
        }
    }
}
