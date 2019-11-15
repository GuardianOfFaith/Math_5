using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointHandler
{
    public static List<GameObject> pointList = new List<GameObject>();

    public static void clear()
    {
        foreach(GameObject go in pointList)
        {
            MonoBehaviour.Destroy(go);
        }
        pointList.Clear();
    }
}
