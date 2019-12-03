using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacePoint : MonoBehaviour
{
    public GameObject refPointPrefab;
    public Text text;

    GameObject pointInCreation;
    bool inputMod = false;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            inputMod = !inputMod;
            if (inputMod)
                text.text = "Press K to exit point input mode";
            else
                text.text = "Press K to enter point input mode";
        }
        if (inputMod)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pointInCreation = Instantiate(refPointPrefab);
                pointInCreation.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y / 50 - 5, 0);
            }
            if (Input.GetMouseButton(0))
            {
                pointInCreation.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            }
            if (Input.GetMouseButtonUp(0))
            {
                PointHandler.pointList.Add(pointInCreation);
                pointInCreation = null;
            }
        }
    }
}
