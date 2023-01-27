using System.Collections;
using UnityEngine;

public class MonoHelper : MonoBehaviour
{
    public static void Error(string str)
    {
        UnityEngine.Debug.LogError(str);
    }

    public static void Debug(string str)
    {
        UnityEngine.Debug.Log(str);
    }
}

public static class MonoExpand
{
    public static Vector3 ToVector3(this RVector3 rv)
    {
        return new Vector3(rv.x, rv.y, rv.z);
    }

    public static RVector3 ToRVector3(this Vector3 v)
    {
        return new RVector3(v.x, v.y, v.z);
    }
}

