using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDeathHandler : MonoBehaviour
{
    public void OnBossDeathAnimationEnd()
    {
        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager != null)
        {
            Destroy(audioManager);
        }
        SceneManager.LoadScene("SceneCredit");
    }
}
