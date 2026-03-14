using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public int health = 3;

    public void TakeDamage(int damage) {
        health -= damage;

        Debug.Log(name + " hat Schaden bekommen. HP: " + health);

        if (health <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log(name + " ist gestorben");
        Destroy(gameObject);
    }
}