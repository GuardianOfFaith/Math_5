using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratePoint : MonoBehaviour
{
    public GameObject Point;
    public InputField quantity;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            int i = 20;
            int.TryParse(quantity.text, out i);
            if (i <= 0)
                i = 20;
            Generate(i);
        }
    }

    void Generate(int num)
    {
        PointHandler.clear();
        List<GameObject> pointList = new List<GameObject>();
        for (int i = 0; i < num; i++)
        {
            pointList.Add(Instantiate(Point, new Vector3(Random.Range(-8.5f,8.5f),Random.Range(-4.5f,4.5f),0), Quaternion.identity));
        }
        PointHandler.pointList = pointList;
    }
}
