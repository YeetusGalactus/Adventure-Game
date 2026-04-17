using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossEnemy : MonoBehaviour
{
    public float speed = 3.0f; // SPEED
    public Rigidbody2D enemyRB; // RB

    bool vertical; // Move Direction

    // Move Timer
    public float changeTime = 3.0f;
    float timer;
    int direction = 1;
    //-------------------------

    //Health and Fixing
    public int maxHealth = 10;
    public int health { get { return currentHealth; } }
    public int currentHealth;

    bool broken = true;
    public bool isBroken { get { return broken; } }
    public event Action OnFixed;

    // particles
    public ParticleSystem smokeParticleEffect;

    // projectile
    public GameObject projectilePrefab;
    public float minDelay = 1.0f;
    public float maxDelay = 3.0f;
    public float launchForce = 1f;
    public Transform launchPoint;
    float shootTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager gm = FindAnyObjectByType<GameManager>();

        if (gm != null) {
            gm.RegisterBoss(this);
        }


        enemyRB = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        timer = changeTime;

        shootTimer = UnityEngine.Random.Range(minDelay, maxDelay);
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;

        if (timer < 0) {
            direction = -direction;
            timer = changeTime;
        }

        shootTimer -= Time.deltaTime;

        if(shootTimer <= 0f) {
            Shoot();

            shootTimer = UnityEngine.Random.Range(minDelay, maxDelay);
        }

        
    }

    private void FixedUpdate() {
        if (!broken) {
            return;
        }

        Vector2 position = enemyRB.position;
        if (!vertical) {
            position.x = position.x + speed * direction * Time.deltaTime;

        } else {
            position.y = position.y + speed * direction * Time.deltaTime;
        }
        enemyRB.MovePosition(position);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null) {
            player.ChangeHealth(-1);
        }
    }

    public void Fix() {
        broken = false;
        enemyRB.simulated = false;
        smokeParticleEffect.Stop();
        OnFixed?.Invoke();
    }

    public void Shot() {
        //broken = true;
        //enemyRB.simulated = false;
        //smokeParticleEffect.Stop();
        //OnFixed?.Invoke();
        currentHealth -= 1;
    }

    void Shoot() {
        if(!broken) {
            return;
        }

        GameObject projectileObject = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        BossProjectile projectile = projectileObject.GetComponent<BossProjectile>();

        if(projectile != null) {
            projectile.Launch(launchForce);
        }
    }
    
}
