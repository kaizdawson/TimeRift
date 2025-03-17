using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Animator myAnimator;

    private void Awake()
    {
        myAnimator=GetComponent<Animator>();
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

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
