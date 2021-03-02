using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsLevelPassed { get; private set; }
    public bool IsGamePaused { get; private set; }
    
    private Scene _scene;
    private void Awake()
    {
        Instance = this;
        _scene = SceneManager.GetActiveScene();
    }

    private void OnEnable()
    {
        GameEvents.ONLevelComplete += LevelComplete;
        GameEvents.ONCollidedWithObstacles += GameOver;
        GameEvents.ONLevelContinue += ContinueGame;
    }
    
    private void OnDisable()
    {
        GameEvents.ONLevelComplete -= LevelComplete;
        GameEvents.ONLevelContinue -= ContinueGame;
        GameEvents.ONCollidedWithObstacles -= GameOver;
    }
    private void GameOver()
    {
        IsGameOver = true;
        Invoke(nameof(GameOverDelay), 1f);
    }

    private void GameOverDelay()
    {
        UIManager.Instance.GameOverPanel(true);
    }

    private void ContinueGame()
    {
        int currentScene = _scene.buildIndex;
        SceneManager.LoadSceneAsync(currentScene);
    }

    private void LevelComplete()
    {
        IsLevelPassed = true;
        Invoke(nameof(LevelCompleteDelay), 1f);
    }

    private void LevelCompleteDelay()
    {
        UIManager.Instance.LevelCompletePanel(true);
    }
    
    public void NextLevel()
    {
        int nextScene = _scene.buildIndex + 1;

        if (nextScene <= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadSceneAsync(nextScene);
            PlayerPrefs.SetInt("SceneIndex", nextScene);
        }
        else
        {
            SceneManager.LoadSceneAsync(1);
            PlayerPrefs.SetInt("SceneIndex", 1);
        }

        
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteKey("CurrentScore");
        int currentScene = _scene.buildIndex;
        SceneManager.LoadSceneAsync(currentScene);
    }
    public void PauseGame()
    {
        IsGamePaused = true;
    }
    public void UnpauseGame()
    {
        IsGamePaused = false;
    }

    public void LevelContinue()
    {
        GameEvents.LevelContinue();
    }

    public void NewRingUnlock()
    {
        GameEvents.NewRingUnlock();
    }
}
