using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Player health
    public float player_health = 100f;

    //Melee Attack info
    public float meleeRange = 1f;   // distance in front of player
    public float meleeRadius = 0.5f; // size of hitbox
    public int damage = 1;
    public float attackRate = 0.5f;
    private float nextAttackTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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

        Vector2 direction = (mousePos - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, GetComponent<Transform>().position, Quaternion.identity);

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Melee()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 direction = (mousePos - transform.position).normalized;
        Vector2 attackPos = (Vector2)transform.position + direction * meleeRange;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, meleeRadius);

    }
    void OnDrawGizmosSelected()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 direction = (mousePos - transform.position).normalized;
        Vector2 attackPos = (Vector2)transform.position + direction * meleeRange;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, meleeRadius);
    }

    public void take_damage(float damage)
    {
        player_health = player_health - damage;
    }
}
