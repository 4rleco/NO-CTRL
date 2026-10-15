using System;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private TrainActivator activator;

    [Header("Speed")]
    [SerializeField] private float speed = 5.0f;
    private Vector3 inittialPos;

   
    private bool subscribedToDeath = false;

    private void Awake()
    {
        activator.OnActivateTrain += OnActivateTrain;

        inittialPos = transform.position;

        enabled = false;
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

    private void Update()
    {
        transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, 0.0f, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Floor")) return;

        ResetTrain();
    }

  
    private void ResetTrain()
    {
        transform.position = inittialPos;

        enabled = false;
    }

    private void OnDestroy()
    {
        activator.OnActivateTrain -= OnActivateTrain;

      
        if (subscribedToDeath && PlayerEvents.current != null)
        {
            PlayerEvents.current.onPlayerDeath -= OnPlayerDeath;
        }
    }
}