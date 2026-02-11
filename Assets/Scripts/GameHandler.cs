using UnityEngine;

public class GameHandler : MonoBehaviour {

    [SerializeField] private GameObject MenuObjekt;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OpenMenu();
            Debug.Log("Etaste gedrückt");
        }

        if (Input.GetMouseButtonDown(0)) {
            AudioManager.Instance.PlayHit();
            Debug.Log("Linke Maustaste gedrückt");
        }

        if (Input.GetMouseButtonDown(1)) {
            AudioManager.Instance.PlayClick();
            Debug.Log("Rechte Maustaste gedrückt");
        }
    }
    




    //Save & Quit noch nicht Implimentiert!!!
    public void OpenMenu() {
        bool isActive = !MenuObjekt.activeSelf;
        MenuObjekt.SetActive(isActive);
        Time.timeScale = isActive ? 0f : 1f;
    }

}
