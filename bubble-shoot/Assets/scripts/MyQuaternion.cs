using UnityEngine;

public static class MyQuaternion
{
    ///<summary>Creates a Quaternion representing a rotation around the Z axis by the given angle in degrees.</summary>
    public static Quaternion EulerZ(float zDegrees)
    {
        float radians = zDegrees * Mathf.Deg2Rad;
        float half = radians * 0.5f;
        float sin = Mathf.Sin(half);
        float cos = Mathf.Cos(half);
        return new Quaternion(0f, 0f, sin, cos);
    }

    ///<summary>Creates a Quaternion representing any rotation around the X Y or Z axis by the given angle in degrees.</summary>
    public static Quaternion EulerXYZ(float xDegrees, float yDegrees, float zDegrees)
    {
        float xRad = xDegrees * Mathf.Deg2Rad;
        float yRad = yDegrees * Mathf.Deg2Rad;
        float zRad = zDegrees * Mathf.Deg2Rad;

        float cx = Mathf.Cos(xRad * 0.5f);
        float sx = Mathf.Sin(xRad * 0.5f);
        float cy = Mathf.Cos(yRad * 0.5f);
        float sy = Mathf.Sin(yRad * 0.5f);
        float cz = Mathf.Cos(zRad * 0.5f);
        float sz = Mathf.Sin(zRad * 0.5f);

        float w = cx * cy * cz + sx * sy * sz;
        float x = sx * cy * cz - cx * sy * sz;
        float y = cx * sy * cz + sx * cy * sz;
        float z = cx * cy * sz - sx * sy * cz;

        return new Quaternion(x, y, z, w);
    }
}
