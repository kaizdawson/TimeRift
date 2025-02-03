using UnityEngine;

public class Rilft : MonoBehaviour
{
    public void Attack()
    {
        Debug.Log("Rilft Attack");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
