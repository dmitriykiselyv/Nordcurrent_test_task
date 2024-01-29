using System.Collections.Generic;
using UnityEngine;

public class SimpleSaveHandler
{
    private int _enemyMaxPoolSize;
    private GameData _gameData;

    public SimpleSaveHandler(int enemyMaxPoolSize)
    {
        _enemyMaxPoolSize = enemyMaxPoolSize;
    }

    public void SaveTanks(Tank playerTank, List<Tank> enemyTanks)
    {
        _gameData.EnemyData.Clear();
        _gameData.PlayerData = new GameData.TankData
        {
            Position = playerTank.transform.position,
            Rotation = playerTank.transform.eulerAngles.z
        };
        foreach (var tank in enemyTanks)
        {
            GameData.TankData tankData = new GameData.TankData
            {
                Position = tank.transform.position,
                Rotation = tank.transform.transform.eulerAngles.z
            };
            _gameData.EnemyData.Add(tankData);
        }

        string json = JsonUtility.ToJson(_gameData);
        System.IO.File.WriteAllText(Application.persistentDataPath + GameTags.SAVE_FILE_NAME, json);
    }

    public GameData LoadGameData()
    {
        string path = Application.persistentDataPath + GameTags.SAVE_FILE_NAME;
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            _gameData = JsonUtility.FromJson<GameData>(json);

            return _gameData;
        }
        _gameData = new GameData(_enemyMaxPoolSize);
        return _gameData;
    }
}
