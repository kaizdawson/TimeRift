using UnityEngine;

public class Handgun : MonoBehaviour
{
    public void Attack()
    {
        Debug.Log("Handgun Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
