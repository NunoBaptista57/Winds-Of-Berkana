using UnityEngine;

public static class FloatUtils
{
    public static float Intensify(float val, float factor)
    {
        if (val == 0) return 0;
        return Mathf.Sign(val) * Mathf.Pow(Mathf.Abs(val), 1 / factor);
    }
}
