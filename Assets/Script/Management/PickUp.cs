using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private AudioClip pickUpSound;
    private AudioSource audioSource;
    private enum PickUpType
    {
        GoldCoin,
        StaminaGlobe,
        HealthGlobe,
    }

    [SerializeField] private PickUpType pickUpType;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private float accelartionRate = .2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;
    private bool isPickedUp = false;



    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelartionRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isPickedUp) return;
        if (other.gameObject.GetComponent<PlayerController>())
        {
            isPickedUp = true;
            DetectPickupType();

            if (audioSource != null && pickUpSound != null)
            {
                audioSource.PlayOneShot(pickUpSound); // Phát âm thanh khi nhặt
                StartCoroutine(DestroyAfterSound(pickUpSound.length)); // Chờ nhạc xong mới hủy
            }
            else
            {
                Destroy(gameObject); // Nếu không có âm thanh, hủy ngay
            }
        }
    }

    private IEnumerator DestroyAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay); // Chờ nhạc phát xong
        Destroy(gameObject);
    }
    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPoint=transform.position;
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        Vector2 endPoint=new Vector2(randomX, randomY);

        float timePassed = 0f;
        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT=timePassed / popDuration;
            float heightT=animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);
            transform.position = Vector2.Lerp(endPoint, startPoint, linearT) + new Vector2(0f, height);
            yield return null;

        }
    }

    private void DetectPickupType()
    {
        switch (pickUpType)
        {
            case PickUpType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentCoin();
                break;
            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer();
                break;
            case PickUpType.StaminaGlobe:
                Stamina.Instance.RefreshStamina();
                break;
        }
    }
}
