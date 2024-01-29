using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TankSpawner
{
    private delegate Vector3 GeneratePositionFunc(BoxCollider2D collider, ref float rotation);

    private PlayerTank _playerTank;
    private GenericObjectPool<Tank> _enemyTankPool;
    private SpriteRenderer _fieldBoundaries;
    private CoroutineRunner _coroutineRunner;
    private int _numberOfTanksDestroyed;
    private SimpleSaveHandler _simpleSaveHandler;
    private GameData _gameData;
    private List<Tank> _activeEnemyTanks = new List<Tank>();
    private int _numberOfTanksAtLaunch;
    private WaitForSeconds _w4s = new WaitForSeconds(1f);

    public TankSpawner(Tank playerTank, GenericObjectPool<Tank> enemyTankPool, SpriteRenderer fieldBoundaries, CoroutineRunner coroutineRunner, SimpleSaveHandler simpleSaveHandler)
    {
        _playerTank = (PlayerTank)playerTank;
        _enemyTankPool = enemyTankPool;
        _fieldBoundaries = fieldBoundaries;
        _coroutineRunner = coroutineRunner;
        _simpleSaveHandler = simpleSaveHandler;

        _gameData = _simpleSaveHandler.LoadGameData();
        _numberOfTanksAtLaunch = _gameData.EnemyData.Count;
    }

    public void SavePositions()
    {
        _simpleSaveHandler.SaveTanks(_playerTank, _activeEnemyTanks);
    }

    public void SpawnPlayerTank()
    {
        float rotation = _gameData.PlayerData.Rotation;
        Vector3 spawnPosition = _gameData.PlayerData.Position;
        bool isOverlap = PositionOverlapCheck(spawnPosition, _playerTank.BoxCollider2D, rotation);
        if(isOverlap)
        {
            spawnPosition = FindFreeSpawnPosition(GenerateRandomPositionInField, _playerTank.BoxCollider2D, ref rotation);
        }
        _playerTank.transform.SetPositionAndRotation(spawnPosition, Quaternion.Euler(0f, 0f, rotation));
        _playerTank.gameObject.SetActive(true);
        _playerTank.TankDestroyed += PlayerTankWasDestroyed;
    }

    private void PlayerTankWasDestroyed(Tank tank)
    {
        _playerTank.TankDestroyed -= PlayerTankWasDestroyed;
        _coroutineRunner.StartCoroutine(RespawnPlayerTank());
    }

    private IEnumerator RespawnPlayerTank()
    {
        yield return _w4s;

        float rotation = Random.Range(0f, 360f);
        Vector2 spawnPosition = FindFreeSpawnPosition(GenerateRandomCornerPosition, _playerTank.BoxCollider2D, ref rotation);
        _playerTank.transform.SetPositionAndRotation(spawnPosition, Quaternion.Euler(0f, 0f, rotation));
        _playerTank.gameObject.SetActive(true);
        _playerTank.TankDestroyed += PlayerTankWasDestroyed;
    }

    public IEnumerator SpawnEnemyTanks()
    {
        _numberOfTanksAtLaunch = _gameData.EnemyData.Count;
        for (int i = 0; i < _gameData.EnemyData.Count; i++)
        {
            Tank tank = _enemyTankPool.Get();
            _activeEnemyTanks.Add(tank);

            EnemyTank enemyTank = (EnemyTank)tank;
            enemyTank.TankDestroyed += EnemyTankWasDestroyed;
            enemyTank.gameObject.name = $"Enemy {i + 1}";

            float rotation = _gameData.EnemyData[i].Rotation;
            Vector3 spawnPosition = _gameData.EnemyData[i].Position;
            bool isOverlap = PositionOverlapCheck(spawnPosition, enemyTank.BoxCollider2D, rotation);
            if (isOverlap)
            {
                spawnPosition = FindFreeSpawnPosition(GenerateRandomBoundaryPosition, enemyTank.BoxCollider2D, ref rotation);
            }
            enemyTank.transform.SetPositionAndRotation(spawnPosition, Quaternion.Euler(0f, 0f, rotation));

            yield return null;
        }
    }

    private void EnemyTankWasDestroyed(Tank tank)
    {
        var enemyTank = (EnemyTank)tank;
        enemyTank.TankDestroyed -= EnemyTankWasDestroyed;
        _enemyTankPool.Release(tank);
        _activeEnemyTanks.Remove(tank);
        _numberOfTanksDestroyed++;

        CheckIsAllTanksDestroyed();
    }

    private void CheckIsAllTanksDestroyed()
    {
        bool allTanksIsDestroyed = _numberOfTanksAtLaunch == _numberOfTanksDestroyed;
        if (allTanksIsDestroyed)
        {
            _numberOfTanksDestroyed = 0;
            _coroutineRunner.StartCoroutine(RespawnEnemyTanks());
        }
    }

    private IEnumerator RespawnEnemyTanks()
    {
        yield return _w4s;

        _activeEnemyTanks.Clear();
        _numberOfTanksAtLaunch = _enemyTankPool.MaxSize;

        for (int i = 0; i < _enemyTankPool.MaxSize; i++)
        {
            Tank tank = _enemyTankPool.Get();
            _activeEnemyTanks.Add(tank);
            EnemyTank enemyTank = (EnemyTank)tank;
            enemyTank.TankDestroyed += EnemyTankWasDestroyed;
            float rotation = 0;
            Vector3 spawnPosition = FindFreeSpawnPosition(GenerateRandomBoundaryPosition, enemyTank.BoxCollider2D, ref rotation);
            enemyTank.transform.SetPositionAndRotation(spawnPosition, Quaternion.Euler(0f, 0f, rotation));

            yield return null;
        }
    }

    private Vector3 GenerateRandomCornerPosition(BoxCollider2D collider, ref float rotation)
    {
        float cornerX, cornerY;
        float boundaryOffset = 0.5f;
        float offsetX = collider.offset.x;
        float offsetY = collider.offset.y;
        float halfWidth = collider.size.x / 2;
        float halfHeight = collider.size.y / 2;
        float x = 0, y = 0;

        int cornerIndex = Random.Range(0, 4);

        switch (cornerIndex)
        {
            case 0:
                cornerX = _fieldBoundaries.bounds.min.x;  //topLeft
                cornerY = _fieldBoundaries.bounds.max.y;
                rotation = Random.value > 0.5f ? 0f : -90f;
                x = cornerX + (rotation == 0f ? halfWidth - offsetX + boundaryOffset : halfHeight + boundaryOffset);
                y = cornerY - (rotation == 0f ? halfHeight - offsetY + boundaryOffset : halfWidth - offsetX + boundaryOffset);
                break;
            case 1:
                cornerX = _fieldBoundaries.bounds.max.x; //topRight
                cornerY = _fieldBoundaries.bounds.max.y;
                rotation = Random.value > 0.5f ? -90f : -180f;
                x = cornerX - (rotation == -90f ? halfHeight + boundaryOffset : halfWidth - offsetX + boundaryOffset);
                y = cornerY - (rotation == -90f ? halfWidth - offsetX + boundaryOffset : halfHeight + boundaryOffset) + offsetY;
                break;
            case 2:
                cornerX = _fieldBoundaries.bounds.min.x; //bottomLeft
                cornerY = _fieldBoundaries.bounds.min.y;
                rotation = Random.value > 0.5f ? 0f : 90f;
                x = cornerX + (rotation == 0f ? halfWidth - offsetX + boundaryOffset : halfHeight + boundaryOffset);
                y = cornerY + (rotation == 0f ? halfHeight - offsetY + boundaryOffset : halfWidth - offsetX + boundaryOffset);
                break;
            case 3:
                cornerX = _fieldBoundaries.bounds.max.x; //bottomRight
                cornerY = _fieldBoundaries.bounds.min.y;
                rotation = Random.value > 0.5f ? 90f : 180f;
                x = cornerX - (rotation == 90f ? halfHeight + offsetX + boundaryOffset : halfWidth + boundaryOffset) + offsetX;
                y = cornerY + (rotation == 90f ? halfWidth - offsetX + boundaryOffset : halfHeight + boundaryOffset) - offsetY;
                break;
        }

        return new Vector3(x, y, 0);
    }

    private Vector3 GenerateRandomBoundaryPosition(BoxCollider2D collider, ref float rotation)
    {
        float x, y;
        float boundaryOffset = 0.5f;

        if (Random.value > 0.5f) 
        {
            var randmValue = Random.value;

            x = randmValue > 0.5 ? _fieldBoundaries.bounds.min.x : _fieldBoundaries.bounds.max.x;
            y = Random.Range(_fieldBoundaries.bounds.min.y + collider.size.y / 2, _fieldBoundaries.bounds.max.y - collider.size.y / 2);

            rotation = Random.value > 0.5f ? 90f : -90f;

            x += randmValue > 0.5f ? collider.size.y / 2 + boundaryOffset : -collider.size.y / 2 - boundaryOffset;
        }
        else 
        {
            var randmValue = Random.value;

            y = randmValue > 0.5f ? _fieldBoundaries.bounds.min.y : _fieldBoundaries.bounds.max.y;
            x = Random.Range(_fieldBoundaries.bounds.min.x + collider.size.x / 2, _fieldBoundaries.bounds.max.x - collider.size.x / 2);

            rotation = Random.value > 0.5f ? 0f : 180f;

            y += (randmValue > 0.5f ? collider.size.y / 2 + boundaryOffset: -collider.size.y / 2 - boundaryOffset);
        }

        return new Vector3(x, y, 0);
    }

    private Vector3 GenerateRandomPositionInField(BoxCollider2D collider, ref float rotation)
    {
        float x = Random.Range(_fieldBoundaries.bounds.min.x + collider.size.x / 2, _fieldBoundaries.bounds.max.x - collider.size.x / 2);
        float y = Random.Range(_fieldBoundaries.bounds.min.y + collider.size.y / 2, _fieldBoundaries.bounds.max.y - collider.size.y / 2);

        return new Vector3(x, y, 0);
    }


    private Vector3 FindFreeSpawnPosition(GeneratePositionFunc generatePositionFunc, BoxCollider2D collider2D, ref float rotation, int maxAttempts = 100)
    {
        int attempts = 0;
        Vector3 spawnPosition;

        do
        {
            spawnPosition = generatePositionFunc(collider2D, ref rotation);
            if (++attempts > maxAttempts)
            {
                Debug.LogWarning("Maximum attempts to find free position reached.");
                return Vector3.zero;
            }
        }
        while (PositionOverlapCheck(spawnPosition, collider2D, rotation));

        //TankSpawnerGizmos.UpdateData(spawnPosition, collider2D, rotation);
        return spawnPosition;
    }

    private bool PositionOverlapCheck(Vector3 position, BoxCollider2D collider2D, float rotation)
    {
        Quaternion quaternion = Quaternion.Euler(0f, 0f, rotation);
        Vector3 adjustedOffset = quaternion * new Vector3(collider2D.offset.x, collider2D.offset.y, 0f);
        Vector3 adjustedPosition = position + adjustedOffset;
        Collider2D hitCollider = Physics2D.OverlapBox(adjustedPosition, collider2D.size, rotation);
        return hitCollider != null;
    }
}
