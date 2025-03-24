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

    private List<GameObject> bubbles = new List<GameObject>(); // Stores bubbles in the grid

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Apply hexagonal offset
                //float xOffset = (row % 2 == 0) ? 0 : bubbleSize / 2;

                // Calculate position
                Vector2 spawnPos = new Vector2(
                    //startPosition.x + col * bubbleSize + xOffset,
                    startPosition.x + col * bubbleSize,
                    startPosition.y - row * (bubbleSize * 0.85f) // Adjust vertical spacing
                );

                // Spawn bubble
                GameObject newBubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
                newBubble.transform.parent = transform;  // Organize hierarchy
            }
        }
    }

    public Vector2 GetNearestGridPosition(Vector2 position)
    {
        /*float x = Mathf.Round((position.x / bubbleSize) * bubbleSize);
        x -= (Mathf.Round(position.y / bubbleSize) % 2 == 0) ? 0 : bubbleSize/2;
        float y = Mathf.Round(position.y / bubbleSize) * bubbleSize;
        GeneralMaths.RoundValue(position.y);*/
        float x = GeneralMaths.RoundValue((position.x / bubbleSize) * bubbleSize);
        x -= (GeneralMaths.RoundValue(position.y / bubbleSize) % 2 == 0) ? 0 : bubbleSize/2;
        float y = GeneralMaths.RoundValue(position.y / bubbleSize) * bubbleSize;
        return new Vector2(x, y);
        
    }
}
