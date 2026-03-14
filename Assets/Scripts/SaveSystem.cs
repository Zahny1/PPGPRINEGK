using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour {
    public static SaveSystem Instance; // Singleton

    private string saveFile => Application.persistentDataPath + "/savefile.json";

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // über Szenen hinweg behalten
    }

    public void SavePlayer(PlayerHealth player) {
        SaveData data = new SaveData();
        data.posX = player.transform.position.x;
        data.posY = player.transform.position.y;
        data.health = player.currentHealth;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFile, json);

        Debug.Log("Spiel gespeichert!");
    }

    public void LoadPlayer(PlayerHealth player) {
        if (!File.Exists(saveFile)) {
            Debug.LogWarning("Keine Save-Datei gefunden!");
            return;
        }

        string json = File.ReadAllText(saveFile);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        player.transform.position = new Vector2(data.posX, data.posY);
        player.currentHealth = data.health;

        Debug.Log("Spielstand geladen!");
    }
}