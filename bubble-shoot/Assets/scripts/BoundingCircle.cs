using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingCircle
{
    public Vector2 centerPoint;
    public float radius;

    public BoundingCircle(Vector2 centerPoint, float radius)
    {
        this.centerPoint = centerPoint;
        this.radius = radius;
    }
    public bool Intersects(BoundingCircle othercircle)
    {

        if (othercircle == null)
        {
            Debug.LogError("BoundingCircle.Intersects() called with null othercircle");
            return false;
        }
        Vector2 VectorToOther = othercircle.centerPoint - centerPoint;
        float combinedRadiusSq = (othercircle.radius + radius);
        combinedRadiusSq *= combinedRadiusSq;
        MyVector2 vectorToOther = new MyVector2(VectorToOther.x, VectorToOther.y);

        return vectorToOther.V2_LengthSq() < combinedRadiusSq;
    }
}
