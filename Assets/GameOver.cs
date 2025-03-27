using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public static GameOver Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
        gameObject.SetActive(false);   
    }

    public void ShowUI()
    {
        Debug.Log(">> [DEBUG] ShowUI Called");

        var es = UnityEngine.EventSystems.EventSystem.current;
        if (es != null)
        {
            Debug.Log(">> [DEBUG] EventSystem exists: " + es.name);
            Debug.Log(">> [DEBUG] Module: " + es.currentInputModule);
        }
        else
        {
            Debug.LogWarning(">> [DEBUG] No EventSystem found!");
        }

        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }


    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(PlaySoundThenLoadMainMenu());
    }

    public void QuitGame()
    {
        StartCoroutine(PlaySoundThenQuit());
    }


    IEnumerator PlaySoundThenLoadMainMenu()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
            yield return new WaitForSecondsRealtime(clickSound.length);
        }

        Time.timeScale = 1f;

        // Dọn sạch các singleton hoặc DontDestroyOnLoad
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("AudioManager"));
        Destroy(GameObject.Find("GameOver"));
        Destroy(GameObject.Find("SceneManagement"));
        Destroy(GameObject.Find("EventSystem")); // nếu cần

        // Load lại scene đầu tiên như vừa mở game
        SceneManager.LoadScene(0);
    }


    IEnumerator PlaySoundThenQuit()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
            yield return new WaitForSecondsRealtime(clickSound.length);
        }

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
