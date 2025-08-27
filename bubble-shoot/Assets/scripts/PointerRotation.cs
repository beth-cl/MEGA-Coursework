using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerRotation : MonoBehaviour
{
    GameObject pivot;

    void Start()
    {
        pivot = GameObject.Find("pivot");
    }

    void Update()
    {
        if (pivot != null)
        {
            PointToMouse();
        }
    }

    ///<summary> function to point to the mouse using custom matrix rotation </summary>
    public void PointToMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 objectPos = pivot.transform.position;
        MyVector2 direction = new MyVector2(mouseWorldPos.x - objectPos.x, mouseWorldPos.y - objectPos.y);

        float angleToMouse = MyVector2.VectorToRadians(direction) * Mathf.Rad2Deg;
        if (angleToMouse < 0)
        {
            angleToMouse += 180;
        }
        //pivot.transform.rotation = MyQuaternion.EulerZ(angleToMouse);
        pivot.transform.rotation = MyQuaternion.EulerXYZ(0f, 0f, angleToMouse);
    }
}
