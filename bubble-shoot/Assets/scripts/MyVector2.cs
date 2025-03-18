using System.Collections;
using System.Collections.Generic;
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

    //Create a static function that takes in two MyVector3 objects, adds them together, and returns the resulting vector
    public static MyVector2 AddingVectors2(MyVector2 vectorA, MyVector2 vectorB)
    {
        MyVector2 vectorC = new MyVector2(0f, 0f);

        vectorC.x = vectorA.x + vectorB.x;
        vectorC.y = vectorA.y + vectorB.y;

        return vectorC;
    }

    //Create a static function that takes in two MyVector3 objects, and subtracts them, and returns the resulting vector
    public static MyVector2 SubtractingVector2(MyVector2 vectorA, MyVector2 vectorB)
    {
        MyVector2 vectorC = new MyVector2(0f, 0f);

        vectorC.x = vectorA.x - vectorB.x;
        vectorC.y = vectorA.y - vectorB.y;

        return vectorC;
    }

    public Vector2 ToUnityVector()
    {
        Vector2 vectorC = new Vector2();

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

}
