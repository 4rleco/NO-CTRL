using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Administrador_QTE : MonoBehaviour
{
    [Header("Configuración de Tiempos")]
    [SerializeField] private float timeBetweenQTE = 20f;
    [SerializeField] private float timeToComplete = 10.0f;

    [Header("Referencias UI")]
    [SerializeField] private Image[] arrowImages;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private GameObject timerFrame;

    [Header("Sprites de las flechas")]
    [SerializeField] private Sprite[] arrowsSprites;

    [Header("Cantidad de teclas")]
    [SerializeField] private int amountOfKeys = 3;

    [Header("Sonidos")]
    [SerializeField] private AK.Wwise.Event successEvent;
    [SerializeField] private AK.Wwise.Event correctKeyEvent;

    private float delayTimer;
    private float timeToCompleteTimer;
    private bool qteActive = false;

    private KeyCode[] correctKey;
    private int amountOfCorrectKeys;
    private Vector2 savePrevPlayerPosition;


    private PlayerActions player;
    private Rigidbody2D playerRb;


    private KeyCode[] arrows = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

    void Start()
    {
        amountOfCorrectKeys = 0;

        player = FindFirstObjectByType<PlayerActions>();
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }

        delayTimer = timeBetweenQTE;

        HideQTEUI();
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

        amountOfCorrectKeys = 0;

        for (int i = 0; i < amountOfKeys; i++)
        {
            int randomIndex = Random.Range(0, arrows.Length);

            correctKey[i] = arrows[randomIndex];

            arrowImages[i].sprite = arrowsSprites[randomIndex];
            arrowImages[i].color = Color.white;
            arrowImages[i].gameObject.SetActive(true);
        }

        if (timerFrame != null)
            timerFrame.SetActive(true);

        if (timer != null)
        {
            timer.gameObject.SetActive(true);
            timer.color = Color.yellow;
            timer.text = $"{timeToCompleteTimer:F1}s";
        }
    }

    void QTEHandler()
    {
        timeToCompleteTimer -= Time.deltaTime;

        if (timer != null)
        {
            timer.text = $"{timeToCompleteTimer:F1}s";

            if (timeToCompleteTimer <= 3.0f)
                timer.color = Color.red;
            else
                timer.color = Color.yellow;
        }

        if (timeToCompleteTimer <= 0)
        {
            FinishWithFail();
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(correctKey[amountOfCorrectKeys]))
            {
                arrowImages[amountOfCorrectKeys].color = Color.green;

                if (correctKeyEvent != null && correctKeyEvent.IsValid() && player != null)
                    AkUnitySoundEngine.PostEvent(correctKeyEvent.Id, player.gameObject);

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

                foreach (Image img in arrowImages)
                    img.color = Color.white;
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

    void HideQTEUI()
    {
        if (arrowImages != null)
        {
            foreach (Image img in arrowImages)
            {
                if (img != null)
                    img.gameObject.SetActive(false);
            }
        }

        if (timerFrame != null)
            timerFrame.SetActive(false);

        if (timer != null)
            timer.gameObject.SetActive(false);
    }

    void CompleteQTESuccessfuly()
    {
        qteActive = false;
        delayTimer = timeBetweenQTE;

        HideQTEUI();

        if (successEvent != null && successEvent.IsValid() && player != null)
            AkUnitySoundEngine.PostEvent(successEvent.Id, player.gameObject);

        UnityEngine.Debug.Log("¡QTE Correcto! Continuando juego.");
    }

    void FinishWithFail()
    {
        qteActive = false;
        delayTimer = timeBetweenQTE;

        HideQTEUI();

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