using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour {
    [Header("UI")]
    [SerializeField] private GameObject menuObject;

    [Header("Scenes")]
    [SerializeField] private int mainMenuBuildIndex = 0; // MainMenu Szene
    [SerializeField] private int gameSceneBuildIndex = 1; // Spielszene

    private bool isMenuOpen = false;

    void Update() {
        HandleInputs();
    }

    // ------------------------
    // INPUT HANDLING
    // ------------------------
    private void HandleInputs() {
        // Menü Toggle
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();
            Debug.Log("ESC gedrückt");
        }

        // Linksklick (Attack / Hit)
        if (!isMenuOpen && Input.GetMouseButtonDown(0)) {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayHit();
            Debug.Log("Linke Maustaste gedrückt");
        }

        // Rechtsklick (Optional / Kontext)
        if (!isMenuOpen && Input.GetMouseButtonDown(1)) {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayClick();
            Debug.Log("Rechte Maustaste gedrückt");
        }

        // Save / Load Hotkeys
        if (Input.GetKeyDown(KeyCode.F5))
            SaveGame();

        if (Input.GetKeyDown(KeyCode.F9))
            LoadGame();
    }

    // ------------------------
    // MENU
    // ------------------------
    public void ToggleMenu() {
        isMenuOpen = !menuObject.activeSelf;
        menuObject.SetActive(isMenuOpen);
        Time.timeScale = isMenuOpen ? 0f : 1f;
    }

    // ------------------------
    // SAVE / LOAD
    // ------------------------
    public void SaveGame() {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null && SaveSystem.Instance != null) {
            SaveSystem.Instance.SavePlayer(player);
            Debug.Log("Spiel gespeichert (Button/F5)");
        } else {
            Debug.LogWarning("Kein Player oder SaveSystem gefunden!");
        }

        // Nach dem Speichern ins MainMenu
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuBuildIndex);
    }

    public void LoadGame() {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null && SaveSystem.Instance != null) {
            SaveSystem.Instance.LoadPlayer(player);
            Debug.Log("Spiel geladen (Button/F9)");
        } else {
            Debug.LogWarning("Kein Player oder SaveSystem gefunden!");
        }
    }

    // ------------------------
    // RESUME BUTTON
    // ------------------------
    public void ResumeGame() {
        // Callback registrieren, um Spielerstand nach Szenenwechsel zu laden
        SceneManager.sceneLoaded += LoadPlayerAfterScene;

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneBuildIndex);
    }

    private void LoadPlayerAfterScene(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == gameSceneBuildIndex) {
            PlayerHealth player = FindObjectOfType<PlayerHealth>();
            if (player != null && SaveSystem.Instance != null) {
                SaveSystem.Instance.LoadPlayer(player);
                Debug.Log("Spielstand geladen nach Resume!");
            } else {
                Debug.LogWarning("Kein Player oder SaveSystem in der Spielszene gefunden!");
            }

            // Callback entfernen, sonst wird es erneut ausgeführt
            SceneManager.sceneLoaded -= LoadPlayerAfterScene;
        }
    }

    // ------------------------
    // QUIT
    // ------------------------
    public void QuitGame() {
        Debug.Log("Spiel wird beendet");
        Application.Quit();
    }
}