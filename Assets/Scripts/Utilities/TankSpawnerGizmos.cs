using System.Collections.Generic;
using UnityEngine;

public class TankSpawnerGizmos : MonoBehaviour
{
    public static List<Vector3> Positions = new List<Vector3>();
    public static List<Vector2> Sizes = new List<Vector2>();
    public static List<float> Rotations = new List<float>();

    private void Awake()
    {
        ClearData();
    }

    private void OnDestroy()
    {
        ClearData();
    }

    public static void UpdateData(Vector3 positions, BoxCollider2D collider2D, float rotation)
    {
        Positions.Add(positions);
        Sizes.Add(collider2D.size);
        Rotations.Add(rotation);
    }

    public static void ClearData()
    {
        Positions.Clear();
        Sizes.Clear();
        Rotations.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < Positions.Count; i++)
        {
            Vector3 pos = Positions[i];
            Vector2 size = Sizes[i];
            float rotationAngle = Rotations[i];

            Vector2 offset = new Vector2(0.23f, 0f); //TODO

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                pos,
                Quaternion.Euler(0f, 0f, rotationAngle),
                Vector3.one 
            );
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(offset, new Vector3(size.x, size.y, 1));
            Gizmos.matrix = Matrix4x4.identity;
        }
    }

}
