using System;
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
        StartCoroutine(jarvis());
        //jarvisList.Clear();
        //jarvisList.Add(findFirstPoint());
        //jarvisList[0].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        //bool isOK = false;
        //int i = 0;
        //while (!isOK && i < 20)
        //{
        //    i++;
        //    GameObject next = findNextPoint(jarvisList[jarvisList.Count - 1]);
        //    next.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        //    if (next == jarvisList[0])
        //        isOK = true;
        //    else
        //        jarvisList.Add(next);
        //    Debug.LogWarning(i);
        //}
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
        GameObject line = new GameObject();
        line.AddComponent<LineRenderer>().SetPosition(0, point.transform.position);
        float angle;
        float lastAngle = 360;
        foreach(GameObject g in PointHandler.pointList)
        {
            if (g != go)
            {
                angle = Vector2.SignedAngle(new Vector2(go.transform.position.x, go.transform.position.y), new Vector2(g.transform.position.x, g.transform.position.y));
                if (angle < 0)
                    angle += 360;
                Debug.Log(angle);
                if (angle < lastAngle && angle > 0)
                {
                    lastAngle = angle;
                    point = g;
                }
            }
        }
        line.GetComponent<LineRenderer>().SetPosition(1, point.transform.position);
        line.GetComponent<LineRenderer>().widthMultiplier = 0.1f;
        return point;
    }
#endregion


    //TEST
    IEnumerator jarvis()
    {
        jarvisList.Clear();
        jarvisList.Add(findFirstPoint());
        jarvisList[0].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        bool isOK = false;
        int i = 0;
        while (!isOK && i < 20)
        {
            i++;
            GameObject next = new GameObject();
            yield return StartCoroutine(pointFinder(jarvisList[jarvisList.Count - 1], b => next = b));
            next.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            if (next == jarvisList[0])
                isOK = true;
            else
                jarvisList.Add(next);
            //yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator pointFinder(GameObject go, Action<GameObject> b)
    {
        GameObject point = go;
        GameObject line = new GameObject();
        line.AddComponent<LineRenderer>().SetPosition(0, point.transform.position);
        line.GetComponent<LineRenderer>().widthMultiplier = 0.1f;
        float angle;
        float lastAngle = 360;
        foreach (GameObject g in PointHandler.pointList)
        {
            if (g != go)
            {
                g.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
                //angle = Vector2.SignedAngle(new Vector2(go.transform.position.x, go.transform.position.y), new Vector2(g.transform.position.x, g.transform.position.y));
                if (jarvisList.Count <= 1)
                    angle = Vector2.Angle(new Vector2(0, 1), new Vector2(g.transform.position.x - go.transform.position.x, g.transform.position.y - go.transform.position.y));
                else
                    angle = Vector2.Angle(new Vector2(go.transform.position.x - jarvisList[jarvisList.Count - 2].transform.position.x, go.transform.position.y - jarvisList[jarvisList.Count - 2].transform.position.y), new Vector2(g.transform.position.x - go.transform.position.x, g.transform.position.y - go.transform.position.y));
            
                if (angle < lastAngle && angle != 0)
                {
                    Debug.Log(angle);
                    lastAngle = angle;
                    point = g;
                }
                else if (angle == 0)
                {
                    Debug.Log("error" + angle.ToString());
                }
                line.GetComponent<LineRenderer>().SetPosition(1, point.transform.position);
                yield return new WaitForEndOfFrame();
                g.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }

        b(point);
    }
}
