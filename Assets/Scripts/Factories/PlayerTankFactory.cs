using UnityEngine;

public class PlayerTankFactory : TankFactory
{
    public PlayerTankFactory(Tank prefab) : base(prefab) { }

    public override Tank Create()
    {
        Tank tank = GameObject.Instantiate(_prefab);
        var playerTank = (PlayerTank)tank;
        playerTank.OnConstruct();
        playerTank.gameObject.SetActive(false);
        return playerTank;
    }
}
