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
        // Quaternion(x, y, z, w) for rotation around Z axis
        return new Quaternion(0f, 0f, sin, cos);
    }
}
