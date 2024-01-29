using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITankInputHandler
{
    Vector2 GetRotationInput();
    Vector2 GetMovementInput();
}
