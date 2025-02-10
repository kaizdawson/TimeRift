using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeObject : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    public float fadeAmount = 0.4f;
    private float originalAlpha;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        originalAlpha = tilemapRenderer.material.color.a;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color tempColor = tilemapRenderer.material.color;
            tempColor.a = fadeAmount;
            tilemapRenderer.material.color = tempColor;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Color tempColor = tilemapRenderer.material.color;
            tempColor.a = originalAlpha;
            tilemapRenderer.material.color = tempColor;
        }
    }
}
