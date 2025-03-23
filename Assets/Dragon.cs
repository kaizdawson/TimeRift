using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Dragon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += PlayDragonDeathSound;  
        }
    }

    private void Update()
    {
        if (PlayerController.Instance == null) return;

        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 dragonPos = transform.position;

        if (playerPos.x < dragonPos.x)
        {
            spriteRenderer.flipX = true; 
        }
        else
        {
            spriteRenderer.flipX = false; 
        }
    }

    private void PlayDragonDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }


    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= PlayDragonDeathSound;  
        }
    }
}
