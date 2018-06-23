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

    public float rayLenght = 10f;
    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        angledVector = Rotate(topAngledRayPoint,180, rayLenght);        
        // Run
        animator.SetFloat("Speed", 1f);
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (triggered == true)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX |
                                RigidbodyConstraints2D.FreezeRotation;  
            }
            else
            {
                drawLine.MouseUp();
            }
        }
        //for debug
        Debug.DrawLine(topAngledRayPoint.transform.position, Rotate(topAngledRayPoint, 90, rayLenght), Color.red);
        Debug.DrawLine(topAngledRayPoint.transform.position, Rotate(topAngledRayPoint, -45f, rayLenght), Color.blue);
        Debug.DrawLine(horizontalTop.transform.position, Rotate(horizontalTop, 0, rayLenght), Color.green);
        Debug.DrawLine(horizontalMiddle.transform.position, Rotate(horizontalMiddle, 0, rayLenght), Color.white);
        Debug.DrawLine(horizontalBottom.transform.position, Rotate(horizontalBottom, 0, rayLenght), Color.yellow);


        RaycastHit2D raycastHit2D = Physics2D.Linecast(topAngledRayPoint.transform.position, Vector2.up);
        if (/*charachter is crouching or crawling and*/ raycastHit2D.collider != null)
        {

        }
        raycastHit2D = Physics2D.Linecast(topAngledRayPoint.transform.position, Rotate(topAngledRayPoint, 45f, rayLenght));

         raycastHit2D = Physics2D.Raycast(topAngledRayPoint.transform.position, Vector2.up,10);
        if(/*charachter is crouching or crawling and*/ raycastHit2D.collider!=null)
        {

        }
        raycastHit2D = Physics2D.Raycast(topAngledRayPoint.transform.position,angledVector , 10);

    }

    private Vector2 Rotate(GameObject go,float angle,float length)
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
