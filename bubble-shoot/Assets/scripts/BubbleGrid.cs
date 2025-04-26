using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGrid : MonoBehaviour
{
    public GameObject bubblePrefab;  // Assign the bubble prefab in the inspector
    public int rows = 9;             // Number of rows
    public int columns = 7;             // Number of columns
    public float bubbleSize = 1f;  // Spacing between bubbles
    public Vector2 startPosition = new Vector2(-3.2f, 3.5f);
    Vector2 celingvector;

    //public List<GameObject> bubbles = new List<GameObject>(); // Stores bubbles in the grid
    public GameObject[,] bubbles; // 2D array to store bubbles

    void Start()
    {
        celingvector = GameObject.FindGameObjectWithTag("Celing").transform.position;
        CreateGrid();
    }

    void Update()
    {

    }

    void CreateGrid()
    {
        bubbles = new GameObject[rows, columns]; // Initialize the 2D array
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate position
                Vector2 spawnPos = new Vector2(
                    //startPosition.x + col * bubbleSize + xOffset,
                    startPosition.x + col * bubbleSize,
                    startPosition.y - row * (bubbleSize * 0.85f) // Adjust vertical spacing
                );
                // Spawn bubble
                GameObject newBubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
                newBubble.transform.parent = transform;  // Organize hierarchy
                //bubbles.Add(newBubble); // Add to list
                bubbles[row, col] = newBubble; // Add to 2D array
            }
        }
    }
    public Vector2 GetNearestGridPosition(Vector2 position)
    {
        //bool bubbleinclosest;
        float y = Mathf.Round(position.y / bubbleSize) * bubbleSize;
        //Mathf.Round(position.y);
        float x = Mathf.Round((position.x / bubbleSize) * bubbleSize);
        x -= (Mathf.Round(position.y / bubbleSize) % 2 == 0) ? 0 : bubbleSize / 2;

        return new Vector2(x, y);
    }
    public List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        int[,] evenOffsets = new int[,] {{ 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { -1, 1 }, { -1, -1 }};
        int[,] oddOffsets = new int[,]{{ 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { 1, -1 }};

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
                    Destroy(bubbles[row, col]);
                    bubbles[row, col] = null;
                }
            }
        }
    }

    public void AddToGrid()
    {

    }


}

