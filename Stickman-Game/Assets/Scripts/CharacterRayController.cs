using UnityEngine;

public partial class CharacterController : MonoBehaviour
{
    public Transform verticalRayPoint;
    public Transform horTopRayPoint;
    public Transform horMidRayPoint;
    public Transform horBottomRayPoint;

    public float rayLength;
    private float angledRayLenght=100;
    public float rayAngle;
    
    void CheckRays()
    {
        // Vertical ray, (towards up)
        RaycastHit2D verRaycastHit = Physics2D.Raycast(verticalRayPoint.position, Vector2.up, rayLength);
        Debug.DrawRay(verticalRayPoint.position, Vector2.up * rayLength, Color.red);

        // Horizontal-Top ray
        RaycastHit2D horTopRaycastHit = Physics2D.Raycast(horTopRayPoint.position, Vector2.right, rayLength);
        Debug.DrawRay(horTopRayPoint.position, Vector2.right * rayLength, Color.green);

        // Horizontal-Middle ray
        RaycastHit2D horMidRaycastHit = Physics2D.Raycast(horMidRayPoint.position, Vector2.right, rayLength);
        Debug.DrawRay(horMidRayPoint.position, Vector2.right * rayLength, Color.white);

        // Horizontal-Bottom ray
        RaycastHit2D horBottomRaycastHit = Physics2D.Raycast(horBottomRayPoint.position, Vector2.right, rayLength);
        Debug.DrawRay(horBottomRayPoint.position, Vector2.right * rayLength, Color.yellow);

        // Angled Ray (for empty spaces)
        Vector2 rayEndPoint = RotateRay(verticalRayPoint, rayAngle, angledRayLenght);
        RaycastHit2D angledRaycastHit = Physics2D.Linecast(verticalRayPoint.position, rayEndPoint);
        Debug.DrawLine(verticalRayPoint.position, rayEndPoint, Color.blue);


        if ((crouching || crawling) && verRaycastHit.collider != null)
        {
            //keep crawling or crouching -- keep doing what you're doing
        }

        //we have empty space decide what to do
        //after we decide player can change his mind
        if (angledRaycastHit.collider == null)
        {
            //there ise a gap ready to jump over
            Debug.Log("jump over gap");
            jumping = true;
            grounded = false;
        }


        //we have 3 line casts for checking either we have somethin on our way like obstacles that we can jump over 
        //that we can climb or we can crouch or crawl
        //if there is something on top so we can crouch maybe
        if (horTopRaycastHit.collider != null)
        {
            //if there  is something in the middle so we can crawl maybe
            if (horMidRaycastHit.collider != null)
            {
                //if there is something in bottom too so we can try to climb
                if (horBottomRaycastHit.collider != null)
                {
                    //climb (running on wall)
                    Debug.Log("climb (running on wall)");
                }
                else
                {
                    //crawl
                    Debug.Log("crawl");
                }
            }
            //if there is nothing in middle
            else
            {
                //if there is something in bottom too so we can try to climb
                if (horBottomRaycastHit.collider != null && horBottomRaycastHit.collider.tag != "Character")
                {
                    Debug.Log("jump between bottom and top");
                }
                else
                {
                    Debug.Log("crouch");
                }
            }
        }
        else
        {
            //if there  is something in the middle so we can jump and climb
            if (horMidRaycastHit.collider != null)
            {
                float randomNumber = Random.Range(0f, 1f);

                //if there is something in bottom too so we can try to climb
                if (horBottomRaycastHit.collider != null)
                {
                    //climb jump
                    Debug.Log("climb (jump)");
                }
                else if (randomNumber < 0.5)
                {
                    //crawl
                    Debug.Log("crawl");

                }
                else
                {
                    // climb Jump
                    Debug.Log("climb (jump)");
                }
            }
            else
            {
                //if there is something in bottom too so we can try to climb
                if (horBottomRaycastHit.collider != null)
                {
                    //jump over bottom
                    Debug.Log("jump over bottom");

                    velocity.y = 8f;
                    animator.SetBool("jumpOverBox", true);
                    
                }
                else
                {
                    //walk trough
                }
            }
        }
    }

    private Vector2 RotateRay(Transform t, float angle, float length)
    {
        Vector2 v = new Vector2(length, 0);
        float newX = v.x * Mathf.Cos(angle * Mathf.Deg2Rad) + v.y * -Mathf.Sin(angle * Mathf.Deg2Rad);
        float newY = v.x * Mathf.Sin(angle * Mathf.Deg2Rad) + v.y * Mathf.Cos(angle * Mathf.Deg2Rad);

        newX += t.position.x;
        newY += t.position.y;
        return new Vector2(newX, newY);
    }
}
