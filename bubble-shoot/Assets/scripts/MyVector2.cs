using UnityEngine;

public class MyVector2
{
    public float x, y;

    ///<summary>Create a constructor that takes in two floats and sets the x and y values to create my own Vector2 class</summary>
    public MyVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    ///<summary>Create a static function that takes in two MyVector2 objects, adds them together, and returns the resulting vector</summary>
    public static MyVector2 AddingVectors2(MyVector2 vectorA, MyVector2 vectorB)
    {
        MyVector2 vectorC = new MyVector2(0f, 0f);

        vectorC.x = vectorA.x + vectorB.x;
        vectorC.y = vectorA.y + vectorB.y;

        return vectorC;
    }

    ///<summary>Create a static function that takes in two MyVector2 objects, and subtracts them, and returns the resulting vector</summary>
    public static MyVector2 SubtractingVector2(MyVector2 vectorA, MyVector2 vectorB)
    {
        MyVector2 vectorC = new MyVector2(0f, 0f);

        vectorC.x = vectorA.x - vectorB.x;
        vectorC.y = vectorA.y - vectorB.y;

        return vectorC;
    }

    ///<summary>Changes MyVector2 back to UnityEngine.Vector2</summary>
    public UnityEngine.Vector2 ToUnityVector()
    {
        UnityEngine.Vector2 vectorC = new UnityEngine.Vector2();

        vectorC.x = x;
        vectorC.y = y;

        return vectorC;
    }

    ///<summary>Create a static function that takes in one vector and returns the length/magnitude squared</summary>
    public float V2_LengthSq()
    {
        float rv;
        rv = x * x + y * y;
        return rv;

    }

    ///<summary>innefficient due to use of sqrd but finds the length/magnitude</summary>
    public float V2_Length()
    {
        float rv;
        rv = Mathf.Sqrt(x * x + y * y);
        return rv;

    }

    ///<summary>Create a static function that takes in a vector and a scalar and returns a new vector that has been multiplied by the scalar</summary>
    public static MyVector2 Scaling_Vectors(MyVector2 vectorA, float scalar)
    {
        MyVector2 vector_scaled = new MyVector2(0f, 0f);

        vector_scaled.x = vectorA.x * scalar;
        vector_scaled.y = vectorA.y * scalar;

        return vector_scaled;

    }

    ///<summary>Create a static function that takes in a vector and a divisor and returns a new vector that has been divided by the divisor</summary>
    public static MyVector2 Dividing_Vectors(MyVector2 vectorA, float divisor)
    {
        MyVector2 vector_divided = new MyVector2(0f, 0f);

        vector_divided.x = vectorA.x / divisor;
        vector_divided.y = vectorA.y / divisor;

        return vector_divided;
    }

    ///<summary>Create a static function that takes in one vector and returns that vector normalized. You will need do step 3 before you can do this.</summary>
    public static MyVector2 Normalising_Vectors(MyVector2 vectorA)
    {
        MyVector2 vector_normalised = new MyVector2(0f, 0f);

        vector_normalised = Dividing_Vectors(vectorA, vectorA.V2_Length());

        return vector_normalised;
    }

    ///<summary>Create a static function that takes in two vectors and returns the dot product of the two vectors</summary>
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

    ///<summary>Create a static function that takes in two vectors and returns the angle between the two vectors</summary>
    public static MyVector2 ReflectVector(MyVector2 vector, MyVector2 normal)
    {
        MyVector2 reflected = new MyVector2(vector.x * -1, vector.y);// Calculate the reflection vector
        Debug.Log("velocity " + vector.x + " " + vector.y);
        Debug.Log("reflected velocity " + reflected.x + " " + reflected.y);
        //Debug.Break();

        return reflected;
    }

    
    ///<summary>static function for Vector Cross Product</summary>
    public static MyVector2 VectorCrossProduct(MyVector2 A, MyVector2 B)
    {
        MyVector2 C = new MyVector2(0f, 0f);
        C.x = A.y * B.x - A.x * B.y;
        C.y = A.x * B.y - A.y * B.x;

        return C;
    }

    ///<summary>Static function to Lerp between two vectors</summary>
    public static Vector2 VecLerp(MyVector2 A, MyVector2 B, float t)
    {
        MyVector2 r = AddingVectors2(Scaling_Vectors(A, (1f - t)), Scaling_Vectors(B, t));

        return r.ToUnityVector();
    }

    ///<summary>Static function to convert a vector to int</summary>
    public static MyVector2 MyVectorToInt(MyVector2 A)
    {
        MyVector2 B = new MyVector2(Mathf.RoundToInt(A.x), Mathf.RoundToInt(A.y));
        return B;
    }


    //--- Euler Angles Functions ---//

    ///<summary>Static function to convert a vector to radians</summary>
    public static float VectorToRadians(MyVector2 V)
    {
        float rv = 0f;
        rv = Mathf.Atan(V.y / V.x);

        return rv;
    }

    ///<summary>Static function to convert radians to a vector</summary>
    public static MyVector2 RadiansToVector(float angle)
    {
        MyVector2 rv = new MyVector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return rv;
    }

    //------------------//

}

