using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _fieldBoundaries;
    [SerializeField] private Tank _playerTank;
    [SerializeField] private Tank _enemyTank;
    [SerializeField] private CoroutineRunner _coroutineRunner;

    private TankSpawner _tankSpawner;
    private int _enemyMaxPoolSize = 10;
    private SimpleSaveHandler _simpleSaveHandler;


    private void Awake()
    {
        var playerTank = CreateTank(new PlayerTankFactory(_playerTank));
        var enemyTankFactory = new EnemyTankFactory(_enemyTank);
        var enemyTankPool = new GenericObjectPool<Tank>(() => CreateTank(enemyTankFactory), _enemyMaxPoolSize);
        enemyTankFactory.OnConstruct(_fieldBoundaries);

        _simpleSaveHandler = new SimpleSaveHandler(_enemyMaxPoolSize);
        _tankSpawner = new TankSpawner(playerTank, enemyTankPool, _fieldBoundaries, _coroutineRunner, _simpleSaveHandler);

        _tankSpawner.SpawnPlayerTank();
        StartCoroutine(_tankSpawner.SpawnEnemyTanks());
    }

    public void SaveTanksPosition()
    {
        _tankSpawner.SavePositions();
    }

    private void OnDestroy()
    {
        StopCoroutine(_tankSpawner.SpawnEnemyTanks());
    }

    private Tank CreateTank(TankFactory factory)
    {
        return factory.Create();
    }
}
