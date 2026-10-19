using System;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private TrainActivator activator;
    [SerializeField] private Transform deactivator;

    [Header("Speed")]
    [SerializeField] private float speed = 5.0f;

    private BoxCollider2D bc;

    private Vector3 inittialPos;

    private bool subscribedToDeath = false;

    private void Awake()
    {
        bc = GetComponentInChildren<BoxCollider2D>();

        activator.OnActivateTrain += OnActivateTrain;

        inittialPos = transform.position;

        enabled = false;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, inittialPos.y, 0.0f);

        if(transform.position.x < deactivator.transform.position.x)
            ResetTrain();
    }

    private void OnDestroy()
    {
        activator.OnActivateTrain -= OnActivateTrain;

        if (subscribedToDeath && PlayerEvents.current != null)
        {
            PlayerEvents.current.onPlayerDeath -= OnPlayerDeath;
        }
    }

    private void OnActivateTrain(bool obj)
    {
        enabled = obj;

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