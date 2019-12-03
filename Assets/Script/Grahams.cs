using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grahams : MonoBehaviour
{

    public Text text;
    public List<GameObject> grahamList = new List<GameObject>();
    public List<GameObject> sortedList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (PointHandler.pointList.Count < 3)
            text.color = new Color(0.8f, 0, 0, 1);
        else
        {
            text.color = new Color(0.2f, 0.2f, 0.2f, 1);
            if (Input.GetKeyDown("s"))
                grahamsscan();
        }
    }

    void grahamsscan()
    {
        StartCoroutine(graham());
    }

    IEnumerator graham()
    {
        grahamList.Clear();
        grahamList.Add(findFirstPoint());
        grahamList[0].GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
        yield return StartCoroutine(sort());
        yield return StartCoroutine(hulldraw());
    }

    GameObject findFirstPoint()
    {
        GameObject point = PointHandler.pointList[0];
        foreach(GameObject p in PointHandler.pointList)
        {
            if (point.transform.position.y > p.transform.position.y)
                point = p;
            else if (point.transform.position.y == p.transform.position.y && p.transform.position.x > p.transform.position.x)
                point = p;
        }
        return point;
    }

    IEnumerator sort()
    {
        sortedList.Clear();
        sortedList.Add(grahamList[0]);

        GameObject point = new GameObject();
        float angle;
        float lastAngle = 360;
        while (sortedList.Count < PointHandler.pointList.Count)
        {
            lastAngle = 360;
            foreach (GameObject g in PointHandler.pointList)
            {
                //Color tmp = g.GetComponent<SpriteRenderer>().color;
                //g.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
                //yield return new WaitForSeconds(0.5f);
                //g.GetComponent<SpriteRenderer>().color = tmp;

                if (!sortedList.Contains(g))
                {
                    g.GetComponent<SpriteRenderer>().color = new Color(0, 1, 1, 1);
                    angle = Vector2.Angle(new Vector2(1, 0), new Vector2(g.transform.position.x - sortedList[0].transform.position.x, g.transform.position.y - sortedList[0].transform.position.y));

                    if (angle < lastAngle && angle != 0)
                    {
                        //Debug.Log(angle);
                        lastAngle = angle;
                        point = g;
                    }
                    yield return new WaitForEndOfFrame();
                    point.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
                }
            }
            sortedList.Add(point);
            //point.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        }
    }

    IEnumerator hulldraw()
    {
        GameObject line = new GameObject();
        int i = 0;
        line.AddComponent<LineRenderer>().SetPosition(i, grahamList[i].transform.position);
        sortedList.RemoveAt(0);
        grahamList.Add(sortedList[0]);
        ++i;
        sortedList.RemoveAt(0);
        line.GetComponent<LineRenderer>().SetPosition(i, grahamList[i].transform.position);
        line.GetComponent<LineRenderer>().startWidth = 0.1f;
        line.GetComponent<LineRenderer>().endWidth = 0.1f;
        while (sortedList.Count > 0)
        {
            float angle = (grahamList[grahamList.Count - 1].transform.position.x - grahamList[grahamList.Count - 2].transform.position.x) * (sortedList[0].transform.position.y - grahamList[grahamList.Count - 2].transform.position.y) - (sortedList[0].transform.position.x - grahamList[grahamList.Count - 2].transform.position.x) * (grahamList[grahamList.Count - 1].transform.position.y - grahamList[grahamList.Count - 2].transform.position.y);
            Debug.Log(angle);
            sortedList[0].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
            yield return new WaitForSeconds(1);
            if (angle >= 0)
            {
                grahamList.Add(sortedList[0]);
                //++i;
                sortedList.RemoveAt(0);
                line.GetComponent<LineRenderer>().positionCount = line.GetComponent<LineRenderer>().positionCount + 1;
                line.GetComponent<LineRenderer>().SetPosition(grahamList.Count - 1, grahamList[grahamList.Count - 1].transform.position);
                Debug.Log("bite");
            }
            else
            {
                sortedList[0].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
                grahamList.RemoveAt(grahamList.Count - 1);
                grahamList.Add(sortedList[0]);
                sortedList[0] = grahamList[grahamList.Count - 1];
                line.GetComponent<LineRenderer>().positionCount = line.GetComponent<LineRenderer>().positionCount - 1;
                grahamList.RemoveAt(grahamList.Count - 1);

            }
            yield return new WaitForSeconds(1);
        }
        line.GetComponent<LineRenderer>().positionCount = line.GetComponent<LineRenderer>().positionCount + 1;
        line.GetComponent<LineRenderer>().SetPosition(grahamList.Count, grahamList[0].transform.position);
    }
}
