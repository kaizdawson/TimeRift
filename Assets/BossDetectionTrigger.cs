using UnityEngine;

public class BossDetectionTrigger : MonoBehaviour
{
    private Animator bossAnimator;
    private bool hasTriggered = false;

    private void Start()
    {

        bossAnimator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;

        if (collision.CompareTag("Player"))
        {
            bossAnimator.SetTrigger("Intro");
            hasTriggered = true;
        }
    }
}
