using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D Rigidbody2d;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Rigidbody2d = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 200.0f) {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force) {
        Rigidbody2d.AddForce(direction * force);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        EnemyController enemy = other.GetComponent<EnemyController>();
        BossEnemy boss = other.GetComponent<BossEnemy>();

        if(enemy != null) {
            enemy.Fix();
        }
        Destroy(gameObject);

        if(boss != null) {
            if(boss.currentHealth == 1) {
                boss.currentHealth = 0;
                boss.Fix();
            } else {
                boss.Shot();
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
