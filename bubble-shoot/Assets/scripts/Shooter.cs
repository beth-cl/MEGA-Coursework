using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public GameObject BubblePrefab;
    public Vector2 BubbleSpawn;
    public float ShootSpeed = 1f;
    //bool bubblespawn = false;

    GameObject newBubble;


    // Start is called before the first frame update
    void Start()
    {
        Bubble bubble = BubblePrefab.GetComponent<Bubble>();


    }

    // Update is called once per frame
    void Update()
    {
        if (newBubble == null)
        {
            SpawnBubble();
        }

        // If the player clicks, shoot the bubble
        if (Input.GetMouseButtonDown(0) && newBubble != null)
        {
            ShootBubble();
        }
    }

    void SpawnBubble()
    {
        Bubble bubble = BubblePrefab.GetComponent<Bubble>(); //get the bubble script

        newBubble = Instantiate(BubblePrefab, BubbleSpawn, Quaternion.identity); //create a new bubble
        newBubble.name = "Bubble " + bubble.BubbleShootID; //set bubble name
        bubble.BubbleShootID++; //set next bubble id
        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>(); //get rigidbody
        rb.velocity = Vector2.zero; // set velocity to zero
        rb.gravityScale = 0;// Prevent it from falling
        rb.isKinematic = true;
    }

    void ShootBubble()
    {
        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.velocity = DirectionMaths();
        newBubble = null;
    }    

    void BubbleDirectionRender()
    {

    }

    public Vector2 DirectionMaths()
    {
        MyVector2 camvector = new MyVector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y); //location of mouse
        MyVector2 transvector = new MyVector2(transform.position.x, transform.position.y); //location of spawner

        MyVector2 Subvector = MyVector2.SubtractingVector2(camvector, transvector); //subtract the mouse position from the spawner position
        MyVector2 Normvector = MyVector2.Normalising_Vectors(Subvector); //normalise the vector

        Normvector = MyVector2.Scaling_Vectors(Normvector, ShootSpeed); //scale the normalised vector

        Vector2 direction = Normvector.ToUnityVector(); //convert MyVector2 to Unity Vector2

        return direction;
    }
}
