using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public GameObject pauseUI;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseUI.SetActive(false);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(PlaySoundThenLoadMainMenu());
    }

    public void ExitGame()
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

        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("AudioManager"));
        Destroy(GameObject.Find("PauseGame"));
        Destroy(GameObject.Find("EventSystem"));

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
