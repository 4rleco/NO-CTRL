using UnityEngine;

public class WireTimer : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float timer = 0.0f;
    int target;
    private void Start()
    {
        target = Random.Range(0, 3);
        
    }
    private void Update()
    {
        if (timer >= target)
        {
            animator.enabled = true;
        }

        timer += Time.deltaTime;
    }
}