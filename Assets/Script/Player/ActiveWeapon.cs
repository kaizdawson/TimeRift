using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    [SerializeField] private MonoBehaviour currentActiveWeapon;

    private PlayerControls playerControls;

    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        //playerControls.Combat.Attack.started += _ => StartAttacking();
        //playerControls.Combat.Attack.canceled += _ => StopAttacking();
        // Tìm kiếm GameObject có script Sword và gán vào currentActiveWeapon
        currentActiveWeapon = FindObjectOfType<Sword>();

        if (currentActiveWeapon == null)
        {
            Debug.LogError("Không tìm thấy vũ khí nào trong game! Hãy kiểm tra lại.");
        }

        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Update()
    {
        Attack();
    }

    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            if (currentActiveWeapon == null)
            {
                Debug.LogError("currentActiveWeapon chưa được gán!");
                return;
            }

            IWeapon weapon = currentActiveWeapon as IWeapon;
            if (weapon != null)
            {
                weapon.Attack();
            }
            else
            {
                Debug.LogError("currentActiveWeapon không implement IWeapon!");
            }
        }
    }
}


