using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
    [Header("Stats")]
    public float moveSpeed = 3f;
    public float attackRange = 1f;
    public int damage = 1;
    public float attackCooldown = 1f;

    [Header("Idle Wander")]
    public float idleMoveRadius = 2f;
    public float idleMoveDelay = 2f;

    [Header("Aggro")]
    public Collider2D aggroHitbox;
    public float aggroDuration = 5f;       // Sekunden, wie lange Aggro anhält
    public float sprintMultiplier = 2f;    // Geschwindigkeit kurzzeitig erhöhen
    public float sprintDuration = 0.5f;    // Dauer des Sprints in Sekunden

    private Transform player;
    private Rigidbody2D rb;
    private bool isAggro = false;
    private float lastAttackTime = 0f;
    private float lastIdleTime = 0f;
    private float aggroTimer = 0f;
    private float sprintTimer = 0f;       // Countdown für Sprint
    private Vector2 idleTarget;
    private float currentSpeed;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        idleTarget = rb.position;
        currentSpeed = moveSpeed;
    }

    void FixedUpdate() {
        // Aggro Timer runterzählen
        if (aggroTimer > 0f) {
            aggroTimer -= Time.fixedDeltaTime;
            isAggro = true;
        } else if (aggroTimer <= 0f) {
            isAggro = false;
        }

        // Sprint Timer runterzählen
        if (sprintTimer > 0f) {
            sprintTimer -= Time.fixedDeltaTime;
            currentSpeed = moveSpeed * sprintMultiplier;
        } else {
            currentSpeed = moveSpeed;
        }

        if (isAggro && player != null) {
            FollowPlayer();
        } else {
            IdleWander();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            aggroTimer = aggroDuration;  // Aggro Timer reset
            sprintTimer = sprintDuration; // Start Sprint
            Debug.Log(name + " ist jetzt aggressiv! Sprint startet");
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log(name + " Spieler aus Aggro, Timer läuft...");
        }
    }

    void FollowPlayer() {
        Vector2 direction = (player.position - transform.position);
        float distance = direction.magnitude;

        if (distance > attackRange) {
            direction.Normalize();
            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);
        } else {
            if (Time.time >= lastAttackTime + attackCooldown) {
                lastAttackTime = Time.time;
                AttackPlayer();
            }
        }
    }

    void AttackPlayer() {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null) {
            playerHealth.TakeDamage(damage);
            Debug.Log(name + " greift Spieler an!");
        }
    }

    void IdleWander() {
        if (Time.time >= lastIdleTime + idleMoveDelay) {
            lastIdleTime = Time.time;
            Vector2 randomOffset = Random.insideUnitCircle * idleMoveRadius;
            idleTarget = (Vector2)rb.position + randomOffset;
        }

        Vector2 moveDir = idleTarget - rb.position;
        if (moveDir.magnitude > 0.1f) {
            moveDir.Normalize();
            rb.MovePosition(rb.position + moveDir * (moveSpeed * 0.5f) * Time.fixedDeltaTime);
        }
    }

    public void TakeDamage(int amount) {
        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null)
            enemyHealth.TakeDamage(amount);
    }
}