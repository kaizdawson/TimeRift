using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    private AudioSource audioSource;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_TEXT = "Scene0";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        isDead= false;
        currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound, 0.5f); 
        }

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            if (deathSound != null)
            {
                audioSource.PlayOneShot(deathSound, 0.7f);
            }
            StartCoroutine(DeathLoadSceneRoutine());
            
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound, 0.5f);
            yield return new WaitForSeconds(deathSound.length);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        // Không destroy và không load scene tại đây
        // Gọi hàm xử lý hiển thị GameOver UI
        OnDeathAnimationEnd();
    }


    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    public void OnDeathAnimationEnd()
    {
        Debug.Log(">> [DEBUG] OnDeathAnimationEnd called");

        if (GameOver.Instance != null)
        {
            GameOver.Instance.ShowUI();
        }
        else
        {
            Debug.LogWarning(">> GameOver.Instance is NULL");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Scene0")
        {
            isDead = false;
            currentHealth = maxHealth;
            canTakeDamage = true;

            if (healthSlider == null)
            {
                healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
            }

            UpdateHealthSlider();
        }
    }




}
