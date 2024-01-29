using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITankAI
{
    void UpdateMovement(Tank tank);
    void HandleCollision(Vector2 currentPosition, Collision2D collision);
}
