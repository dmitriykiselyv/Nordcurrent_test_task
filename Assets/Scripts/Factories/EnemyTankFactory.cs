using UnityEngine;

public class EnemyTankFactory : TankFactory
{
    private SpriteRenderer _fieldBoundaries;
    public EnemyTankFactory(Tank prefab) : base(prefab) { }

    public void OnConstruct(SpriteRenderer fieldBoundaries)
    {
        _fieldBoundaries = fieldBoundaries;
    }

    public override Tank Create()
    {
        Tank tank = GameObject.Instantiate(_prefab);
        var enemyTank = (EnemyTank)tank;
        enemyTank.OnConstruct(new SimpleTankAI(_fieldBoundaries));
        return enemyTank;
    }
}