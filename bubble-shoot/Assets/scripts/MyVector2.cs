using UnityEngine;

public class MyVector2
{
    public float x, y;


    // WORKSHOP 2

    public MyVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    //Create a static function that takes in two MyVector2 objects, adds them together, and returns the resulting vector
    public static MyVector2 AddingVectors2(MyVector2 vectorA, MyVector2 vectorB)
    {
        MyVector2 vectorC = new MyVector2(0f, 0f);

        vectorC.x = vectorA.x + vectorB.x;
        vectorC.y = vectorA.y + vectorB.y;

        return vectorC;
    }

    //Create a static function that takes in two MyVector2 objects, and subtracts them, and returns the resulting vector
    public static MyVector2 SubtractingVector2(MyVector2 vectorA, MyVector2 vectorB)
    {
        MyVector2 vectorC = new MyVector2(0f, 0f);

        vectorC.x = vectorA.x - vectorB.x;
        vectorC.y = vectorA.y - vectorB.y;

        return vectorC;
    }

    public UnityEngine.Vector2 ToUnityVector()
    {
        UnityEngine.Vector2 vectorC = new UnityEngine.Vector2();

        vectorC.x = x;
        vectorC.y = y;

        return vectorC;
    }

    //Create a static function that takes in one vector and returns the length/magnitude squared
    public float V2_LengthSq()
    {
        float rv;
        rv = x * x + y * y;
        return rv;

    }

    //innefficient due to use of sqrd but finds the length/magnitude
    public float V2_Length()
    {
        float rv;
        rv = Mathf.Sqrt(x * x + y * y);
        return rv;

    }

    //Create a static function that takes in a vector and a scalar and returns a new vector that has been multiplied by the scalar
    public static MyVector2 Scaling_Vectors(MyVector2 vectorA, float scalar)
    {
        MyVector2 vector_scaled = new MyVector2(0f, 0f);

        vector_scaled.x = vectorA.x * scalar;
        vector_scaled.y = vectorA.y * scalar;

        return vector_scaled;

    }

    //Create a static function that takes in a vector and a divisor and returns a new vector that has been divided by the divisor
    public static MyVector2 Dividing_Vectors(MyVector2 vectorA, float divisor)
    {
        MyVector2 vector_divided = new MyVector2(0f, 0f);

        vector_divided.x = vectorA.x / divisor;
        vector_divided.y = vectorA.y / divisor;

        return vector_divided;
    }

    //Create a static function that takes in one vector and returns that vector normalized. You will need do step 3 before you can do this.
    public static MyVector2 Normalising_Vectors(MyVector2 vectorA)
    {
        MyVector2 vector_normalised = new MyVector2(0f, 0f);

        vector_normalised = Dividing_Vectors(vectorA, vectorA.V2_Length());

        return vector_normalised;
    }
    public static float DotProduct(MyVector2 vectorA, MyVector2 vectorB, bool should_normalise = true)
    {
        float dotProduct = 0f;

        MyVector2 vectorA_normalised = new MyVector2(0f, 0f);
        MyVector2 vectorB_normalised = new MyVector2(0f, 0f);

        if (should_normalise)
        {
            vectorA_normalised = Normalising_Vectors(vectorA);
            vectorB_normalised = Normalising_Vectors(vectorB);
        }

        dotProduct = (vectorA_normalised.x * vectorB_normalised.x + vectorA_normalised.y * vectorB_normalised.y);

        return dotProduct;
    }

    public static MyVector2 ReflectVector(MyVector2 vector, MyVector2 normal)
    {
        //float dot = DotProduct(vector, normal);// Calculate the dot product of the vector and the normal
        MyVector2 reflected = new MyVector2(vector.x*-1,vector.y);// Calculate the reflection vector
        Debug.Log("velocity " + vector.x + " " + vector.y);
        Debug.Log("reflected velocity " + reflected.x + " "+ reflected.y);

        return reflected;
    }

    public static float VectorToRadians(Vector2 V)
    {
        float rv = 0f;
        rv = Mathf.Atan(V.y / V.x);

        return rv;
    }
    public static Vector2 RadiansToVector(float angle)
    {
        Vector2 rv = new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
        rv.x = Mathf.Cos(angle);
        rv.y = Mathf.Sin(angle);

        return rv;
    }

    public static Vector2 VectorCrossProduct(Vector2 A, Vector2 B)
    {
        Vector2 C = new Vector2();
        C.x = A.y * B.x - A.x * B.y;
        C.y = A.x * B.y - A.y * B.x;

        return C;
    }

    public static Vector2 VecLerp(Vector2 A, Vector2 B, float t)
    {
        return A*(1.0f-t)+B * t;
    }
}
