using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI jumpKey;
    [SerializeField] private TextMeshProUGUI dashKey;
    [SerializeField] private TextMeshProUGUI crouchKey;

    [SerializeField] private PlayerActions player;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;

    private KeyCode lastJump;
    private KeyCode lastDash;
    private KeyCode lastCrouch;

    private void Start()
    {
        pauseMenu.SetActive(false);

        winMenu.SetActive(false);

        loseMenu.SetActive(false);
    }

    private void Update()
    {
        jumpKey.text = "Jump Key: " + player.GetJumpKey();

        dashKey.text = "Dash Key: " + player.GetDashKey();

        crouchKey.text = "Crouch Key: " + player.GetCrouchKey();

        pauseMenu.SetActive(player.GetPlayerPause());

        loseMenu.SetActive(player.GetPlayerDied());

        float cooldownRestante = player.GetDashCooldownTimer();

        if (cooldownRestante > 0)
        {
            dashKey.text = "Dash Key: " + player.GetDashKey() + " [ " + cooldownRestante.ToString("F1") + "s]";
        }
        else
        {
            dashKey.text = "Dash Key: " + player.GetDashKey() + " [READY]";
        }

        if (lastJump != KeyCode.None && player.GetJumpKey() != lastJump) StartCoroutine(FlashRed(jumpKey));

        if (lastDash != KeyCode.None && player.GetDashKey() != lastDash) StartCoroutine(FlashRed(dashKey));

        if (lastCrouch != KeyCode.None && player.GetCrouchKey() != lastCrouch) StartCoroutine(FlashRed(crouchKey));


        lastJump = player.GetJumpKey();

        lastDash = player.GetDashKey();

        lastCrouch = player.GetCrouchKey();

        if (loseMenu.activeSelf || pauseMenu.activeSelf)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;


        if (player.GetPlayerWin())
        {
            Time.timeScale = 0.0f;

            if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("ThirdLevel"))
                winMenu.SetActive(true);
            else
                NextLevel();
        }
    }

    private IEnumerator FlashRed(TextMeshProUGUI textMesh)
    {
        textMesh.color = Color.red;

        yield return new WaitForSeconds(1.0f);

        textMesh.color = Color.white;
    }

    private void NextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevel > SceneManager.sceneCount)
        {
            SceneManager.LoadScene(nextLevel);
        }

        Time.timeScale = 1.0f;

        player.SetPlayerWin(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetGame()
    {
        Time.timeScale = 1.0f;

        int curretnScene = SceneManager.GetActiveScene().buildIndex;

        player.SetPlayerDied(false);

        pauseMenu.SetActive(false);
        loseMenu.SetActive(false);

        SceneManager.LoadScene(curretnScene);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);

        player.SetPlayerPause(false);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detiene el modo Play en Unity
#endif
    }
}