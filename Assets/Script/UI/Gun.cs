using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private AudioClip fireSound;
    readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Animator myAnimator;
    private AudioSource audioSource;
    private void Awake()
    {
        myAnimator=GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        MouseFollowWithOffset();
    }
    public void Attack()
    {
        myAnimator.SetTrigger(FIRE_HASH);
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newBullet.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);


        if (audioSource != null && fireSound != null)
        {
            AudioClip shortClip = TrimAudioClip(fireSound, 0f, 0.2f); // Lấy 1.5 giây đầu
            audioSource.PlayOneShot(shortClip, 0.5f);
        }
        else
        {
            Debug.LogWarning("Thiếu AudioSource hoặc Fire Sound trên Gun!");
        }
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
     
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;

        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 weaponScale = ActiveWeapon.Instance.transform.localScale;
        weaponScale.y = (mousePos.x < playerScreenPoint.x) ? -1 : 1;
        ActiveWeapon.Instance.transform.localScale = weaponScale;
    }

    private AudioClip TrimAudioClip(AudioClip clip, float startTime, float length)
    {
        int frequency = clip.frequency;
        int samplesLength = Mathf.Clamp((int)(length * frequency), 0, clip.samples);
        float[] data = new float[samplesLength];

        clip.GetData(data, (int)(startTime * frequency));

        AudioClip newClip = AudioClip.Create(clip.name + "_trimmed", samplesLength, clip.channels, frequency, false);
        newClip.SetData(data, 0);
        return newClip;
    }
}
