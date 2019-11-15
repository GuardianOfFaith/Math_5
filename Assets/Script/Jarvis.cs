using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jarvis : MonoBehaviour
{
    public Text text;
    public List<GameObject> jarvisList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (PointHandler.pointList.Count < 3)
            text.color = new Color(0.8f, 0, 0, 1);
        else
        {
            text.color = new Color(0.2f, 0.2f, 0.2f, 1);
            if (Input.GetKeyDown("j"))
                jarvisWalk();
        }

    }

    void jarvisWalk()
    {
        jarvisList.Clear();
        jarvisList.Add(findFirstPoint());
        jarvisList[0].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        bool isOK = false;
        int i = 0;
        while (!isOK && i < 5)
        {
            i++;
            GameObject next = findNextPoint(jarvisList[jarvisList.Count - 1]);
            next.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            if (next == jarvisList[0])
                isOK = true;
            else
                jarvisList.Add(next);
        }
    }

#region jarvisFunction
    GameObject findFirstPoint()
    {
        GameObject point = PointHandler.pointList[0];
        foreach(GameObject p in PointHandler.pointList)
        {
            if (point.transform.position.x > p.transform.position.x)
                point = p;
            else if (point.transform.position.x == p.transform.position.x && p.transform.position.y > p.transform.position.y)
                point = p;
        }
        return point;
    }

    GameObject findNextPoint(GameObject go)
    {
        GameObject point = go;
        float angle;
        float lastAngle = 360;
        foreach(GameObject g in PointHandler.pointList)
        {
            if (g != go)
            {
                angle = Vector2.Angle(new Vector2(go.transform.position.x, go.transform.position.y), new Vector2(g.transform.position.x, g.transform.position.y));
                Debug.Log(angle);
                if (angle < lastAngle)
                {
                    point = g;
                }
            }
        }
        return point;
    }
#endregion
}
