using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bubble : MonoBehaviour
{
    public int colourIndex;
    public bool wasFired = false;
    public List<Bubble> currentconnectedbubbles = new List<Bubble>();
    public bool isFloating = false;
    Renderer BubbleRenderer;
    MyVector2 rv;
    private BubbleGrid bubbleGrid;

    void Start()
    {
        colourIndex = UnityEngine.Random.Range(0, 3);
        BubbleRenderer = GetComponent<Renderer>();
        Color BubbleColour = RandomBubble(colourIndex);
        BubbleRenderer.material.color = BubbleColour;
        rv = new MyVector2(gameObject.transform.position.x, gameObject.transform.position.y);
        bubbleGrid = FindObjectOfType<BubbleGrid>();
    }

    void Update()
    {
        MyVector2 gridCoords = bubbleGrid.GetGridCoords(new MyVector2(gameObject.transform.position.x, gameObject.transform.position.y));
        bubbleGrid.RemoveFloatingBubbles();
        gameover();

        if (isFloating)
        {
            Collider2D collider = GetComponent<Collider2D>();
            Debug.Log("Bubble is floating");
            collider.enabled = false;
            transform.position = MyVector2.VecLerp(new MyVector2(transform.position.x, transform.position.y), new MyVector2(transform.position.x, -10f), 0.1f);
        }

        if (gameObject.transform.position.y < -6)
        {
            Debug.Log("Bubble is out of bounds and will be destroyed.");
            Destroy(gameObject);
        }
    }

    /// <summary>Generates a random color for the bubble based on the given random integer.</summary>
    private Color RandomBubble(int colourIndex)
    {
        Color BubbleType;
        switch (colourIndex)
        {
            case 0: BubbleType = new Color(242f / 255f, 163f / 255f, 89f / 255f); break;
            case 1: BubbleType = new Color(184f / 255f, 51f / 255f, 106f / 255f); break;
            case 2: BubbleType = new Color(37f / 255f, 142f / 255f, 166f / 255f); break;
            default: BubbleType = Color.white; break;
        }
        return BubbleType;
    }

    /// <summary>Handles collision events with other objects, such as bubbles or walls.</summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        MyVector2 TransVector = new MyVector2(gameObject.transform.position.x, gameObject.transform.position.y);
        BubbleGrid bubbleGrid = FindObjectOfType<BubbleGrid>();

        if (collision.gameObject.CompareTag("Bubble") || collision.gameObject.CompareTag("Celing"))
        {
            Debug.Log("Bubble Collision Detected " + gameObject.transform.position);
            MyVector2 snappedPosition = bubbleGrid.GetNearestGridPosition(TransVector);
            Debug.Log("Snapped Position: " + snappedPosition.x + " " + snappedPosition.y);
            myMatrix4x4.ApplyCustom2DTranslation(gameObject, MyVector2.SubtractingVector2(snappedPosition, TransVector));
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().freezeRotation = true;

            MyVector2 gridCoords = bubbleGrid.GetGridCoords(snappedPosition);
            if (gridCoords.x >= 0 && gridCoords.x < bubbleGrid.rows && gridCoords.y >= 0 && gridCoords.y < bubbleGrid.columns && bubbleGrid.bubbles[(int)gridCoords.x, (int)gridCoords.y] == null)
            {
                bubbleGrid.bubbles[(int)gridCoords.x, (int)gridCoords.y] = gameObject;
                Debug.Log("Bubble assigned to grid position: " + gridCoords);
            }
            else
            {
                Debug.Log("Grid position already occupied");
            }

            if (wasFired)
            {
                TryPopBubbles();
            }
        }

        if (collision.gameObject.CompareTag("BubbleBin"))
        {
            Destroy(gameObject);
        }

        rv = new MyVector2(gameObject.transform.position.x, gameObject.transform.position.y);
    }

    /// <summary>Handles trigger events with walls to reflect the bubble's velocity.</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LeftWall") || other.CompareTag("RightWall"))
        {
            Debug.Log("Wall Collision Detected " + gameObject.transform.position);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 velocity = rb.velocity;

            if (velocity.magnitude > 0.01f)
            {
                MyVector2 myVelocity = new MyVector2(velocity.x, velocity.y);
                
                

                MyVector2 MyReflectedVelocity = MyVector2.ReflectVector(myVelocity);
                Vector2 reflectedVelocity = MyReflectedVelocity.ToUnityVector();

                rb.velocity = reflectedVelocity;
            }
            else
            {
                Debug.LogWarning("Velocity too small, skipping reflection.");
            }
        }
    }

    /// <summary>Checks for game over conditions and triggers the game over panel if necessary.</summary>
    void gameover()
    {
        GameObject GOshooter = GameObject.Find("Spawner");
        Shooter shooter = GOshooter.GetComponent<Shooter>();

        if (shooter.BubbleInSpawn == true && gameObject.transform.position.y == -3)
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

    /// <summary>Finds all bubbles connected to this bubble within a certain radius.</summary>
    public List<Bubble> GetConnectedBubbles()
    {
        List<Bubble> connectedBubbles = new List<Bubble>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(gameObject.transform.position, 0.8f);

        foreach (Collider2D hit in hits)
        {
            Bubble bubble = hit.GetComponent<Bubble>();
            if (bubble != null && bubble.colourIndex == this.colourIndex)
            {
                connectedBubbles.Add(bubble);
            }
        }

        return connectedBubbles;
    }

    /// <summary>Finds all bubbles that match the color of this bubble and are connected.</summary>
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

    /// <summary>Attempts to pop bubbles if there are at least three matching bubbles connected.</summary>
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

    /// <summary>Delays the popping of bubbles by a short duration.</summary>
    IEnumerable DelayedPopBubbles()
    {
        yield return new WaitForSeconds(0.1f);

        if (wasFired)
        {
            TryPopBubbles();
        }
    }

    /// <summary>Moves the bubble to a specified position and destroys it after a duration.</summary>
    private IEnumerator MoveAndDestroy()
    {
        float duration = 2f;
        float elapsedTime = 0f;
        MyVector2 startPosition = new MyVector2(gameObject.transform.position.x, gameObject.transform.position.y);
        MyVector2 endPosition = new MyVector2(gameObject.transform.position.x, -10f);
        MyVector2 lastPosition = startPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            Vector2 currentInterpolated = MyVector2.VecLerp(startPosition, endPosition, t);
            MyVector2 test = new MyVector2(currentInterpolated.x, currentInterpolated.y);

            MyVector2 delta = MyVector2.SubtractingVector2(test, lastPosition);
            myMatrix4x4.ApplyCustom2DTranslation(gameObject, delta);

            lastPosition = test;

            Vector2 interpolatedPosition = MyVector2.VecLerp(startPosition, endPosition, t);
            myMatrix4x4.ApplyCustom2DTranslation(gameObject, MyVector2.SubtractingVector2(new MyVector2(interpolatedPosition.x, interpolatedPosition.y), startPosition));
            yield return null;
        }

        Destroy(gameObject);
    }

    /// <summary>Delays the destruction of the bubble by a specified duration.</summary>
    IEnumerator DelayedDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

