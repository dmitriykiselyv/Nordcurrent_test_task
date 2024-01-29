using System;
using UnityEngine;

public class PlayerTank : Tank
{
    public event Action<Tank> TankDestroyed;
    public BoxCollider2D BoxCollider2D => _boxCollider2D;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float turnSpeed = 80f;
    [SerializeField] private Rigidbody2D _rb2D;
    [SerializeField] private BoxCollider2D _boxCollider2D;

    private ITankInputHandler _inputHandler;

    public void OnConstruct()
    {
        _inputHandler = new PlayerTankInputHandler();
    }

    private void Update()
    {
        var rotationInput = _inputHandler.GetRotationInput();
        var movementInput = _inputHandler.GetMovementInput();

        RotateTowards(rotationInput);
        MoveTo(movementInput);
    }

    public override void MoveTo(Vector2 direction)
    {
        float angleRad = _rb2D.rotation * Mathf.Deg2Rad;
        Vector2 movementDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        Vector2 moveDirection = movementDirection.normalized * direction.y;
        Vector2 newPosition = _rb2D.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        _rb2D.MovePosition(newPosition);
    }

    public override void RotateTowards(Vector2 rotationDirection)
    {
        _rb2D.rotation += rotationDirection.x * Time.deltaTime * turnSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameTags.ENEMY_TANK))
        {
            gameObject.SetActive(false);
            TankDestroyed?.Invoke(this);
        }
    }
}
