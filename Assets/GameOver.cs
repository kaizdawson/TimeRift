using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Hủy nếu đã có một GameOver tồn tại
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Không bị mất khi chuyển scene
        gameObject.SetActive(false);   // Ẩn lúc đầu
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
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu"); 
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


}
