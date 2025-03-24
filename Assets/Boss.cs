using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject hitbox_Slash;
    public Transform player;
    public bool isFlipped = false;
    public AudioClip attackClip;
    private AudioSource audioSource;
    public AudioClip footstepClip;
    public AudioClip DeathClip;
    public AudioClip IntroClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    private void Start()
    {
        if (PlayerController.Instance == null)
            Debug.LogWarning("PlayerController.Instance is NULL at Boss Start");

        player = PlayerController.Instance?.transform;
    }




    public void EnableHitbox()
    {
        hitbox_Slash.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitbox_Slash.SetActive(false);
    }

    public void PlayAttackSound()
    {
        if (attackClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackClip);
        }
    }
    public void PlayFootstepSound()
    {
        if (footstepClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(footstepClip);
        }
    }
    public void PlayDeathSound()
    {
        if (footstepClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(DeathClip);
        }
    }
    public void PlayIntroSound()
    {
        if (footstepClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(IntroClip);
        }
    }

}
