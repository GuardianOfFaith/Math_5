using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grahams : MonoBehaviour
{
    public float speed = 1;

    public Text text;
    public List<GameObject> grahamList = new List<GameObject>();
    public List<GameObject> sortedList = new List<GameObject>();

    [SerializeField]
    private GameObject linePrefab;
    private GameObject line;

    private bool running = false;

    // Update is called once per frame
    void Update()
    {
        if (PointHandler.pointList.Count < 3)
            text.color = new Color(0.8f, 0, 0, 1);
        else
        {
            text.color = new Color(0.2f, 0.2f, 0.2f, 1);
            if (Input.GetKeyDown("s") && !running)
                grahamsscan();
        }
    }

    void grahamsscan()
    {
        StartCoroutine(graham());
    }

    IEnumerator graham()
    {
        running = true;
        grahamList.Clear();
        if (line != null) { Destroy(line); }

        //find the pivot
        grahamList.Add(findFirstPoint());

        //Set the Pivot in Blue
        grahamList[0].GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);

        //Sort the list of point
        yield return StartCoroutine(sort());

        //Run the scan to draw the hull
        yield return StartCoroutine(hulldraw());

        running = false;
    }

    //Find the Pivot
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

    //sort the list of point
    IEnumerator sort()
    {
        sortedList.Clear();
        sortedList.Add(grahamList[0]);

        float angle;
        float lastAngle;

        #region SorterRunner
        while (sortedList.Count < PointHandler.pointList.Count)
        {
            //just a trick to optimize the display without making garbage
            GameObject point = new GameObject();
            Destroy(point, speed * 2.5f * PointHandler.pointList.Count);

            lastAngle = 360;
            foreach (GameObject g in PointHandler.pointList)
            {
                if (!sortedList.Contains(g))
                {
                    //Point analyzed in Yellow
                    #region ProgressViewer 
                    Color tmp = g.GetComponent<SpriteRenderer>().color;
                    g.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
                    yield return new WaitForSeconds(speed);
                    g.GetComponent<SpriteRenderer>().color = tmp;
                    #endregion

                    angle = Vector2.Angle(new Vector2(1, 0), new Vector2(g.transform.position.x - sortedList[0].transform.position.x, g.transform.position.y - sortedList[0].transform.position.y));
                    //Debug.Log(angle);
                    if (angle < lastAngle && angle != 0)
                    {
                        lastAngle = angle;

                        //Point Rejected
                        #region ProgressViewer 
                        if (point.GetComponent<SpriteRenderer>() != null)
                            point.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
                        #endregion

                        point = g;
                    }
                    else
                    {
                        //Point Rejected
                        #region ProgressViewer 
                        g.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
                        #endregion
                    }

                    //current point to add to the sorted list
                    #region ProgressViewer
                    point.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
                    yield return new WaitForSeconds(speed);
                    #endregion
                }
            }
            sortedList.Add(point);
        }
        #endregion
        sortedList.Add(grahamList[0]);
    }

    IEnumerator hulldraw()
    {
        //reset Colors of point (except pivot)
        #region ProgressViewer
        foreach (GameObject g in PointHandler.pointList)
        {
            if (!grahamList.Contains(g)) {g.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); }
        }
        #endregion

        //set the line to draw the hull and the 2 first point
        line = Instantiate(linePrefab);
        line.GetComponent<LineRenderer>().SetPosition(0, grahamList[0].transform.position);
        //as the first point of the sorted list is the pivot and the pivot already in grahamlist, we don't need to put it again
        sortedList.RemoveAt(0);
        grahamList.Add(sortedList[0]);
        sortedList.RemoveAt(0);
        line.GetComponent<LineRenderer>().SetPosition(1, grahamList[1].transform.position);

        #region ProgressViewer
        grahamList[1].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        #endregion

        float angle = 0;
        #region runner
        while (sortedList.Count > 0)
        {
            angle = (grahamList[grahamList.Count - 1].transform.position.x - grahamList[grahamList.Count - 2].transform.position.x) * (sortedList[0].transform.position.y - grahamList[grahamList.Count - 2].transform.position.y) - (sortedList[0].transform.position.x - grahamList[grahamList.Count - 2].transform.position.x) * (grahamList[grahamList.Count - 1].transform.position.y - grahamList[grahamList.Count - 2].transform.position.y);
            //Debug.Log(angle);

            //Analyzed point in yellow
            #region ProgressViewer
            sortedList[0].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
            yield return new WaitForSeconds(speed);
            #endregion

            if (angle >= 0)
            {
                grahamList.Add(sortedList[0]);
                //Added point in Green
                #region ProgressViewer
                sortedList[0].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
                yield return new WaitForSeconds(speed);
                #endregion

                //update the hull
                sortedList.RemoveAt(0);
                line.GetComponent<LineRenderer>().positionCount = line.GetComponent<LineRenderer>().positionCount + 1;
                line.GetComponent<LineRenderer>().SetPosition(grahamList.Count - 1, grahamList[grahamList.Count - 1].transform.position);
            }
            else
            {
                //rejected point in red and the new approuved in green
                #region ProgressViewer
                sortedList[0].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
                grahamList[grahamList.Count - 1].GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
                yield return new WaitForSeconds(speed);
                #endregion

                //we back one iteration before so we remove the last saved point from the list and put the new one
                grahamList.RemoveAt(grahamList.Count - 1);
                grahamList.Add(sortedList[0]);
                //we get back to the previous point to check if it still good too
                sortedList[0] = grahamList[grahamList.Count - 1];

                //just to update the hull and don't get a line that go to 0,0,0
                line.GetComponent<LineRenderer>().positionCount = line.GetComponent<LineRenderer>().positionCount - 1;

                grahamList.RemoveAt(grahamList.Count - 1);
            }
        }
        #endregion
    }
}
