using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<TankData> EnemyData = new List<TankData>();
    public TankData PlayerData;

    public GameData(int enemyMaxPoolSize)
    {
        PlayerData = new TankData()
        {
            Position = Vector3.zero,
            Rotation = Random.Range(0f, 360f)
        };

        for(int i = 0; i < enemyMaxPoolSize; i++)
        {
            var enemyTankData = new TankData()
            {
                Position = Vector3.zero,
                Rotation = Random.Range(0f, 360f)
            };
            EnemyData.Add(enemyTankData);
        }
    }

    [System.Serializable]
    public class TankData
    {
        public Vector3 Position;
        public float Rotation;
    }
}
