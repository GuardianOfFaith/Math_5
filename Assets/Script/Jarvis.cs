using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jarvis : MonoBehaviour
{
    public float speed = 1;

    public Text text;
    public List<GameObject> jarvisList = new List<GameObject>();

    [SerializeField]
    private GameObject linePrefab;
    private GameObject line;
    private GameObject line2;
    private GameObject linetest;
    private bool running = false;

    // Update is called once per frame
    void Update()
    {
        if (PointHandler.pointList.Count < 3)
            text.color = new Color(0.8f, 0, 0, 1);
        else
        {
            text.color = new Color(0.2f, 0.2f, 0.2f, 1);
            if (Input.GetKeyDown("j") && !running)
                jarvisWalk();
        }
    }

    void jarvisWalk()
    {
        StartCoroutine(jarvis());
    }

    #region jarvisFunction
    GameObject findFirstPoint()
    {
        GameObject point = PointHandler.pointList[0];
        foreach(GameObject p in PointHandler.pointList)
        {
            if (point.transform.position.x > p.transform.position.x)
            {
                point = p;
            }
            else if (point.transform.position.x == p.transform.position.x && p.transform.position.y > p.transform.position.y)
            {
                point = p;
            }
        }
        return point;
    }

    //GameObject findNextPoint(GameObject go)
    //{
    //    GameObject point = go;
    //    GameObject line = new GameObject();
    //    line.AddComponent<LineRenderer>().SetPosition(0, point.transform.position);
    //    float angle;
    //    float lastAngle = 360;
    //    foreach(GameObject g in PointHandler.pointList)
    //    {
    //        if (g != go)
    //        {
    //            angle = Vector2.SignedAngle(new Vector2(go.transform.position.x, go.transform.position.y), new Vector2(g.transform.position.x, g.transform.position.y));
    //            if (angle < 0)
    //                angle += 360;
    //            Debug.Log(angle);
    //            if (angle < lastAngle && angle > 0)
    //            {
    //                lastAngle = angle;
    //                point = g;
    //            }
    //        }
    //    }
    //    line.GetComponent<LineRenderer>().SetPosition(1, point.transform.position);
    //    line.GetComponent<LineRenderer>().widthMultiplier = 0.1f;
    //    return point;
    //}
#endregion

    IEnumerator jarvis()
    {
        running = true;
        if (line != null) { Destroy(line);}
        line = Instantiate(linePrefab);
        line2 = Instantiate(linePrefab);
        line2.GetComponent<LineRenderer>().startColor = Color.white;
        line2.GetComponent<LineRenderer>().endColor = Color.white;
        linetest = Instantiate(linePrefab);
        linetest.GetComponent<LineRenderer>().startColor = Color.yellow;
        linetest.GetComponent<LineRenderer>().endColor = Color.yellow;
        jarvisList.Clear();
        jarvisList.Add(findFirstPoint());
        line.GetComponent<LineRenderer>().SetPosition(0, jarvisList[0].transform.position);
        line.GetComponent<LineRenderer>().SetPosition(1, jarvisList[0].transform.position);
        jarvisList[0].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);

        bool isOK = false;
        while (!isOK)
        {
            GameObject next = new GameObject();
            Destroy(next, 2.5f * jarvisList.Count * speed);
            yield return StartCoroutine(pointFinder(jarvisList[jarvisList.Count - 1], b => next = b));
            next.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
            if (next == jarvisList[0])
            {
                isOK = true;
                line.GetComponent<LineRenderer>().positionCount = jarvisList.Count+1;
                line.GetComponent<LineRenderer>().SetPosition(jarvisList.Count, next.transform.position);
                Destroy(line2);
                Destroy(linetest);
            }
            else
            {
                jarvisList.Add(next);
                line.GetComponent<LineRenderer>().positionCount = jarvisList.Count;
                line.GetComponent<LineRenderer>().SetPosition(jarvisList.Count - 1, next.transform.position);
            }
        }
        running = false;
    }

    IEnumerator pointFinder(GameObject go, Action<GameObject> b)
    {
        GameObject point = go;
        linetest.GetComponent<LineRenderer>().SetPosition(0, go.transform.position);
        line2.GetComponent<LineRenderer>().SetPosition(0, go.transform.position);
        float angle;
        float lastAngle = 360;

        //Just for display
        foreach (GameObject g in PointHandler.pointList)
            if (!jarvisList.Contains(g))
                g.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        foreach (GameObject g in PointHandler.pointList)
        {
            linetest.GetComponent<LineRenderer>().SetPosition(1,g.transform.position);
            if (g != go)
            {
                #region ProgessViewer
                if (!jarvisList.Contains(g))
                    g.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
                #endregion
                if (jarvisList.Count <= 1)
                    angle = Vector2.Angle(new Vector2(0, 1), new Vector2(g.transform.position.x - go.transform.position.x, g.transform.position.y - go.transform.position.y));
                else
                    angle = Vector2.Angle(new Vector2(go.transform.position.x - jarvisList[jarvisList.Count - 2].transform.position.x, go.transform.position.y - jarvisList[jarvisList.Count - 2].transform.position.y), new Vector2(g.transform.position.x - go.transform.position.x, g.transform.position.y - go.transform.position.y));
            
                if (angle < lastAngle && angle != 0)
                {
                    //Point Rejected
                    #region ProgressViewer 
                    if (!jarvisList.Contains(point))
                        point.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
                    #endregion

                    lastAngle = angle;
                    point = g;
                    #region ProgressViewer 
                    if (!jarvisList.Contains(point))
                        point.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
                    #endregion
                }
                else
                {
                    //Point Rejected
                    #region ProgressViewer 
                    if (!jarvisList.Contains(g))
                        g.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
                    #endregion
                }
                line2.GetComponent<LineRenderer>().SetPosition(1, point.transform.position);

                yield return new WaitForSeconds(speed);
            }
        }
        b(point);
    }
}
