using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;

    // Variables related to the health system
    public int maxHealth = 5;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;
    public InputAction LaunchAction;


    // talk to NPC
    public InputAction TalkAction;
    public event Action OnTalkedToNPC;

    // audio one shots
    AudioSource audioSource;

    private NonPlayerCharacter lastNonPlayerCharacter;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

        MoveAction.Enable();
        LaunchAction.Enable();
        TalkAction.Enable();

        currentHealth = maxHealth;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();


        GameManager gm = FindAnyObjectByType<GameManager>();
        if (gm != null) {
            OnTalkedToNPC += gm.HandlePlayerTalkedToNPC;
        }
    }

    // Update is called once per frame
    void Update() {
        move = MoveAction.ReadValue<Vector2>();
        //Debug.Log(move);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible) {
            damageCooldown -= Time.deltaTime;

            if(damageCooldown < 0) {
                isInvincible = false;
            }
        }

        if (LaunchAction.WasPressedThisFrame()) {
            Launch();
        }

        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.5f, moveDirection, 2f, LayerMask.GetMask("NPC"));
        if (hit.collider != null) {
            NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();
            npc.dialogueBubble.SetActive(true);
            lastNonPlayerCharacter = npc;
            FindFriend(hit);
            
        } else {
            if (lastNonPlayerCharacter != null) {
                lastNonPlayerCharacter.dialogueBubble.SetActive(false);
                lastNonPlayerCharacter = null;
            }
        }

    }

    // FixedUpdate has the same call rate as the physics system 
    void FixedUpdate() {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount) {
        if(amount < 0) {
            if(isInvincible) {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    void Launch() {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);
        animator.SetTrigger("Launch");
    }

    void FindFriend(RaycastHit2D hit) {
        if (TalkAction.WasPressedThisFrame()) {
            UIHandler.instance.DisplayDialogue();
            OnTalkedToNPC?.Invoke();
        }
    }

    public void PlaySound(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }


}