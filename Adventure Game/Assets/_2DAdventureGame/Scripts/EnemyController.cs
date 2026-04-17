using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    //variables
    public float speed = 3.0f;
    public Rigidbody2D enemyRB;
    
    bool vertical;

    public float changeTime = 3.0f;
    float timer;
    int direction = 1;

    Animator animator;
    bool broken = true;

    public bool isBroken { get { return broken; } }

    public ParticleSystem smokeParticleEffect;

    public event Action OnFixed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();

        timer = changeTime;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0) {
            direction = -direction;
            timer = changeTime;
        }
    }

    private void FixedUpdate() {
        if(!broken) {
            return;
        }

        Vector2 position = enemyRB.position;
        if(!vertical) {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
            position.x = position.x + speed * direction * Time.deltaTime;
            
        }else {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            position.y = position.y + speed * direction * Time.deltaTime;
        }
        enemyRB.MovePosition(position);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if(player != null) {
            player.ChangeHealth(-1);
        }
    }

    public void Fix() {
        broken = false;
        enemyRB.simulated = false;
        animator.SetTrigger("Fixed");
        smokeParticleEffect.Stop();
        OnFixed?.Invoke();
    }
}
