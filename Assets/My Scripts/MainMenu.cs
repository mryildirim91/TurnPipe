using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        if (PlayerPrefs.HasKey("SceneIndex"))
        {
            SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("SceneIndex"));
        }
        else
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
