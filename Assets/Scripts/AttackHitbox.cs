using UnityEngine;

public class AttackHitboxTest : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Hitbox hat getroffen: " + other.name);
    }
}