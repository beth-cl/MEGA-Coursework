using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public int RandInt;
    
    Renderer BubbleRenderer;

    // Start is called before the first frame update
    void Start()
    {
        RandInt = UnityEngine.Random.Range(0,3);
        BubbleRenderer = GetComponent<Renderer>();
        //Debug.Log(RandInt);
        RandomBubble(RandInt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomBubble(int RandInt)
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

        BubbleRenderer.material.color = BubbleType;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble") || collision.gameObject.CompareTag("Celing"))
        {
            Vector2 snappedPosition = FindObjectOfType<BubbleGrid>().GetNearestGridPosition(transform.position);
            transform.position = snappedPosition;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop movement
            GetComponent<Rigidbody2D>().isKinematic = true;      // Fix in place
        }
        if (collision.gameObject.CompareTag("BubbleBin")) // Replace with the relevant tag
        {
            Destroy(gameObject); // Destroys the object this script is attached to
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // Get the object's current velocity
            Vector2 velocity = rb.velocity;

            // Get the normal of the surface we collided with
            Vector2 normal = collision.contacts[0].normal;

            // Calculate the reflected velocity
            Vector2 reflectedVelocity = Vector2.Reflect(velocity, normal);

            // Apply the new velocity
            GetComponent<Rigidbody2D>().velocity = reflectedVelocity;
        }
    }
}
