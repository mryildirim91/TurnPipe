using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _levelCompletePanel, _gameOverPanel, _ringPanel, _totalGoldPanel;

    [SerializeField] 
    private Text _goldText, _totalGoldText;
    [SerializeField]
    private Text _highScoreText, _levelClearedText, _moreGoldText;

    [SerializeField] private TMP_Text _levelTxt, _scoreTxt;

    [SerializeField] 
    private Button _ringButton, _continueButton, _moreGoldButton;
    private int _score, _golds;
    private int _highScore;
    private int _levelIndex;
    private int _randomNum;
    
    private void Awake()
    {
        Instance = this;
        _score = PlayerPrefs.GetInt("CurrentScore");
        _highScore = PlayerPrefs.GetInt("HighScore");
        _levelIndex = PlayerPrefs.GetInt("CurrentLevel") + 1;
        _levelTxt.text = "Level " + _levelIndex;
        _scoreTxt.text = _score.ToString();
        
        _randomNum = Random.Range(0, 10);
    }

    private void OnEnable()
    {
        GameEvents.OnNewRingUnlock += GiveMorePoints;
    }

    private void OnDisable()
    {
        GameEvents.OnNewRingUnlock -= GiveMorePoints;
    }

    public void LevelCompletePanel(bool active)
    {
        SetHighScore();
        _levelCompletePanel.SetActive(active);
        _moreGoldText.text = "GET x " + RandomMultiplier(); 
        _totalGoldPanel.SetActive(true);
        _highScoreText.gameObject.SetActive(active);
        _levelClearedText.text = "LEVEL " + _levelIndex + " CLEARED!";
        
        PlayerPrefs.SetInt("TotalPoints", PlayerPrefs.GetInt("TotalPoints") + _golds);
        _goldText.text = _golds.ToString();
        _totalGoldText.text = PlayerPrefs.GetInt("TotalPoints").ToString();
    }

    public void GameOverPanel(bool active)
    {
        SetHighScore();
        _gameOverPanel.SetActive(active);
        _highScoreText.gameObject.SetActive(active);
    }
    public void UpdateScore()
    {
        _score++;
        _golds++;
        _scoreTxt.text = _score.ToString();
        PlayerPrefs.SetInt("CurrentScore", _score);
    }

    public void RingSelectionPanel(bool active)
    {
        _ringPanel.SetActive(active);

        if (active)
            GameManager.Instance.PauseGame();
        else
            GameManager.Instance.UnpauseGame();
    }
    public IEnumerator CloseRingButton()
    {
        yield return BetterWaitForSeconds.Wait(0.1f);
        _ringButton.gameObject.SetActive(false);
        yield return BetterWaitForSeconds.Wait(0.1f);
        StopCoroutine(CloseRingButton());
    }
    private void SetHighScore()
    {
        if (_score > _highScore)
        {
            PlayerPrefs.SetInt("HighScore", _score);
        }

        _highScoreText.text = _score + "\nBEST " + PlayerPrefs.GetInt("HighScore");
    }

    private void GiveMorePoints()
    {
        _golds *= RandomMultiplier();
        PlayerPrefs.SetInt("TotalPoints", PlayerPrefs.GetInt("TotalPoints") + (_golds * (RandomMultiplier() - 1) / RandomMultiplier()));
        _goldText.text = _golds.ToString();
        _totalGoldText.text = PlayerPrefs.GetInt("TotalPoints").ToString();
        
        _continueButton.gameObject.SetActive(false);
        _moreGoldButton.gameObject.SetActive(false);
    }

    private int RandomMultiplier()
    {
        int multiplier;
        
        if (_randomNum == 5)
        {
            multiplier = 4;
        }
        else if (_randomNum == 3 || _randomNum == 7)
        {
            multiplier = 3;
        }
        else
        {
            multiplier = 2;
        }

        return multiplier;
    }
}
