using UnityEngine;

public class PlayerMovement2D : MonoBehaviour {
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Input holen (WASD + Pfeiltasten)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
