using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void LoadGame()
    {
        StartCoroutine(PlaySoundThenLoadScene());
    }

    IEnumerator PlaySoundThenLoadScene()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound,2f);
            yield return new WaitForSeconds(clickSound.length);
        }

        // Xoá toàn bộ các DontDestroyOnLoad nếu cần
        var go = GameObject.Find("GameOver");
        if (go != null) Destroy(go);

        var player = GameObject.Find("Player");
        if (player != null && player.transform.parent == null)
        {
            Destroy(player);
        }

        SceneManager.LoadScene("Scene0");
    }

    public void ExitGame()
    {
        StartCoroutine(PlaySoundThenExit());
    }

    IEnumerator PlaySoundThenExit()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound,2f);
            yield return new WaitForSeconds(clickSound.length);
        }

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
