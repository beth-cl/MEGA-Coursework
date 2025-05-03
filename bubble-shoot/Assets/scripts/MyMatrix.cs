using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom 4x4 matrix implementation for mathematical operations
/// </summary>
public class MyMatrix4x4
{
    // Matrix values stored in row-major order
    private float[,] _values;
    
    /// <summary>
    /// Access the internal values of the matrix
    /// </summary>
    public float[,] Values 
    { 
        get { return _values; } 
        private set { _values = value; } 
    }
    
    /// <summary>
    /// Get or set a specific element in the matrix
    /// </summary>
    public float this[int row, int col]
    {
        get { return _values[row, col]; }
        set { _values[row, col] = value; }
    }
    
    /// <summary>
    /// Returns an identity matrix
    /// </summary>
    public static MyMatrix4x4 Identity
    {
        get
        {
            return new MyMatrix4x4(
                new Vector4(1, 0, 0, 0), 
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, 1, 0), 
                new Vector4(0, 0, 0, 1));
        }
    }
    
    /// <summary>
    /// Creates a matrix from four Vector4 columns
    /// </summary>
    public MyMatrix4x4(Vector4 column1, Vector4 column2, Vector4 column3, Vector4 column4)
    {
        _values = new float[4, 4];

        // Store in column-major order to match Unity's convention
        _values[0, 0] = column1.x;
        _values[1, 0] = column1.y;
        _values[2, 0] = column1.z;
        _values[3, 0] = column1.w;

        _values[0, 1] = column2.x;
        _values[1, 1] = column2.y;
        _values[2, 1] = column2.z;
        _values[3, 1] = column2.w;

        _values[0, 2] = column3.x;
        _values[1, 2] = column3.y;
        _values[2, 2] = column3.z;
        _values[3, 2] = column3.w;

        _values[0, 3] = column4.x;
        _values[1, 3] = column4.y;
        _values[2, 3] = column4.z;
        _values[3, 3] = column4.w;
    }
    
    /// <summary>
    /// Creates a matrix from four Vector3 columns (with implicit w components)
    /// </summary>
    public MyMatrix4x4(Vector3 column1, Vector3 column2, Vector3 column3, Vector3 column4)
    {
        _values = new float[4, 4];

        // Store in column-major order to match Unity's convention
        _values[0, 0] = column1.x;
        _values[1, 0] = column1.y;
        _values[2, 0] = column1.z;
        _values[3, 0] = 0f;

        _values[0, 1] = column2.x;
        _values[1, 1] = column2.y;
        _values[2, 1] = column2.z;
        _values[3, 1] = 0f;

        _values[0, 2] = column3.x;
        _values[1, 2] = column3.y;
        _values[2, 2] = column3.z;
        _values[3, 2] = 0f;

        _values[0, 3] = column4.x;
        _values[1, 3] = column4.y;
        _values[2, 3] = column4.z;
        _values[3, 3] = 1f;
    }
    
    /// <summary>
    /// Creates a matrix from a 2D array of values
    /// </summary>
    public MyMatrix4x4(float[,] values)
    {
        if (values.GetLength(0) != 4 || values.GetLength(1) != 4)
        {
            Debug.LogError("Matrix must be 4x4");
            _values = new float[4, 4]; // Initialize with zeros
            return;
        }
            
        _values = new float[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                _values[i, j] = values[i, j];
            }
        }
    }
    
    /// <summary>
    /// Multiplies a matrix by a vector
    /// </summary>
    public static Vector4 operator *(MyMatrix4x4 lhs, Vector4 vector)
    {
        Vector4 result = new Vector4();
        result.x = lhs._values[0, 0] * vector.x + lhs._values[0, 1] * vector.y + lhs._values[0, 2] * vector.z + lhs._values[0, 3] * vector.w;
        result.y = lhs._values[1, 0] * vector.x + lhs._values[1, 1] * vector.y + lhs._values[1, 2] * vector.z + lhs._values[1, 3] * vector.w;
        result.z = lhs._values[2, 0] * vector.x + lhs._values[2, 1] * vector.y + lhs._values[2, 2] * vector.z + lhs._values[2, 3] * vector.w;
        result.w = lhs._values[3, 0] * vector.x + lhs._values[3, 1] * vector.y + lhs._values[3, 2] * vector.z + lhs._values[3, 3] * vector.w;

        return result;
    }
    
    /// <summary>
    /// Multiplies a matrix by a 3D vector (treating it as a 4D vector with w=1)
    /// </summary>
    public Vector3 MultiplyPoint(Vector3 point)
    {
        Vector4 result = this * new Vector4(point.x, point.y, point.z, 1f);
        
        // Perform perspective division if w is not 1
        if (Mathf.Abs(result.w) > Mathf.Epsilon)
        {
            return new Vector3(result.x / result.w, result.y / result.w, result.z / result.w);
        }
        
        return new Vector3(result.x, result.y, result.z);
    }
    
    /// <summary>
    /// Multiplies a matrix by a 3D vector as a direction (treating it as a 4D vector with w=0)
    /// </summary>
    public Vector3 MultiplyVector(Vector3 vector)
    {
        Vector4 result = this * new Vector4(vector.x, vector.y, vector.z, 0f);
        return new Vector3(result.x, result.y, result.z);
    }
    
    /// <summary>
    /// Multiplies two matrices
    /// </summary>
    public static MyMatrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
    {
        float[,] result = new float[4, 4];
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < 4; k++)
                {
                    result[i, j] += lhs._values[i, k] * rhs._values[k, j];
                }
            }
        }
        
        return new MyMatrix4x4(result);
    }
    
    /// <summary>
    /// Returns the transpose of this matrix
    /// </summary>
    public MyMatrix4x4 Transpose()
    {
        float[,] result = new float[4, 4];
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result[i, j] = _values[j, i];
            }
        }
        
        return new MyMatrix4x4(result);
    }
    
    /// <summary>
    /// Calculates the determinant of a 2x2 submatrix
    /// </summary>
    private float Determinant2x2(float a, float b, float c, float d)
    {
        return a * d - b * c;
    }
    
    /// <summary>
    /// Calculates the determinant of a 3x3 submatrix
    /// </summary>
    private float Determinant3x3(float[,] matrix)
    {
        return matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1])
             - matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0])
             + matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);
    }
    
    /// <summary>
    /// Creates a 3x3 submatrix by removing the specified row and column
    /// </summary>
    private float[,] CreateSubMatrix3x3(int excludeRow, int excludeCol)
    {
        float[,] subMatrix = new float[3, 3];
        int r = 0;
        
        for (int i = 0; i < 4; i++)
        {
            if (i == excludeRow) continue;
            
            int c = 0;
            for (int j = 0; j < 4; j++)
            {
                if (j == excludeCol) continue;
                subMatrix[r, c] = _values[i, j];
                c++;
            }
            r++;
        }
        
        return subMatrix;
    }
    
    /// <summary>
    /// Calculates the determinant of the 4x4 matrix
    /// </summary>
    public float Determinant()
    {
        // Use cofactor expansion along the first row
        float det = 0;
        
        for (int j = 0; j < 4; j++)
        {
            float[,] subMatrix = CreateSubMatrix3x3(0, j);
            float cofactor = _values[0, j] * Determinant3x3(subMatrix);
            
            // Alternate signs based on position
            if (j % 2 == 1) cofactor = -cofactor;
            
            det += cofactor;
        }
        
        return det;
    }
    
    /// <summary>
    /// Calculates the inverse of the matrix
    /// </summary>
    public MyMatrix4x4 Inverse()
    {
        float det = Determinant();
        
        if (Mathf.Abs(det) < Mathf.Epsilon)
        {
            Debug.LogError("Matrix is singular and cannot be inverted.");
            return Identity; // Return identity as a fallback
        }
        
        float[,] result = new float[4, 4];
        
        // Calculate the adjugate matrix (transpose of cofactor matrix)
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                float[,] subMatrix = CreateSubMatrix3x3(i, j);
                float cofactor = Determinant3x3(subMatrix);
                
                // Apply checkerboard pattern for cofactor signs
                if ((i + j) % 2 == 1) cofactor = -cofactor;
                
                // Transpose while calculating the adjugate
                result[j, i] = cofactor / det;
            }
        }
        
        return new MyMatrix4x4(result);
    }
    
    /// <summary>
    /// Creates a translation matrix
    /// </summary>
    public static MyMatrix4x4 CreateTranslation(float x, float y, float z)
    {
        return new MyMatrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(x, y, z, 1)
        );
    }
    
    /// <summary>
    /// Creates a 2D translation matrix
    /// </summary>
    public static MyMatrix4x4 CreateTranslation2D(float x, float y)
    {
        return new MyMatrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(x, y, 0, 1)
        );
    }
    
    /// <summary>
    /// Creates a rotation matrix around the X axis
    /// </summary>
    public static MyMatrix4x4 CreateRotationX(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        
        return new MyMatrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, cos, sin, 0),
            new Vector4(0, -sin, cos, 0),
            new Vector4(0, 0, 0, 1)
        );
    }
    
    /// <summary>
    /// Creates a rotation matrix around the Y axis
    /// </summary>
    public static MyMatrix4x4 CreateRotationY(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        
        return new MyMatrix4x4(
            new Vector4(cos, 0, -sin, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(sin, 0, cos, 0),
            new Vector4(0, 0, 0, 1)
        );
    }
    
    /// <summary>
    /// Creates a rotation matrix around the Z axis
    /// </summary>
    public static MyMatrix4x4 CreateRotationZ(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        
        return new MyMatrix4x4(
            new Vector4(cos, sin, 0, 0),
            new Vector4(-sin, cos, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1)
        );
    }
    
    /// <summary>
    /// Creates a 2D rotation matrix (around Z axis)
    /// </summary>
    public static MyMatrix4x4 CreateRotation2D(float degrees)
    {
        return CreateRotationZ(degrees);
    }
    
    /// <summary>
    /// Creates a scale matrix
    /// </summary>
    public static MyMatrix4x4 CreateScale(float scaleX, float scaleY, float scaleZ)
    {
        return new MyMatrix4x4(
            new Vector4(scaleX, 0, 0, 0),
            new Vector4(0, scaleY, 0, 0),
            new Vector4(0, 0, scaleZ, 0),
            new Vector4(0, 0, 0, 1)
        );
    }
    
    /// <summary>
    /// Creates a 2D scale matrix
    /// </summary>
    public static MyMatrix4x4 CreateScale2D(float scaleX, float scaleY)
    {
        return new MyMatrix4x4(
            new Vector4(scaleX, 0, 0, 0),
            new Vector4(0, scaleY, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1)
        );
    }
    
    /// <summary>
    /// Creates a perspective projection matrix
    /// </summary>
    public static MyMatrix4x4 CreatePerspective(float fov, float aspectRatio, float nearPlane, float farPlane)
    {
        float tanHalfFov = Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        float zRange = farPlane - nearPlane;
        
        if (Mathf.Abs(zRange) < Mathf.Epsilon || Mathf.Abs(tanHalfFov) < Mathf.Epsilon || aspectRatio == 0)
        {
            Debug.LogError("Invalid parameters for perspective matrix");
            return Identity;
        }
        
        float[,] result = new float[4, 4];
        
        result[0, 0] = 1.0f / (aspectRatio * tanHalfFov);
        result[1, 1] = 1.0f / tanHalfFov;
        result[2, 2] = -(farPlane + nearPlane) / zRange;
        result[2, 3] = -1.0f;
        result[3, 2] = -(2.0f * farPlane * nearPlane) / zRange;
        
        return new MyMatrix4x4(result);
    }
    
    /// <summary>
    /// Creates an orthographic projection matrix
    /// </summary>
    public static MyMatrix4x4 CreateOrthographic(float left, float right, float bottom, float top, float nearPlane, float farPlane)
    {
        float width = right - left;
        float height = top - bottom;
        float depth = farPlane - nearPlane;
        
        if (Mathf.Abs(width) < Mathf.Epsilon || Mathf.Abs(height) < Mathf.Epsilon || Mathf.Abs(depth) < Mathf.Epsilon)
        {
            Debug.LogError("Invalid parameters for orthographic matrix");
            return Identity;
        }
        
        float[,] result = new float[4, 4];
        
        result[0, 0] = 2.0f / width;
        result[1, 1] = 2.0f / height;
        result[2, 2] = -2.0f / depth;
        result[0, 3] = -(right + left) / width;
        result[1, 3] = -(top + bottom) / height;
        result[2, 3] = -(farPlane + nearPlane) / depth;
        result[3, 3] = 1.0f;
        
        return new MyMatrix4x4(result);
    }
    
    /// <summary>
    /// Creates a look-at view matrix
    /// </summary>
    public static MyMatrix4x4 CreateLookAt(Vector3 eye, Vector3 target, Vector3 up)
    {
        Vector3 zAxis = (eye - target).normalized;
        Vector3 xAxis = Vector3.Cross(up, zAxis).normalized;
        Vector3 yAxis = Vector3.Cross(zAxis, xAxis);
        
        float[,] result = new float[4, 4];
        
        result[0, 0] = xAxis.x;
        result[0, 1] = xAxis.y;
        result[0, 2] = xAxis.z;
        result[0, 3] = -Vector3.Dot(xAxis, eye);
        
        result[1, 0] = yAxis.x;
        result[1, 1] = yAxis.y;
        result[1, 2] = yAxis.z;
        result[1, 3] = -Vector3.Dot(yAxis, eye);
        
        result[2, 0] = zAxis.x;
        result[2, 1] = zAxis.y;
        result[2, 2] = zAxis.z;
        result[2, 3] = -Vector3.Dot(zAxis, eye);
        
        result[3, 0] = 0;
        result[3, 1] = 0;
        result[3, 2] = 0;
        result[3, 3] = 1;
        
        return new MyMatrix4x4(result);
    }
    
    /// <summary>
    /// Converts to Unity's Matrix4x4
    /// </summary>
    public Matrix4x4 ToUnityMatrix()
    {
        Matrix4x4 result = new Matrix4x4();
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result[i, j] = _values[i, j];
            }
        }
        
        return result;
    }
    
    /// <summary>
    /// Creates a MyMatrix4x4 from Unity's Matrix4x4
    /// </summary>
    public static MyMatrix4x4 FromUnityMatrix(Matrix4x4 matrix)
    {
        float[,] values = new float[4, 4];
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                values[i, j] = matrix[i, j];
            }
        }
        
        return new MyMatrix4x4(values);
    }
    
    /// <summary>
    /// Returns a string representation of the matrix
    /// </summary>
    public override string ToString()
    {
        string result = "";
        
        for (int i = 0; i < 4; i++)
        {
            result += "[ ";
            for (int j = 0; j < 4; j++)
            {
                result += _values[i, j].ToString("F2") + " ";
            }
            result += "]\n";
        }
        
        return result;
    }
}