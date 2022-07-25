using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 GetDirection(this Vector2 from, Vector2 to, bool normalized = true)
    {
        if (normalized)
        {
            return (from - to).normalized;
        }
        else
        {
            return from - to;
        }
    }
}
