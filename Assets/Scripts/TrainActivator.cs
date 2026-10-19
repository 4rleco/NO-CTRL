using System;
using UnityEngine;

public class TrainActivator : MonoBehaviour
{
    public event Action<bool> OnActivateTrain;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnActivateTrain?.Invoke(true); 
        }
    }         
}
