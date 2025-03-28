using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public GameObject BubblePrefab;
    public Vector2 BubbleSpawn;
    public float ShootSpeed = 1f;

    public bool BubbleInSpawn = false; // is there a bubble in the spawn

    Rigidbody rb;

    GameObject newBubble;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BubbleInSpawn == false)
        {
            newBubble = Instantiate(BubblePrefab, BubbleSpawn, Quaternion.identity); //instantiate new bubble
            Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0; // make sure the bubble doesn't fall
            BubbleInSpawn = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            shoot_bubble();
            StartCoroutine(DelayedAction());

        }

    }

    void shoot_bubble()
    {
        //GameObject newBubble = Instantiate(BubblePrefab, BubbleSpawn, Quaternion.identity); //instantiate new bubble
        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>(); //get bubbles rigidbody2d
        rb.gravityScale = 1;
        rb.velocity = DirectionMaths(); // assign the vector2 to the rigid body

    }

    void raycastLine()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Draw a line from the ray's origin to the hit point
            Debug.DrawLine(ray.origin, hit.point, Color.yellow);
        }
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

    IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 2 seconds
        BubbleInSpawn = false;
    }
}
