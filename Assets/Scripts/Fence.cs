using UnityEngine;

[RequireComponent(typeof(AkGameObj))]
public class ElectricFence : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event loopEvent; 
    [SerializeField] private AK.Wwise.Event hitEvent;  

    private void OnEnable()
    {
        AkUnitySoundEngine.PostEvent(loopEvent.Id, gameObject);
    }

    private void OnDisable()
    {
        loopEvent.Stop(gameObject);
    }

    public void PlayHit()
    {
        AkUnitySoundEngine.PostEvent(hitEvent.Id, gameObject);
    }
}