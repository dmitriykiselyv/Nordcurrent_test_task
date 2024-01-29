using UnityEngine;

public abstract class Tank : MonoBehaviour
{
    public abstract void MoveTo(Vector2 direction);
    public abstract void RotateTowards(Vector2 targetDirection);
}
