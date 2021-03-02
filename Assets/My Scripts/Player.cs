using UnityEngine;
using DG.Tweening;
public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform[] _targets;
    
    [SerializeField]
    private float _speed;
    private int waypointIndex = 0;
    
    private Touch touch;
    
    private Vector3 touchPos;
    private Vector3 _initialScale;

    private bool _ringButtonActive;
    private void Awake()
    {
        _initialScale = transform.localScale;

        if (PlayerPrefs.GetInt("CurrentLevel") > 10 && PlayerPrefs.GetInt("CurrentLevel") < 20)
        {
            _speed = 8;
        }
        
        else if (PlayerPrefs.GetInt("CurrentLevel") >= 20 && PlayerPrefs.GetInt("CurrentLevel") < 30)
        {
            _speed = 9;
        }
        else if (PlayerPrefs.GetInt("CurrentLevel") >= 30)
        {
            _speed = 10;
        }
    }
    private void Update()
    {
        if (GameManager.Instance.IsLevelPassed || GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePaused)
        {
            return;
        }
        
        
        TouchInput();
        ScaleInAndOut();
        Move();
        
    }
    
    private void Move()
    {
        if(transform.position == _targets[_targets.Length - 1].position)
        {
            AudioManager.Instance.PlaySound(2);
            GameEvents.LevelComplete();
            GameObject obj = GameObjectPool.Instance.SpawnFromPool("Confetti");
            GameObject obj2 = GameObjectPool.Instance.SpawnFromPool("Firework");
            obj.transform.position = transform.position;
            obj2.transform.position = transform.position;
            return;
        }
        
        if (waypointIndex <= _targets.Length - 1)
        {
            Vector3 targetDirection = _targets[waypointIndex].position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection,
                5 * Time.deltaTime, 1f);

            transform.rotation = Quaternion.LookRotation(newDirection);

            transform.position = Vector3.MoveTowards(transform.position,
            _targets[waypointIndex].transform.position,_speed * Time.deltaTime);

            if (transform.position == _targets[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
    }

    private void ScaleInAndOut()
    {
        if (Input.GetKey(KeyCode.Space) || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        {
            transform.DOScaleX(0.26f, 0.1f);
            transform.DOScaleY(0.26f, 0.1f);
            
            if(_ringButtonActive)
                return;
            
            StartCoroutine(UIManager.Instance.CloseRingButton());
            _ringButtonActive = true;
        }

        else
        {
            transform.DOScale(_initialScale, 0.1f);
        }
    }

    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        }
    }
    
}
