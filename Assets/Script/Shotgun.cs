using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public void Attack()
    {
        Debug.Log("Shotgun Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
