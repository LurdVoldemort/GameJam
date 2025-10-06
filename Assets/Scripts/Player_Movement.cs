using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

public class Player_Movement : MonoBehaviour
{
    //player movement
    public float speed;
    private Rigidbody2D rb2d;
    //movement animation
    public Animator animator;

    //Firing bullet info
    public float fireRate = 0.25f;
    private float nextFireTime = 0f;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public bool activeBulletModifier = false;

    //bullet trail
    public GameObject trail;

    //Player health
    [SerializeField] public float player_max_health = 100f;
    public float player_health = 100f;

    //Melee Attack info
    public GameObject meleePrefab;
    public float meleeRange = 1f;   // distance in front of player
    public float meleeLifetime = 0.2f;
    public float meleeRadius = 0.5f; // size of hitbox
    public int damage = 1;
    public float attackRate = 0.5f;
    private float nextAttackTime = 0f;

    //list of cards
    private List<cards> cardList = new List<cards>();

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player_health = player_max_health;
        Debug.Log(player_health);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) // hold left click
        {
            Shoot();
            nextFireTime = Time.time + fireRate; // set next available fire time
        } else if (Input.GetMouseButton(1) && Time.time >= nextAttackTime)
        {
            Melee();
            nextAttackTime = Time.time + attackRate;
        }

        //Card activations
        if (Input.GetKeyDown(KeyCode.Alpha1) && cardList[0] != null){cardList[0].canUse(this);}
        if (Input.GetKeyDown(KeyCode.Alpha2) && cardList[1] != null) { cardList[1].canUse(this);}
        if (Input.GetKeyDown(KeyCode.Alpha3) && cardList[2] != null) { cardList[2].canUse(this);}

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        rb2d.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);


        //animator movement info
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("Speed", 0 < movement.sqrMagnitude);
    }

    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = ((Vector2)mousePos - (Vector2)transform.position).normalized;


        float trailOffset = 1f;
        Vector2 spawnPos = (Vector2)transform.position + direction * trailOffset;
        GameObject bulletTrail = Instantiate(trail, spawnPos, Quaternion.identity);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        bulletTrail.transform.rotation = Quaternion.Euler(0, 0, angle); 
        bulletTrail.transform.SetParent(transform);

        // Get number of bullets from active card
        int bulletCount = activeBulletModifier ? 3 : 1;

        float spreadAngle = 10f;

        if (bulletCount == 1)
        {
            SpawnBullet(direction);
        }
        else
        {
            float startAngle = -spreadAngle * (bulletCount - 1) / 2f; // center the spread

            for (int i = 0; i < bulletCount; i++)
            {
                float angleOffset = startAngle + (spreadAngle * i);
                Vector2 bulletDir = Quaternion.Euler(0, 0, angleOffset) * direction;
                SpawnBullet(bulletDir);
            }

        }
    }

    void SpawnBullet(Vector2 bulletDir)
    {
        float trailOffset = 1f;
        Vector2 spawnPos = (Vector2)transform.position + bulletDir * trailOffset;

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        float angle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
        Collider2D[] otherBullets = FindObjectsOfType<Collider2D>();

        foreach (Collider2D other in otherBullets)
        {
            if (other.TryGetComponent(out Bullet_Logic item))
            {
                Physics2D.IgnoreCollision(bulletCollider, other);
            }
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = bulletDir * bulletSpeed;
    }

    public void addCard(cards card)
    {
        cardList.Add(card);
    }

    void Melee()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDir = (mousePos - transform.position).normalized;

        // Spawn position a short distance in front of the player
        GameObject attackObj = Instantiate(meleePrefab, transform.position, Quaternion.identity);   

        // Determine the main attack direction
        string direction = "Attack_" + GetDirection(attackDir);

        // Enable the correct sprite
        foreach (Transform child in attackObj.transform)
        {
            child.gameObject.SetActive(child.name.Contains(direction));
        }

        attackObj.transform.SetParent(transform);

        Destroy(attackObj, meleeLifetime);

    }
    string GetDirection(Vector2 dir)
    {
        // Determine which direction is dominant
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            return dir.x > 0 ? "Right" : "Left";
        }
        else
        {
            return dir.y > 0 ? "Up" : "Down";
        }
    }

    public void take_damage(float damage)
    {
        Debug.Log(player_health + " - " + damage);
        player_health -= damage;
        if (player_health <= 0)
        {
            GameManager.Instance.PlayerDied(gameObject);
        }
        Debug.Log(player_health);
    }
}
