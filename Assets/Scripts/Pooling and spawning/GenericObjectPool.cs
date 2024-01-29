using UnityEngine.Pool;
using UnityEngine;
using System;

public class GenericObjectPool<T> where T : Component
{
    public int MaxSize { get; private set; }

    private ObjectPool<T> _objectPool;

    public GenericObjectPool(Func<T> createFunc, int maxSize = 10)
    {
        MaxSize = maxSize;

        _objectPool = new ObjectPool<T>(
            createFunc: createFunc,
            actionOnGet: (item) => item.gameObject.SetActive(true),
            actionOnRelease: (item) => item.gameObject.SetActive(false),
            actionOnDestroy: (item) => UnityEngine.Object.Destroy(item.gameObject),
            collectionCheck: false,
            maxSize: MaxSize
        );
    }

    public T Get()
    {
        return _objectPool.Get();
    }

    public void Release(T item)
    {
        _objectPool.Release(item);
    }
}