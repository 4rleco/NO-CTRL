using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionNumber;
    [SerializeField] private Canvas options;

    private void Start()
    {
        versionNumber.text = GameVersion.GetVersionNumber();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenOptions()
    {
        gameObject.SetActive(false);
        options.gameObject.SetActive(true);
    }

    public void CloseOptions()
    {
        gameObject.SetActive(true);
        options.gameObject.SetActive(false);
    }    

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detiene el modo Play en Unity
#endif
    }
}
