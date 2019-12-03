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
            Object.Destroy(go);
        }
        pointList.Clear();
    }
}
