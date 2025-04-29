using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingCircle
{
    public Vector3 centerPoint;
    public float radius;

    public BoundingCircle(Vector3 centerPoint, float radius)
    {
        this.centerPoint = centerPoint;
        this.radius = radius;
    }
    public bool Intersects(BoundingCircle othercircle)
    {
        Vector2 VectorToOther = othercircle.centerPoint - centerPoint;
        float combinedRadiusSq = (othercircle.radius + radius);
        combinedRadiusSq *= combinedRadiusSq;
        MyVector2 vectorToOther = new MyVector2(VectorToOther.x, VectorToOther.y);

        return vectorToOther.V2_LengthSq() < combinedRadiusSq;
    }
}
