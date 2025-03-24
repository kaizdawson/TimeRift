using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public float scrollSpeed = 40f;
    private RectTransform rectTransform;
    public float timeToReturn = 30f;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(ReturnToMainMenuAfterDelay());
    }

     void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }

    IEnumerator ReturnToMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(timeToReturn);
        SceneManager.LoadScene("MainMenu");
    }
}
