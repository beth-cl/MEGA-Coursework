using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBubble : MonoBehaviour
{
    public int RandInt;
    public bool newbubblecolor;

    Renderer BubbleRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
        RandInt = UnityEngine.Random.Range(0, 3);
        BubbleRenderer = GetComponent<Renderer>();
        //Debug.Log(RandInt);
        //RandomBubble(RandInt);
        BubbleRenderer.material.color = RandomBubble(RandInt);
    }

    // Update is called once per frame
    void Update()
    {
        if (newbubblecolor)
        {
            RandInt = UnityEngine.Random.Range(0, 3);
            BubbleRenderer.material.color = RandomBubble(RandInt);
            newbubblecolor = false;
        }
        
    }

    public static Color RandomBubble(int RandInt)
    {
        Color BubbleType;

        switch (RandInt)
        {
            case 0:
                BubbleType = Color.green;
                break;
            case 1:
                BubbleType = Color.magenta;
                break;
            case 2:
                BubbleType = Color.cyan;
                break;
            default:
                BubbleType = Color.white;
                break;
        }

        return BubbleType;
    }
}
