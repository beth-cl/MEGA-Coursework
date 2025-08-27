using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class BubbleGrid : MonoBehaviour
{
    public GameObject bubblePrefab;
    public int rows = 8;
    public int columns = 8;
    public float bubbleSize = 1f;
    public MyVector2 startPosition = new MyVector2(-3.2f, 4f);
    public GameObject[,] bubbles;

    void Start()
    {
        CreateGrid();
    }

    /// <summary>Creates the bubble grid and populates it with bubble objects.</summary>
    void CreateGrid()
    {
        bubbles = new GameObject[rows, columns];
        for (int row = 0; row <= 3; row++)
        {
            float offset = (row % 2 == 0) ? 0f : bubbleSize / 2f;
            for (int col = 0; col < columns; col++)
            {
                MyVector2 spawnPos = new MyVector2(
                    startPosition.x + col * bubbleSize + offset,
                    startPosition.y - row * (bubbleSize * 0.85f));
                GameObject newBubble = Instantiate(bubblePrefab, spawnPos.ToUnityVector(), Quaternion.identity);
                newBubble.transform.parent = transform;
                bubbles[row, col] = newBubble;
            }
        }
    }

    /// <summary>Finds the nearest grid position to a given world position.</summary>
    public MyVector2 GetNearestGridPosition(MyVector2 position)
    {
        float row = Mathf.Round((startPosition.y - position.y) / (bubbleSize * 0.85f));
        float y = startPosition.y - row * (bubbleSize * 0.85f);

        float offset = (row % 2 == 0) ? 0f : bubbleSize / 2f;
        float col = Mathf.Round((position.x - startPosition.x - offset) / bubbleSize);
        float x = startPosition.x + col * bubbleSize + offset;

        return new MyVector2(x, y);
    }

    /// <summary>Gets the grid coordinates (row and column) for a given world position.</summary>
    public MyVector2 GetGridCoords(MyVector2 position)
    {
        int col = Mathf.RoundToInt((position.x - startPosition.x) / bubbleSize);
        int row = Mathf.RoundToInt((startPosition.y - position.y) / (bubbleSize * 0.85f));
        MyVector2 FindRandC = new MyVector2(row, col);
        return MyVector2.MyVectorToInt(FindRandC);
    }

    /// <summary>Performs a flood fill to find all connected bubbles starting from a given bubble.</summary>
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

    /// <summary>Removes a bubble from the grid at its current position.</summary>
    public void RemoveFromBubbleGrid(GameObject bubble)
    {
        BubbleGrid bubbleGrid = FindObjectOfType<BubbleGrid>();
        MyVector2 gridCoords = bubbleGrid.GetGridCoords(new MyVector2(bubble.transform.position.x, bubble.transform.position.y));
        bubbleGrid.bubbles[(int)gridCoords.x, (int)gridCoords.y] = null;
        Debug.Log("removing bubble at: " + gridCoords);
    }

    /// <summary>Gets the neighboring grid positions for a given position.</summary>
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

    /// <summary>Finds all bubbles connected to the top row of the grid.</summary>
    public HashSet<Vector2Int> FindConnectedToTop()
    {
        HashSet<Vector2Int> connected = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

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

    /// <summary>Removes all bubbles that are not connected to the top row.</summary>
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
                    Debug.Log("removing floating bubble at: " + pos);
                    objectBubble.isFloating = true;
                    bubbles[row, col] = null;
                }
            }
        }
    }
}
