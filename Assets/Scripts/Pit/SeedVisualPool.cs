using UnityEngine;
using System.Collections.Generic;


public class SeedVisualPool : MonoBehaviour
{
    [SerializeField] private GameObject seedPrefab;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(seedPrefab);
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
