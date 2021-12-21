using UnityEngine;

public static class HorizontalProjectionExtension
{
    public static Vector3 HorizontalProjection(this Vector3 v)
    {
        var u = v;
        u.y = 0;
        return u;
    }
}