using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBubble : MonoBehaviour
{
    public int RandInt; //initialise random number for colour
    public bool newbubblecolor; // bool to check if new bubble colour is needed

    Renderer BubbleRenderer; // renderer

    // Start is called before the first frame update
    void Start()
    {
        RandInt = UnityEngine.Random.Range(0, 3); // random number between 0 and 3
        BubbleRenderer = GetComponent<Renderer>(); // find renderer
        //Debug.Log(RandInt);
        //RandomBubble(RandInt);
        BubbleRenderer.material.color = RandomBubble(RandInt); // change colour to be the equivalent of the switchcase
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
