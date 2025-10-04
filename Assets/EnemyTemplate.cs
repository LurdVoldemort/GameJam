using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Type", menuName = "EnemyType")]
public class EnemyTemplate : ScriptableObject
{
    public string enemyName = "New Enemy";
    public int maxHealth = 100;
    public float movementSpeed = 5f;
    public float damage = 5f;
    public GameObject enemyPrefab;

    [SerializeReference]
    public IUniqueBehavior uniqueBehavior;
}
