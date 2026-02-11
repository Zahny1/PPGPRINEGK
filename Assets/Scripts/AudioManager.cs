using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip menuMusik;
    public AudioClip jumpSound;
    public AudioClip clickSound;
    public AudioClip hitSound;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        CheckScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        CheckScene(scene.buildIndex);
    }

    private void CheckScene(int buildIndex) {
        if (buildIndex == 0) // Menü Szene
        {
            audioSource.clip = menuMusik;
            audioSource.loop = true;
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
    }

    public void PlaySound(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }

    public void PlayJump() => PlaySound(jumpSound);
    public void PlayClick() => PlaySound(clickSound);
    public void PlayHit() => PlaySound(hitSound);
}
