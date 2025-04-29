using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BubbleGrid : MonoBehaviour
{
    public GameObject bubblePrefab;  // Assign the bubble prefab in the inspector
    public int rows = 8;             // Number of rows
    public int columns = 7;             // Number of columns
    public float bubbleSize = 1f;  // Spacing between bubbles
    public Vector2 startPosition = new Vector2(-3.2f, 3.5f);
    public GameObject[,] bubbles; // 2D array to store bubbles

    void Start()
    {
        CreateGrid();
    }

    void Update()
    {
        RemoveFloatingBubbles();
    }
    void CreateGrid()
    {
        bubbles = new GameObject[rows, columns]; // Initialize the 2D array
        for (int row = 0; row <= 3; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate position
                Vector2 spawnPos = new Vector2(
                    startPosition.x + col * bubbleSize,
                    startPosition.y - row * (bubbleSize * 0.85f));
                GameObject newBubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
                newBubble.transform.parent = transform;  // Organize hierarchy
                bubbles[row, col] = newBubble; // Add to 2D array
            }
        }
    }
    public Vector2 GetNearestGridPosition(Vector2 position)
    {
        float y = Mathf.Round(position.y / bubbleSize) * bubbleSize;
        float x = Mathf.Round((position.x / bubbleSize) * bubbleSize);
        x -= (Mathf.Round(position.y / bubbleSize) % 2 == 0) ? 0 : bubbleSize / 2;

        return new Vector2(x, y);
    }

    public Vector2Int GetGridCoords(Vector2 position)
    {
        int col = Mathf.RoundToInt((position.x - startPosition.x) / bubbleSize);
        int row = Mathf.RoundToInt((startPosition.y - position.y) / (bubbleSize * 0.85f)); // Adjust for vertical spacing

        return new Vector2Int(row, col);
    }

    // Helper flood fill
    private void FloodFillConnected(Bubble start, HashSet<Bubble> connectedSet)
    {
        Queue<Bubble> queue = new Queue<Bubble>();
        queue.Enqueue(start);
        connectedSet.Add(start);

        while (queue.Count > 0)
        {
            Bubble current = queue.Dequeue();
            foreach (Bubble neighbor in current.GetConnectedBubbles())
            {
                if (!connectedSet.Contains(neighbor))
                {
                    connectedSet.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }
    }


    public void RemoveFromBubbleGrid(GameObject bubble)
    {
        BubbleGrid bubbleGrid = FindObjectOfType<BubbleGrid>();
        Vector2Int gridCoords = bubbleGrid.GetGridCoords(bubble.transform.position);
        bubbleGrid.bubbles[gridCoords.x, gridCoords.y] = null; // Remove the bubble from the array  
        Debug.Log("removing bubble at: " + gridCoords);
    }

    public List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        int[,] evenOffsets = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { -1, 1 }, { -1, -1 } };
        int[,] oddOffsets = new int[,] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { 1, -1 } };
        bool isEvenRow = pos.x % 2 == 0;
        int[,] offsets = isEvenRow ? evenOffsets : oddOffsets;
        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int newRow = pos.x + offsets[i, 0];
            int newCol = pos.y + offsets[i, 1];
            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < columns)
            {
                neighbors.Add(new Vector2Int(newRow, newCol));
            }
        }
        return neighbors;
    }

    public HashSet<Vector2Int> FindConnectedToTop()
    {
        HashSet<Vector2Int> connected = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        // Start from all bubbles in top row
        for (int col = 0; col < columns; col++)
        {
            if (bubbles[0, col] != null)
            {
                Vector2Int start = new Vector2Int(0, col);
                queue.Enqueue(start);
                connected.Add(start);
            }
        }

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (connected.Contains(neighbor))
                    continue;

                if (bubbles[neighbor.x, neighbor.y] != null)
                {
                    connected.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return connected;
    }

    public void RemoveFloatingBubbles()
    {
        HashSet<Vector2Int> connected = FindConnectedToTop();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2Int pos = new Vector2Int(row, col);

                if (bubbles[row, col] != null && !connected.Contains(pos))
                {
                    Bubble objectBubble = bubbles[row, col].GetComponent<Bubble>();
                    //objectBubble.isFloating = true;
                    
                    Debug.Log("removing floating bubble at: " + pos);
                    //bubbles[row,col].gameObject.transform.position = Vector2.Lerp(bubbles[row,col].gameObject.transform.position, new Vector2(bubbles[row,col].gameObject.transform.position.x,-10f), 3f);
                    //Destroy(bubbles[row, col]); // Destroy the bubble
                    StartCoroutine(DropAndDestroy(bubbles[row, col]));
                    bubbles[row, col] = null;
                }
            }
        }
    }

    IEnumerator DropAndDestroy(GameObject bubble)
    {
        Vector2 start = bubble.transform.position;
        Vector2 end = new Vector2(start.x, -10f);
        float t = 0;
        float duration = 1f;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            bubble.transform.position = Vector2.Lerp(start, end, t);
            yield return null;
        }

        Destroy(bubble);
    }


}

