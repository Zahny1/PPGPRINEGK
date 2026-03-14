using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement2D : MonoBehaviour {
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Attack")]
    public Collider2D attackUp;
    public Collider2D attackDown;
    public Collider2D attackLeft;
    public Collider2D attackRight;
    public int damage = 1;
    public float attackDuration = 0.2f;
    public float attackCooldown = 0.5f;

    [Header("UI")]
    public GameObject pauseMenu;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;
    private bool isMenuOpen = false;
    private float lastAttackTime = 0f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        HandleInputs();
    }

    void FixedUpdate() {
        if (isMenuOpen) return;
        MovePlayer();
    }

    // ------------------------
    // INPUT
    // ------------------------
    void HandleInputs() {
        // Menü Toggle
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMenu();

        if (isMenuOpen) return;

        // Bewegung Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (movement != Vector2.zero)
            lastDirection = movement;

        // Angriff Input Linksklick + Cooldown
        if (Input.GetMouseButtonDown(0)) {
            if (Time.time >= lastAttackTime + attackCooldown) {
                lastAttackTime = Time.time;
                Debug.Log("Attack Input (Left Click)");
                Attack();
            } else {
                Debug.Log("Attack on Cooldown");
            }
        }
    }

    // ------------------------
    // MENU
    // ------------------------
    void ToggleMenu() {
        isMenuOpen = !isMenuOpen;

        if (pauseMenu != null)
            pauseMenu.SetActive(isMenuOpen);

        Time.timeScale = isMenuOpen ? 0f : 1f;
    }

    // ------------------------
    // MOVEMENT
    // ------------------------
    void MovePlayer() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // ------------------------
    // ATTACK
    // ------------------------
    void Attack() {
        Collider2D selectedHitbox;

        if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y)) {
            selectedHitbox = lastDirection.x > 0 ? attackRight : attackLeft;
        } else {
            selectedHitbox = lastDirection.y > 0 ? attackUp : attackDown;
        }

        StartCoroutine(DoAttack(selectedHitbox));
    }

    IEnumerator DoAttack(Collider2D hitbox) {
        hitbox.gameObject.SetActive(true);

        DealDamage(hitbox);

        yield return new WaitForSeconds(attackDuration);

        hitbox.gameObject.SetActive(false);
    }

    // ------------------------
    // DAMAGE ONLY ON ENEMY BODY 
    // ------------------------
    void DealDamage(Collider2D hitbox) {
        // Nur echte Colliders, keine Trigger (z.B. AggroHitbox)
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;

        Collider2D[] results = new Collider2D[10];
        int count = hitbox.OverlapCollider(filter, results);

        HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();

        for (int i = 0; i < count; i++) {
            GameObject obj = results[i].gameObject;

            if (damagedEnemies.Contains(obj)) continue;

            damagedEnemies.Add(obj);

            // Nur Damageable Enemy Collider
            EnemyHealth enemy = obj.GetComponentInParent<EnemyHealth>();
            if (enemy != null) {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy getroffen: " + obj.name);
            }
        }
    }
}