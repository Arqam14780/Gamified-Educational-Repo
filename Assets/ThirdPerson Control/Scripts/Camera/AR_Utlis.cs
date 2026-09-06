using UnityEngine;

public static class AR_Utlis
{
    private const float MAX_DELTA_TIME = 0.5f;
    private const float MAX_LEFT_FACTOR = 0.75f;

    public static float realDeltaTime
    { get { return ((Time.captureFramerate <= 0) ? Time.unscaledDeltaTime : (1.0f / (float)Time.captureFramerate)); } }
    static public float realDeltaTimeClamped
    { get { return Mathf.Min(AR_Utlis.realDeltaTime, MAX_DELTA_TIME); } }


    public static float SmoothTowards(float a, float b, float smoothingTime, float deltaTime, float epsilon, float maxLeftFactor = MAX_LEFT_FACTOR)
    {
        if (Mathf.Abs(b - a) <= epsilon)
            return b;
        return ((smoothingTime < 0.001f)) ? b : Mathf.Lerp(a, b, GetLerpFactor(smoothingTime, deltaTime, maxLeftFactor));
    }
    private static float GetLerpFactor(float smoothingTime, float deltaTime, float maxLerpFactor)
    {
        return Mathf.Min(maxLerpFactor, ((smoothingTime <= deltaTime) ? 1 : (deltaTime / smoothingTime)));
    }

}
