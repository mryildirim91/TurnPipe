using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            other.GetComponent<AudioSource>().Play();
            GameObject obj = GameObjectPool.Instance.SpawnFromPool("Ring");
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            
            GameEvents.CollidedWithObstacles();
            transform.parent.gameObject.SetActive(false);
        }

        if(other.tag == "Collectable")
        {
            AudioManager.Instance.PlaySound(0);
            other.gameObject.SetActive(false);
            GameObject obj = GameObjectPool.Instance.SpawnFromPool("Collectable");
            obj.transform.position = other.transform.position;
            obj.transform.rotation = other.transform.rotation;
            UIManager.Instance.UpdateScore();
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.name != "Egg" || gameObject.name != "Crown")
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 90, Space.Self);
        }
    }
}
