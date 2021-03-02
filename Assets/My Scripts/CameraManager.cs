using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offSet;
    private Vector3 _velocity = Vector3.one;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _smoothness;

    private Vector3 _cameraInitialPosition;
    private float _shakeMagnitude = 0.095f;
    private float _shakeTime = 0.2f;

    private bool _stopFollowing;
    
    private void OnEnable()
    {
        GameEvents.ONCollidedWithObstacles += ShakeIt;
        GameEvents.ONLevelComplete += StopFollowing;
    }

    private void OnDisable()
    {
        GameEvents.ONCollidedWithObstacles -= ShakeIt;
        GameEvents.ONLevelComplete -= StopFollowing;
    }
    private void LateUpdate()
    {
        if(!_stopFollowing)
            Follow();
    }
    private void StopFollowing()
    {
        _stopFollowing = true;
    }
    private void Follow()
    {
        Vector3 toPos = _target.position + (_target.rotation * _offSet);
        Vector3 curPos = Vector3.SmoothDamp(transform.position, toPos, ref _velocity, _smoothness);
        transform.position = curPos;

        transform.LookAt(_target);
    }

    private void ShakeIt()
    {
        _cameraInitialPosition = transform.position;
        InvokeRepeating(nameof(StartCameraShaking), 0.1f, 0.001f);
        Invoke(nameof(StopCameraShaking), _shakeTime);
    }

    private void StartCameraShaking()
    {
        float cameraShakingOffsetX = Random.value * _shakeMagnitude * 2 - _shakeMagnitude;
        float cameraShakingOffsetZ = Random.value * _shakeMagnitude * 2 - _shakeMagnitude;
        Vector3 cameraIntermadiatePosition = transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
        cameraIntermadiatePosition.z += cameraShakingOffsetZ;
        transform.position = cameraIntermadiatePosition;
    }

    private void StopCameraShaking()
    {
        CancelInvoke(nameof(StartCameraShaking));
        transform.position = _cameraInitialPosition;
    }
}
