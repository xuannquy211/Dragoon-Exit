using System.Collections.Generic;
using UnityEngine;

public class GamePool<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T prefab;
    [SerializeField] private int defaultAmount;
    [SerializeField] private Transform container;

    private Queue<T> pool;
    
    private void Start()
    {
        pool = new Queue<T>();

        for (var i = 0; i < defaultAmount; i++)
        {
            var obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);
            
            pool.Enqueue(obj);
        }
    }

    public T GetObj()
    {
        while (true)
        {
            if (pool.Count > 0)
            {
                var obj = pool.Dequeue();
                if (obj == null) continue;

                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                var obj = Instantiate(prefab, container ? container : transform);
                return obj;
            }

            break;
        }
    }

    public void BackToPool(T obj)
    {
        if(pool.Count >= defaultAmount) Destroy(obj.gameObject);
        else
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}