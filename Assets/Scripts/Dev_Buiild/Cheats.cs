using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    [SerializeField] private PlayerActions player;

    private void Awake()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        enabled = true;
        gameObject.SetActive(true);
#else
        enabled = false;
        gameObject.SetActive(false);
#endif
    }

    public void FirstLevel()
    {
        Debug.Log("1");

        SceneManager.LoadScene(1);

        Time.timeScale = 1.0f;

        player.SetPlayerWin(false);
    }

    public void SecondLevel()
    {
        Debug.Log("2");

        SceneManager.LoadScene(2);

        Time.timeScale = 1.0f;

        player.SetPlayerWin(false);
    }

    public void ThirdLevel()
    {
        Debug.Log("3");

        SceneManager.LoadScene(3);

        Time.timeScale = 1.0f;

        player.SetPlayerWin(false);
    }
}
