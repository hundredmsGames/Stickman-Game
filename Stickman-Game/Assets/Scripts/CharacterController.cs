using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator animator;

    public float speed = 3f;

    private Rigidbody2D rigidBody;

    public DrawLine2D drawLine;

    bool triggered;

    public GameObject topAngledRayPoint;
    public GameObject horizontalTop;
    public GameObject horizontalMiddle;
    public GameObject horizontalBottom;
    Vector2 angledVector;

    public float rayLength = 10f;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        angledVector = RotateRay(topAngledRayPoint,180, rayLength);        
        
        // Run
        animator.SetFloat("Speed", 1f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckLine();
        CheckRays();
    }

    // TODO: We can find better names for these methods.
    void CheckLine()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // FIXME: triggered is not true ever never, because there is not trigger
            // for character right now. We need to add a trigger for every part of the character


            // Line is in the character, stop character
            if (triggered == true)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX |
                                RigidbodyConstraints2D.FreezeRotation;

                // Don't enable collider. Because it has side effects like throwing character.
                drawLine.MouseUp(false);
            }
            // Line is not in the character
            else
            {
                // Enable collider because line is not in the character.
                drawLine.MouseUp(true);
            }
        }
    }

    void CheckRays()
    {
        //for debug
        Debug.DrawLine(topAngledRayPoint.transform.position, RotateRay(topAngledRayPoint, 90, rayLength), Color.red);
        Debug.DrawLine(topAngledRayPoint.transform.position, RotateRay(topAngledRayPoint, -45f, rayLength), Color.blue);
        Debug.DrawLine(horizontalTop.transform.position, RotateRay(horizontalTop, 0, rayLength), Color.green);
        Debug.DrawLine(horizontalMiddle.transform.position, RotateRay(horizontalMiddle, 0, rayLength), Color.white);
        Debug.DrawLine(horizontalBottom.transform.position, RotateRay(horizontalBottom, 0, rayLength), Color.yellow);

        RaycastHit2D raycastHit2D = Physics2D.Linecast(topAngledRayPoint.transform.position, Vector2.up);
        if (/*charachter is crouching or crawling and*/ raycastHit2D.collider != null)
        {

        }
        raycastHit2D = Physics2D.Linecast(topAngledRayPoint.transform.position, RotateRay(topAngledRayPoint, 45f, rayLength));

        raycastHit2D = Physics2D.Raycast(topAngledRayPoint.transform.position, Vector2.up, 10);
        if (/*charachter is crouching or crawling and*/ raycastHit2D.collider != null)
        {

        }
        raycastHit2D = Physics2D.Raycast(topAngledRayPoint.transform.position, angledVector, 10);
    }

    private Vector2 RotateRay(GameObject go, float angle, float length)
    {
        Vector2 v = new Vector2(length, 0);
       float newX = v.x * Mathf.Cos(angle * Mathf.Deg2Rad) + v.y * -Mathf.Sin(angle * Mathf.Deg2Rad);
       float newY = v.x * Mathf.Sin(angle * Mathf.Deg2Rad) + v.y * Mathf.Cos(angle * Mathf.Deg2Rad);

        newX += go.transform.position.x;
        newY += go.transform.position.y;
        return new Vector2(newX,newY);
    }

    private void FixedUpdate()
    {
        // Update character's position
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        triggered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggered = false;
    }
}
