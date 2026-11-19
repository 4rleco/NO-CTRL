using TMPro;
using UnityEngine;

public class Administrador_QTE : MonoBehaviour
{
    [Header("Configuración de Tiempos")]
    [SerializeField] private float timeBetweenQTE = 20f;
    [SerializeField] private float timeToComplete = 10.0f;

    [Header("Referencias UI (TextMeshPro)")]
    [SerializeField] private TextMeshProUGUI arrowText;

    [Header("Cantidad de teclas")]
    [SerializeField] private int amountOfKeys = 3;

    private float delayTimer;
    private float timeToCompleteTimer;
    private bool qteActive = false;

    private KeyCode[] correctKey;
    private int amountOfCorrectKeys;
    private Vector2 savePrevPlayerPosition;


    private PlayerActions player;
    private Rigidbody2D playerRb;


    private KeyCode[] arrows = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
    private string[] arrowsText = { "PRESIONÁ: ↑ (ARRIBA)", "PRESIONÁ: ↓ (ABAJO)", "PRESIONÁ: ← (IZQUIERDA)", "PRESIONÁ: → (DERECHA)" };


    private string currentArrowBaseText;

    void Start()
    {
        amountOfCorrectKeys = 0;

        player = FindFirstObjectByType<PlayerActions>();
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }

        delayTimer = timeBetweenQTE;

        if (arrowText != null)
            arrowText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!qteActive)
        {
            NormalModeHandler();
        }
        else
        {
            if (player != null) player.SetCurrentSpeed(0);
            if (playerRb != null) playerRb.linearVelocity = Vector2.zero;

            QTEHandler();
        }
    }

    void NormalModeHandler()
    {
        if (!player.GetPlayerPause())
            delayTimer -= Time.deltaTime;

        if (delayTimer <= 0)
            ActivateQTE();
    }

    void ActivateQTE()
    {
        if (player == null) return;

        qteActive = true;
        timeToCompleteTimer = timeToComplete;

        savePrevPlayerPosition = player.transform.position;

        correctKey = new KeyCode[amountOfKeys];

        currentArrowBaseText = " ";
        amountOfCorrectKeys = 0;

        for (int i = 0; i < amountOfKeys; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, arrows.Length);
            correctKey[i] = arrows[randomIndex];
            currentArrowBaseText += arrowsText[randomIndex] + " ";
        }

        if (arrowText != null)
        {
            arrowText.color = Color.yellow;
            arrowText.gameObject.SetActive(true);
        }
    }

    void QTEHandler()
    {
        timeToCompleteTimer -= Time.deltaTime;

        if (arrowText != null)
        {
            if (timeToCompleteTimer <= 3.0f)
            {
                arrowText.color = Color.red;
            }
            else
            {
                arrowText.color = Color.yellow;
            }

            arrowText.text = $"{currentArrowBaseText} [{timeToCompleteTimer.ToString("F1")}s]";
        }

        if (timeToCompleteTimer <= 0)
        {
            FinishWithFail();
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(correctKey[amountOfCorrectKeys]))
            {
                amountOfCorrectKeys++;

                if (amountOfCorrectKeys >= amountOfKeys)
                {
                    CompleteQTESuccessfuly();
                    return;
                }
            }
            else if (PressIncorrectKey(correctKey[amountOfCorrectKeys]))
            {
                timeToCompleteTimer -= 0.5f;
                amountOfCorrectKeys = 0;
            }
        }

        Debug.Log(timeToCompleteTimer);
    }

    bool PressIncorrectKey(KeyCode currentArrow)
    {
        foreach (KeyCode arrow in arrows)
        {
            if (Input.GetKeyDown(arrow) && arrow != currentArrow)
            {
                return true;
            }
        }
        return false;
    }

    void CompleteQTESuccessfuly()
    {
        qteActive = false;
        delayTimer = timeBetweenQTE;

        if (arrowText != null)
            arrowText.gameObject.SetActive(false);

        UnityEngine.Debug.Log("¡QTE Correcto! Continuando juego.");
    }

    void FinishWithFail()
    {
        qteActive = false;
        delayTimer = timeBetweenQTE;

        if (arrowText != null)
            arrowText.gameObject.SetActive(false);

        UnityEngine.Debug.Log("¡QTE Fallado! El jugador muere y reinicia en el lugar.");


        if (player != null)
        {
            player.SetPlayerDied(true);
        }
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
        }

    }

    public bool GetIsActive()
    {
        return qteActive;
    }
}