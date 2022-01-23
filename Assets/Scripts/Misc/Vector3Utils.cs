using UnityEngine;

public static class Vector3Utils
{
    public static Vector3 Sign(this Vector3 v)
    {
        return new Vector3(Mathf.Sign(v.x), Mathf.Sign(v.y), Mathf.Sign(v.y));
    }
}