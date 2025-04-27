using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBubble : MonoBehaviour
{
    public float destroyHeight = -10f;

    void Update()
    {
        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}