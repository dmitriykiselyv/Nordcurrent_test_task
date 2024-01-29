using UnityEngine;

public abstract class TankFactory
{
    protected Tank _prefab;

    protected TankFactory(Tank prefab)
    {
        _prefab = prefab;
    }

    public abstract Tank Create();
}
