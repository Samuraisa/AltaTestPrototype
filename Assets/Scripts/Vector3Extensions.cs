using UnityEngine;

public static class Vector3Extensions
{
    public static Vector2 ToVector2XZ(this Vector3 v) => new Vector2(v.x, v.z);
    
    public static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);
    public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
    public static Vector3 WithZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);
}
