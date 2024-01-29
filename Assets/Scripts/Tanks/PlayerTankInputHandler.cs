using UnityEngine;

public class PlayerTankInputHandler : ITankInputHandler
{
    public Vector2 GetRotationInput()
    {
        Vector2 rotation = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            rotation = new Vector2(1, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotation = new Vector2(-1, 0);
        }
        return rotation;
    }

    public Vector2 GetMovementInput()
    {
        Vector2 movement = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movement = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement = Vector2.down;
        }
        return movement;
    }
}