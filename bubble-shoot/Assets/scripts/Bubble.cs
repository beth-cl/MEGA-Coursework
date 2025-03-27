using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
        Color BubbleColour = RandomBubble(RandInt);
        BubbleRenderer.material.color = BubbleColour;
    }

    // Update is called once per frame
    void Update()
    {
        TryPopBubbles();
        gameover();
    }

    private Color RandomBubble(int RandInt)
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble") || collision.gameObject.CompareTag("Celing"))
        {
            Vector2 snappedPosition = FindObjectOfType<BubbleGrid>().GetNearestGridPosition(transform.position);
            /*if ((Vector2)snappedPosition == (Vector2)collision.transform.position)
            {
                snappedPosition = FindObjectOfType<BubbleGrid>().GetNearestGridPosition(transform.position + new Vector3(0.5f, 0.5f, 0));
            }*/
            transform.position = snappedPosition;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop movement
            GetComponent<Rigidbody2D>().isKinematic = true;      // Fix in place
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().freezeRotation = true;

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

    void gameover()
    {
        //Debug.Log(transform.position.y);
        GameObject GOshooter = GameObject.Find("Spawner");
        Shooter shooter = GOshooter.GetComponent<Shooter>();

        if (shooter.BubbleInSpawn == true && transform.position.y == -3)
        {
            Debug.Log("gameover");
        }

    }


    // testing bubble destroy

    public List<Bubble> GetConnectedBubbles()
    {
        List<Bubble> connectedBubbles = new List<Bubble>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (Collider2D hit in hits)
        {
            Bubble bubble = hit.GetComponent<Bubble>();
            if (bubble != null && bubble.RandInt == this.RandInt)
            {
                connectedBubbles.Add(bubble);
            }
        }

        return connectedBubbles;
    }

    public List<Bubble> FindMatchingBubbles()
    {
        List<Bubble> matchingBubbles = new List<Bubble>();
        Queue<Bubble> toCheck = new Queue<Bubble>();

        toCheck.Enqueue(this);
        matchingBubbles.Add(this);

        while (toCheck.Count > 0)
        {
            Bubble current = toCheck.Dequeue();
            foreach (Bubble neighbor in current.GetConnectedBubbles())
            {
                if (!matchingBubbles.Contains(neighbor))
                {
                    matchingBubbles.Add(neighbor);
                    toCheck.Enqueue(neighbor);
                }
            }
        }

        return matchingBubbles;
    }

    public void TryPopBubbles()
    {
        List<Bubble> matchingBubbles = FindMatchingBubbles();

        if (matchingBubbles.Count >= 3)
        {
            foreach (Bubble bubble in matchingBubbles)
            {
                Destroy(bubble.gameObject);
            }
        }
    }


}
