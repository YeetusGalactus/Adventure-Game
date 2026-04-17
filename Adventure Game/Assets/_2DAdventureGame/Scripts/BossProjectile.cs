using UnityEngine;

public class BossProjectile : MonoBehaviour
{

    Rigidbody2D Rigidbody2d;

    void Awake() {
        Rigidbody2d = GetComponent<Rigidbody2D>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 400.0f) {
            Destroy(gameObject);
        }
    }

    public void Launch(float force) {
        Vector2 direction = new Vector2(Random.Range(-0.1f, 0.1f), -1f).normalized;
        Rigidbody2d.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null) {
            player.ChangeHealth(-1);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Hit by " + collision.gameObject.name);
        Destroy(gameObject);
    }

}
