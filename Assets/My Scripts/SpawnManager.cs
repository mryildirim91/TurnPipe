using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private int _numOfObjectRings;
    private int[] _objectsAroundRod;

    private float[] _pipeRadius, _pipeCircumference, _pipeLength;
    private float _objectDiamater;
    private const float _completeCircleDegree = 360.0f;
    private int currentPipe = 0;

    private Vector3 _objectPos, _obstaclePos;
    private Quaternion _objectRot, _obtacleRot;

    [SerializeField]private GameObject _object;
    [SerializeField]private GameObject[] _obstacles;

    [SerializeField] private GameObject[] _pipes;
    
    private GameObject[] _pipeClones;

    private void Awake()
    {
        _pipeClones = new GameObject[_pipes.Length];
        _pipeRadius = new float[_pipes.Length];
        _pipeCircumference = new float[_pipes.Length];
        _pipeLength = new float[_pipes.Length];
        _objectsAroundRod = new int[_pipes.Length];
        PipeAndObjectParameters();
        SpawnObjects();
        SpawnObstacle();
    }

    private void PipeAndObjectParameters()
    {
        _objectDiamater = _object.transform.localScale.x;

        for (int i = 0; i < _pipes.Length; i++)
        {
            _pipeRadius[i] = _pipes[i].transform.localScale.x / 2 * 0.02f ;
            _pipeCircumference[i] = 2 * Mathf.PI * _pipeRadius[i];
            _pipeLength[i] = _pipes[i].transform.localScale.z * 0.02f;
            _objectsAroundRod[i] = (int)(_pipeCircumference[i] / _objectDiamater + 1);
            _numOfObjectRings = (int)(_pipeLength[i] / _objectDiamater);
        }
    }

    private void SpawnObjects()
    {
        int v = 0;
        
        for (int i = 0; i < _pipes.Length; i++)
        {
            Vector3 center = _pipes[i].transform.position;
            
            for (int z = -_numOfObjectRings / 2; z < _numOfObjectRings / 2  ; z++)
            {
                float angle = (v * _completeCircleDegree / _objectsAroundRod[i]);
                float objRingsOffset = _objectDiamater * z;
                _pipeClones[i] = _pipes[i];
                Vector3 pos = ObjectLocation(center, _pipeRadius[i], angle, objRingsOffset);
                Quaternion rot = ObjectRotation(angle);
                GameObject obj = Instantiate(_object, pos, rot);             
                obj.transform.parent = _pipeClones[i].transform;
                v++;
            }
            currentPipe++;
        }
    }
    private Vector3 ObjectLocation(Vector3 center, float radius, float angle, float objRingsOffset)
    {
        if (_pipes[currentPipe].transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
        {
            _objectPos.x = center.x + (radius + _objectDiamater / 2) * Mathf.Sin(angle * Mathf.Deg2Rad);
            _objectPos.y = center.y + (radius + _objectDiamater / 2) * Mathf.Cos(angle * Mathf.Deg2Rad);
            _objectPos.z = center.z + objRingsOffset;
        }
        else if (_pipes[currentPipe].transform.rotation == Quaternion.Euler(new Vector3(90, 0, 0)))
        {
            _objectPos.x = center.x + (radius + _objectDiamater / 2) * Mathf.Sin(angle * Mathf.Deg2Rad);
            _objectPos.y = center.y + objRingsOffset;
            _objectPos.z = center.z + (radius + _objectDiamater / 2) * Mathf.Cos(angle * Mathf.Deg2Rad);
        }
        else if (_pipes[currentPipe].transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
        {
            _objectPos.x = center.x + objRingsOffset;
            _objectPos.y = center.y + (radius + _objectDiamater / 2) * Mathf.Sin(angle * Mathf.Deg2Rad);
            _objectPos.z = center.z + (radius + _objectDiamater / 2) * Mathf.Cos(angle * Mathf.Deg2Rad);
        }

        return _objectPos;
    }

    private Quaternion ObjectRotation(float angle)
    {
        if (_pipes[currentPipe].transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
        {
            _objectRot = Quaternion.AngleAxis(angle, Vector3.forward);
            _objectRot.eulerAngles = new Vector3(90, transform.rotation.y, transform.rotation.z);
        }
        else if (_pipes[currentPipe].transform.rotation == Quaternion.Euler(new Vector3(90, 0, 0)))
        {
            _objectRot = Quaternion.AngleAxis(angle, Vector3.up);
        }
        else if (_pipes[currentPipe].transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
        {
            _objectRot = Quaternion.AngleAxis(angle, Vector3.right);
            _objectRot.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, 90);
        }

        return _objectRot;
    }
    private void SpawnObstacle()
    {
        for (int i = 0; i < _pipes.Length; i++)
        {
            Vector3 pos = _pipes[i].transform.position;
            GameObject obj = Instantiate(_obstacles[Random.Range(0, _obstacles.Length)], pos, _pipes[i].transform.rotation);
            obj.transform.parent = _pipes[i].transform;
        }
        
        /*for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < _pipes.Length; i++)
            {
                Vector3 pos = _pipes[i].transform.position;
                Vector3 pos2 = _pipes[i].transform.position;

                if (j == 0)
                {
                    if (_pipes[i].transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                    {
                        pos.z = _pipes[i].transform.position.z + _pipeLength[i] / 4f;
                        pos2.z = pos.z - 4;
                    }

                    else if (_pipes[i].transform.rotation == Quaternion.Euler(new Vector3(90, 0, 0)))
                    {
                        pos.y = _pipes[i].transform.position.y + _pipeLength[i] / 4f;
                        pos2.y = pos.y - 4;
                    }

                    else if (_pipes[i].transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                    {
                        pos.x = _pipes[i].transform.position.x + _pipeLength[i] / 4f;
                        pos2.x = pos.x - 4;
                    } 
                }
                
                else if (j == 1)
                {
                    if (_pipes[i].transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
                    {
                        pos.z = _pipes[i].transform.position.z - _pipeLength[i] / 4f;
                        pos2.z = pos.z + 4;
                    }

                    else if (_pipes[i].transform.rotation == Quaternion.Euler(new Vector3(90, 0, 0)))
                    {
                        pos.y = _pipes[i].transform.position.y - _pipeLength[i] / 4f;
                        pos2.y = pos.y + 4;
                    }

                    else if (_pipes[i].transform.rotation == Quaternion.Euler(new Vector3(0, 90, 0)))
                    {
                        pos.x = _pipes[i].transform.position.x - _pipeLength[i] / 4f;
                        pos2.x = pos.x + 4;
                    }
                }
                
                GameObject obj = Instantiate(_obstacles[Random.Range(0, _obstacles.Length)], pos, _pipes[i].transform.rotation);
                obj.transform.parent = _pipes[i].transform;
            }
        }*/
    }
}
