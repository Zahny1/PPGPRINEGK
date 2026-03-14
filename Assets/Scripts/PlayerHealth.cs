using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [Header("Player Stats")]
    public int maxHealth = 10;
    public int currentHealth;

    void Awake() {
        currentHealth = maxHealth;
    }

    // ------------------------
    // Schaden nehmen
    // ------------------------
    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log("Spieler hat " + damage + " Schaden bekommen. HP: " + currentHealth);

        if (currentHealth <= 0) {
            Die();
        }
    }

    // ------------------------
    // Spieler Tod
    // ------------------------
    void Die() {
        Debug.Log("Spieler ist gestorben!");
        // Option 1: Scene neu laden
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

        // Option 2: Spieler deaktivieren
        gameObject.SetActive(false);

        // Hier könntest du z.B. GameOver-UI aktivieren
    }

    // ------------------------
    // Optional: Heilung
    // ------------------------
    public void Heal(int amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        Debug.Log("Spieler geheilt. HP: " + currentHealth);
    }
}