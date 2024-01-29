using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _maxLifetime = 5f;

    private GenericObjectPool<Projectile> _projectilePool;
    private float _lifetime;
    
    public void OnConstruct(GenericObjectPool<Projectile> projectilePool)
    {
        _projectilePool = projectilePool;
    }

    private void OnEnable()
    {
        _lifetime = _maxLifetime;
    }

    private void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector2.right);
        TrackingLifeTime();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToPool();
    }

    private void TrackingLifeTime()
    {
        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        _projectilePool.Release(this);
    }
}
