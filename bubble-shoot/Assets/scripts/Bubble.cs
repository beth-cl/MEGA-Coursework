using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public int RandInt;
    public bool wasFired = false;
    public List<Bubble> currentconnectedbubbles = new List<Bubble>();


    Renderer BubbleRenderer;

    // Start is called before the first frame update
    void Start()
    {
        RandInt = UnityEngine.Random.Range(0,3);
        BubbleRenderer = GetComponent<Renderer>();
        Color BubbleColour = RandomBubble(RandInt);
        BubbleRenderer.material.color = BubbleColour;
    }

    // Update is called once per frame  
    void Update()
    {
        // Check if the bubble is out of bounds of the grid array  
        BubbleGrid bubbleGrid = FindObjectOfType<BubbleGrid>();
        Vector2Int gridCoords = bubbleGrid.GetGridCoords(transform.position);

        if (gridCoords.y < 0 || gridCoords.y >= bubbleGrid.bubbles.GetLength(1))
        {
            Debug.LogWarning("Bubble is out of bounds and will be destroyed.");
            Destroy(gameObject);
        }

        //FindObjectOfType<BubbleGrid>().RemoveFloatingBubbles();  
        gameover();
    }

    private Color RandomBubble(int RandInt)
    {
        Color BubbleType;
        switch (RandInt)
        {
            case 0: BubbleType = Color.green; break;
            case 1: BubbleType = Color.magenta; break;
            case 2: BubbleType = Color.cyan; break;
            default: BubbleType = Color.white; break;
        }
        return BubbleType;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BubbleGrid bubbleGrid = FindObjectOfType<BubbleGrid>();
        if (collision.gameObject.CompareTag("Bubble") || collision.gameObject.CompareTag("Celing"))
        {
            Vector2 snappedPosition = bubbleGrid.GetNearestGridPosition(transform.position);
            transform.position = snappedPosition;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop movement  
            GetComponent<Rigidbody2D>().isKinematic = true;      // Fix in place  
            GetComponent<Rigidbody2D>().freezeRotation = true;

            Vector2Int gridCoords = bubbleGrid.GetGridCoords(snappedPosition);
            if (!bubbleGrid.bubbles[gridCoords.x, gridCoords.y]) // Check if the grid position is empty
            {
                bubbleGrid.bubbles[gridCoords.x, gridCoords.y] = gameObject; // Assign the bubble to the grid
                Debug.Log("Bubble assigned to grid position: " + gridCoords);
            }
            else
            {
                Debug.Log("Grid position already occupied");
                //Destroy(gameObject); // Destroy the bubble if the position is occupied
            }
            //bubbleGrid.bubbles[gridCoords.x, gridCoords.y] = gameObject;

            if (wasFired)
            {
                TryPopBubbles(); // Call the method to check for matching bubbles
                //RemoveFloatingBubbles();
            }
        }
        if (collision.gameObject.CompareTag("BubbleBin")) // Replace with the relevant tag  
        {
            Destroy(gameObject); // Destroys the object this script is attached to  
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall Collision Detected");
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            Vector2 velocity = rb.velocity; // Get the object's current velocity  

            if (velocity.magnitude > 0.01f) // Only reflect if moving fast enough
            {
                MyVector2 myVelocity = new MyVector2(velocity.x, velocity.y); // MyVector2 of the velocity  
                Vector2 normal = collision.contacts[0].normal; // Get the normal of the surface we collided with  
                MyVector2 myNormal = new MyVector2(normal.x, normal.y); // MyVector2 of normal  

                MyVector2 MyReflectedVelocity = MyVector2.ReflectVector(myVelocity, myNormal); // Reflect velocity  
                Vector2 reflectedVelocity = MyReflectedVelocity.ToUnityVector(); // Convert back to Vector2  

                rb.velocity = reflectedVelocity; // Apply reflected velocity
            }
            else
            {
                Debug.LogWarning("Velocity too small, skipping reflection.");
            }
        }

    }

    void gameover()
    {
        GameObject GOshooter = GameObject.Find("Spawner");
        Shooter shooter = GOshooter.GetComponent<Shooter>();

        if (shooter.BubbleInSpawn == true && transform.position.y == -3)
        {
            Debug.Log("gameover");

            GameObject panel = GameObject.Find("EventSystem");
            if (panel != null)
            {
                GameOver panelScript = panel.GetComponent<GameOver>();
                if (panelScript != null)
                {
                    panelScript.GameOverFlag = true;

                }
            }
        }
    }


    // testing bubble destroy

    public List<Bubble> GetConnectedBubbles()
    {
        List<Bubble> connectedBubbles = new List<Bubble>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.8f);

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
                FindObjectOfType<BubbleGrid>().RemoveFromBubbleGrid(bubble.gameObject);
                Destroy(bubble.gameObject);
            }
        }
    }
    IEnumerable DelayedPopBubbles()
    {
        yield return new WaitForSeconds(0.1f); // Wait 0.1 seconds

        if (wasFired)
        {
            TryPopBubbles();

        }
    }


}