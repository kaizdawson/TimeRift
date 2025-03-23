using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Dragon : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
}
