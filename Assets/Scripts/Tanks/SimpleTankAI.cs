using UnityEngine;

public class SimpleTankAI : ITankAI
{
    private SpriteRenderer _fieldBoundaries;
    private Vector2 _targetPosition;
    private float _turnThreshold = 2f;
    private float _minimumDistanceToTarget = 5f;
    private float _maxFieldWidth;
    private float _maxFieldHeight;
    private float _offset = 5;

    public SimpleTankAI(SpriteRenderer fieldBoundaries)
    {
        _turnThreshold = Random.Range(2f, 3f);
        _minimumDistanceToTarget = Random.Range(3f, 7f);
        _fieldBoundaries = fieldBoundaries;

        float x = Random.Range(_fieldBoundaries.bounds.min.x, _fieldBoundaries.bounds.max.x);
        float y = Random.Range(_fieldBoundaries.bounds.min.y, _fieldBoundaries.bounds.max.y);
        _targetPosition = new Vector2(x, y);

        _maxFieldWidth = _fieldBoundaries.bounds.size.x;
        _maxFieldHeight = _fieldBoundaries.bounds.size.y;
    }

    public void UpdateMovement(Tank tank)
    {
        Vector2 directionToTarget = (_targetPosition - (Vector2)tank.transform.position).normalized;
        float distanceToTarget = Vector2.Distance(tank.transform.position, _targetPosition);
        float angleToTarget = Vector2.SignedAngle(tank.transform.right, directionToTarget);

        if (distanceToTarget < _minimumDistanceToTarget)
        {
            ChangeTargetPosition();
        }
        else
        {
            if (Mathf.Abs(angleToTarget) > _turnThreshold)
            {
                Vector2 rotationDirection = new Vector2(Mathf.Sign(angleToTarget), 0);
                tank.RotateTowards(rotationDirection);
            }
            else
            {
                tank.MoveTo(directionToTarget);
            }
        }
    }

    public void HandleCollision(Vector2 currentPosition, Collision2D collision)
    {
        Vector2 collisionNormal = collision.GetContact(0).normal;
        float newAngle = Vector2.SignedAngle(Vector2.up, collisionNormal) + 90f;
        Vector2 newDirection = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));

        float randomDistance = Random.Range(0f, Mathf.Max(_maxFieldWidth, _maxFieldHeight));
        _targetPosition = currentPosition + newDirection.normalized * randomDistance;
    }

    private void ChangeTargetPosition()
    {
        float x = Random.Range(_fieldBoundaries.bounds.min.x - _offset, _fieldBoundaries.bounds.max.x + _offset);
        float y = Random.Range(_fieldBoundaries.bounds.min.y - _offset, _fieldBoundaries.bounds.max.y + _offset);
        _targetPosition = new Vector2(x, y);
    }


}
