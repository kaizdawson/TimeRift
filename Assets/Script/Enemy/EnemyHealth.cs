using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVGXPrefab;
    [SerializeField] private float knockBackThrust = 15f;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;

    private int currentHealth;
    private Knockback Knockback;
    private Flash flash;
    private bool isDead = false;
    public bool IsDead => isDead;
    public event System.Action OnDeath;
    private void Awake()
    {
        flash = GetComponent<Flash>();
        Knockback = GetComponent<Knockback>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        Knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        if (!isDead && !TryGetComponent<Boss>(out _))
            StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }
    public void DetectDeath()
    {

        if (isDead) return;
        if (currentHealth <= 0)
        {
            isDead = true;
            if (TryGetComponent<Boss>(out Boss boss))
            {
                Animator animator = GetComponent<Animator>();
                if (animator != null)
                {
                    flash.ResetMaterial();
                    animator.SetTrigger("Die");
                }

                
                StartCoroutine(DelayBeforeDestroy());
            }
            else
            {
                Instantiate(deathVGXPrefab, transform.position, Quaternion.identity);
                GetComponent<PickUpSpawner>().DropItems();
                if (TryGetComponent<AudioSource>(out _))
                {
                    Die();
                }
                else
                {
                    Destroy(gameObject); 
                }
            }
        }
    }
    private IEnumerator DelayBeforeDestroy()
    {
        yield return new WaitForSeconds(2f); 
        Instantiate(deathVGXPrefab, transform.position, Quaternion.identity);
        GetComponent<PickUpSpawner>().DropItems();
        if (TryGetComponent<AudioSource>(out _))
        {
            Die();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
            StartCoroutine(DestroyAfterSound(deathSound.length));
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private IEnumerator DestroyAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay); 
        Destroy(gameObject);
    }

}
