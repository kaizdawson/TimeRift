using UnityEngine;

public class BossDetectionTrigger : MonoBehaviour
{
    public AudioClip bossMusic;
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
            AudioManager.Instance.PlayMusicWithFade(bossMusic, 1.5f);
            bossAnimator.SetTrigger("Intro");
            hasTriggered = true;
        }
    }
}
