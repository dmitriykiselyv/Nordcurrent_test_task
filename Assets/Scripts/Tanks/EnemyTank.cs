using System;
using UnityEngine;

public class EnemyTank : Tank
{
    public event Action<Tank> TankDestroyed;

    public BoxCollider2D BoxCollider2D => _boxCollider2D;

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _turnSpeed = 45f;
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private BoxCollider2D _boxCollider2D;

    private ITankAI _tankAI;

    public void OnConstruct(ITankAI tankAI)
    {
        _tankAI = tankAI;
    }

    private void Update()
    {
        _tankAI.UpdateMovement(this);
    }

    public override void MoveTo(Vector2 direction)
    {
        Vector2 newPosition = _rb2D.position + _moveSpeed * Time.fixedDeltaTime * direction;
        _rb2D.MovePosition(newPosition);
    }

    public override void RotateTowards(Vector2 targetDirection)
    {
        float rotationAmount = targetDirection.x * _turnSpeed * Time.deltaTime;
        _rb2D.rotation += rotationAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameTags.ENEMY_TANK) ||
            collision.collider.CompareTag(GameTags.FIELD_BOUNDARIES))
        {
            _tankAI.HandleCollision(transform.position, collision);
        }
        else if(collision.collider.CompareTag(GameTags.PROJECTILE))
        {
            TankDestroyed?.Invoke(this);
        }
    }
}