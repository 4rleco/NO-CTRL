using UnityEngine;
using UnityEngine.UI;

public class Train : MonoBehaviour
{
    [SerializeField] private TrainActivator activator;
    [SerializeField] private Transform deactivator;


    [Header("Speed")]
    [SerializeField] private float speed = 5.0f;

    private BoxCollider2D bc;

    [Header("Alert Icon")]
    [SerializeField] private Image image;
    [SerializeField] private float alertDuration = 2.5f;
    private float currentAlertDuration = 0.0f;

    private Vector3 inittialPos;

    private bool subscribedToDeath = false;

    private void Awake()
    {
        bc = GetComponentInChildren<BoxCollider2D>();

        activator.OnActivateTrain += OnActivateTrain;

        inittialPos = transform.position;

        enabled = false;

        image.enabled = false;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, inittialPos.y, 0.0f);

        if (transform.position.x < deactivator.transform.position.x)
            ResetTrain();

        if (image.enabled)
        {
            Debug.Log("A");

            currentAlertDuration += Time.deltaTime;

            if (currentAlertDuration >= alertDuration)
                image.enabled = false;
        }
    }

    private void OnDestroy()
    {
        activator.OnActivateTrain -= OnActivateTrain;

        if (subscribedToDeath && PlayerEvents.current != null)
        {
            PlayerEvents.current.onPlayerDeath -= OnPlayerDeath;
        }

        currentAlertDuration = 0.0f;
    }

    private void OnActivateTrain(bool obj)
    {
        enabled = obj;

        image.enabled = true;

        Debug.Log("Activated");

        if (!subscribedToDeath && PlayerEvents.current != null)
        {
            PlayerEvents.current.onPlayerDeath += OnPlayerDeath;
            subscribedToDeath = true;
        }
    }

    private void OnPlayerDeath(PlayerActions player)
    {
        ResetTrain();
    }

    private void ResetTrain()
    {
        transform.position = inittialPos;

        enabled = false;
    }
}