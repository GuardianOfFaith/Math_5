using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratePoint : MonoBehaviour
{
    public GameObject Point;
    public InputField quantity;
    public bool is3D;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            int i = 20;
            int.TryParse(quantity.text, out i);
            if (i <= 0) { 
                i = 20;
                quantity.text = i.ToString();
            }
            if(is3D) Generate3D(i);
            else Generate2D(i);
        }
    }

    void Generate2D(int num)
    {
        PointHandler.clear();
        List<GameObject> pointList = new List<GameObject>();
        for (int i = 0; i < num; i++)
        {
            pointList.Add(Instantiate(Point, new Vector3(Random.Range(-8.5f,8.5f),Random.Range(-4.5f,4.5f),0), Quaternion.identity));
        }
        PointHandler.pointList = pointList;
    }

    public void Generate3D(int num) {
        PointHandler.clear();
        List<GameObject> pointList = new List<GameObject>();
        for (int i = 0; i < num; i++)
        {
            pointList.Add(Instantiate(Point, new Vector3(Random.Range(-8.5f, 8.5f), Random.Range(-4.5f, 4.5f), Random.Range(-4.5f, 4.5f)), Quaternion.identity));
        }
        PointHandler.pointList = pointList;
    }
}
