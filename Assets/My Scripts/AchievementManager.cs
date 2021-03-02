using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    [System.Serializable]
    private struct Rings
    {
        public string name;
        public int points;
        public Sprite sprite;
        public Button button;
        public Text text;
    }

    private int _currentRing;
    
    [SerializeField] private Rings[] _rings;
    [SerializeField] private Transform _player;
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private Text _priceText;
    [SerializeField] private GameObject _buyPanel;
    [SerializeField] private GameObject _closeButton;
    [SerializeField] private Image _ringImage;
    
    private void Awake()
    {
        PlayerPrefs.SetInt("TotalPoints", 1546937);
        _pointsText.text = PlayerPrefs.GetInt("TotalPoints").ToString();
        
        RingSelection();
        KeepSelectedRing();
    }
    private void RingSelection()
    {
        for (int i = 1; i < _rings.Length; i++)
        {
            if (PlayerPrefs.HasKey("Ring Name" + i))
            {
                _rings[i].button.image.sprite = _rings[i].sprite;
                _rings[i].text.text = "";
            }
            else
            {
                _rings[i].text.text = _rings[i].points.ToString();

                if (PlayerPrefs.GetInt("TotalPoints") < _rings[i].points)
                {
                    _rings[i].button.interactable = false;
                }
            }
        }
    }
    
    private void KeepSelectedRing()
    {
        for (int i = 0; i < _player.childCount; i++)
        {
            if (_rings[PlayerPrefs.GetInt("CurrentRing")].name == _player.GetChild(i).name)
            {
                _player.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                _player.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void SelectRing(int index)
    {
        _currentRing = index;
        
        if (!PlayerPrefs.HasKey("Ring Name" + index) && index > 0)
        {
            _buyPanel.SetActive(true);
            _ringImage.sprite = _rings[index].button.image.sprite;
            _priceText.text = _rings[index].text.text;
        }
        
        else if (PlayerPrefs.HasKey("Ring Name" + index))
        {
            for (int i = 0; i < _player.childCount; i++)
            {
                if(_rings[index].name == _player.GetChild(i).name)
                {
                    _player.GetChild(i).gameObject.SetActive(true);
                    PlayerPrefs.SetInt("CurrentRing", _currentRing);
                }
                else
                {
                    _player.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public void CloseBuyPanel()
    {
        _closeButton.SetActive(false);
        _buyPanel.SetActive(false);
    }

    public void BuyRing()
    {
        if (!PlayerPrefs.HasKey("Ring Name" + _currentRing) && _currentRing > 0)
        {
            AudioManager.Instance.PlaySound(1);
            PlayerPrefs.SetString("Ring Name" + _currentRing, _rings[_currentRing].name);
            _rings[_currentRing].button.image.sprite = _rings[_currentRing].sprite;
            _ringImage.sprite = _rings[_currentRing].sprite;
            _priceText.text = "";
            _rings[_currentRing].text.text = "";
            PlayerPrefs.SetInt("TotalPoints", PlayerPrefs.GetInt("TotalPoints") - _rings[_currentRing].points);
            _pointsText.text = PlayerPrefs.GetInt("TotalPoints").ToString();
        }

        for (int i = 1; i < _rings.Length; i++)
        {
            if (!PlayerPrefs.HasKey("Ring Name" + i) && PlayerPrefs.GetInt("TotalPoints") < _rings[i].points && _rings[i].button.interactable)
            {
                _rings[i].button.interactable = false;
            }
        }
        
        for (int i = 0; i < _player.childCount; i++)
        {
            if(_rings[_currentRing].name == _player.GetChild(i).name)
            {
                _player.GetChild(i).gameObject.SetActive(true);
                PlayerPrefs.SetInt("CurrentRing", _currentRing);
            }
            else
            {
                _player.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        GameObject obj = GameObjectPool.Instance.SpawnFromPool("BuyEffect");
        obj.transform.position = new Vector3(0,0.4f,5);
        
        _closeButton.SetActive(true);
    }
}
