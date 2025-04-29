using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class myMatrix4x4
{
    public float[,] values;
    public static myMatrix4x4 Identity
    {
        get
        {
            return new myMatrix4x4(
                new Vector4(1,0,0,0), new Vector4(0,1,0,0),
                new Vector4(0,0,1,0), new Vector4(0,0,0,1));
        }
    }
    public myMatrix4x4(Vector4 column1, Vector4 column2, Vector4 column3, Vector4 column4)
    {
        values = new float[4, 4];

        values[0, 0] = column1.x;
        values[1, 0] = column1.y;
        values[2, 0] = column1.z;
        values[3, 0] = column1.w;

        values[0, 1] = column2.x;
        values[1, 1] = column2.y;
        values[2, 1] = column2.z;
        values[3, 1] = column2.w;

        values[0, 2] = column3.x;
        values[1, 2] = column3.y;
        values[2, 2] = column3.z;
        values[3, 2] = column3.w;

        values[0, 3] = column4.x;
        values[1, 3] = column4.y;
        values[2, 3] = column4.z;
        values[3, 3] = column4.w;
    }
    public myMatrix4x4(Vector3 column1, Vector3 column2, Vector3 column3, Vector3 column4)
    {
        values = new float[4, 4];

        values[0, 0] = column1.x;
        values[1, 0] = column1.y;
        values[2, 0] = column1.z;
        values[3, 0] = 0f;

        values[0, 1] = column2.x;
        values[1, 1] = column2.y;
        values[2, 1] = column2.z;
        values[3, 1] = 0f;

        values[0, 2] = column3.x;
        values[1, 2] = column3.y;
        values[2, 2] = column3.z;
        values[3, 2] = 0f;

        values[0, 3] = column4.x;
        values[1, 3] = column4.y;
        values[2, 3] = column4.z;
        values[3, 3] = 1f;
    }

    public static Vector4 operator *(myMatrix4x4 lhs, Vector4 vector)
    {
        Vector4 rv = new Vector4();
        rv.x = lhs.values[0, 0] * vector.x + lhs.values[0, 1] * vector.y + lhs.values[0, 2] * vector.z + lhs.values[0, 3] * vector.w;
        rv.y = lhs.values[1, 0] * vector.x + lhs.values[1, 1] * vector.y + lhs.values[1, 2] * vector.z + lhs.values[1, 3] * vector.w;
        rv.z = lhs.values[2, 0] * vector.x + lhs.values[2, 1] * vector.y + lhs.values[2, 2] * vector.z + lhs.values[2, 3] * vector.w;
        rv.w = lhs.values[3, 0] * vector.x + lhs.values[3, 1] * vector.y + lhs.values[3, 2] * vector.z + lhs.values[3, 3] * vector.w;


        return rv;
    }

    public static myMatrix4x4 CreateTranslation2D(float x, float y)
    {
        return new myMatrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(x, y, 0, 1)
        );
    }

    public static myMatrix4x4 CreateRotation2D(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new myMatrix4x4(
            new Vector4(cos, sin, 0, 0),
            new Vector4(-sin, cos, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1)
        );
    }

    public static myMatrix4x4 CreateScale2D(float scaleX, float scaleY)
    {
        return new myMatrix4x4(
            new Vector4(scaleX, 0, 0, 0),
            new Vector4(0, scaleY, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1)
        );
    }

}
