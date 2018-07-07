using UnityEngine;

public partial class CharacterController
{
    public Transform verticalRayPoint;
    public Transform horTopRayPoint;
    public Transform horMidRayPoint;
    public Transform horBottomRayPoint;

    private RaycastHit2D verRaycastHit;
    private RaycastHit2D horTopRaycastHit;
    private RaycastHit2D horMidRaycastHit;
    private RaycastHit2D horBottomRaycastHit;
    private RaycastHit2D angledRaycastHit;

    public float rayLength;
    private float angledRayLenght=100;
    public float rayAngle;


    public const int BOT_HOR_RAY = 1;
    public const int MID_HOR_RAY = 2;
    public const int TOP_HOR_RAY = 4;
    public const int ANGLED_RAY = 8;
    public const int VERTICAL_RAY= 16;

    /*  Most valuable bit is the leftmost one.
     *  
     *  Index 0: Bottom Horizontal Ray
     *  Index 1: Middle Horizontal Ray
     *  Index 2: Top    Horizontal Ray
     *  Index 3: Angled Ray
     *  Index 4: Vertical Ray
     *  
     */
    private int characterState;

    private void UpdateCharacterState()
    {
        // Set character's state to 0 before doing anything.
        characterState = 0;

        CheckRays();
        CheckGrounded();
        CheckJumping();
        CheckFalling();
        
        //PrintCharacterState();
    }

    private void CheckGrounded()
    {
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(groundedTransform.position, 1f, groundedLayerMask);

        if (colliders2D.Length > 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void CheckJumping()
    {
        if(grounded == true && (horBottomRaycastHit.collider != null ||
            angledRaycastHit.collider == null))
        {
            jumping = true;
        }
        else
        {
            jumping = false;
        }
    }

    private void CheckFalling()
    {
        if (rigidBody.velocity.y < -5f)
        {
            falling = true;
        }
        else
        {
            falling = false;
        }
    }

    private void CheckRays()
    {
        // Vertical ray, (towards up)
        verRaycastHit = Physics2D.Raycast(verticalRayPoint.position, Vector2.up, rayLength);
        Debug.DrawRay(verticalRayPoint.position, Vector2.up * rayLength, Color.red);

        // Horizontal-Top ray
        horTopRaycastHit = Physics2D.Raycast(horTopRayPoint.position, Vector2.right, rayLength);
        Debug.DrawRay(horTopRayPoint.position, Vector2.right * rayLength, Color.green);

        // Horizontal-Middle ray
        horMidRaycastHit = Physics2D.Raycast(horMidRayPoint.position, Vector2.right, rayLength);
        Debug.DrawRay(horMidRayPoint.position, Vector2.right * rayLength, Color.white);

        // Horizontal-Bottom ray
        horBottomRaycastHit = Physics2D.Raycast(horBottomRayPoint.position, Vector2.right, rayLength);
        Debug.DrawRay(horBottomRayPoint.position, Vector2.right * rayLength, Color.yellow);

        // Angled Ray (for empty spaces)
        Vector2 rayEndPoint = RotateRay(verticalRayPoint, rayAngle, angledRayLenght);
        angledRaycastHit = Physics2D.Linecast(verticalRayPoint.position, rayEndPoint);
        Debug.DrawLine(verticalRayPoint.position, rayEndPoint, Color.blue);

        if(verRaycastHit.collider != null)
        {
            characterState |= VERTICAL_RAY;
        }

        if(angledRaycastHit.collider != null)
        {
            characterState |= ANGLED_RAY;
        }

        if(horTopRaycastHit.collider != null)
        {
            characterState |= TOP_HOR_RAY;
        }

        if(horMidRaycastHit.collider != null)
        {
            characterState |= MID_HOR_RAY;
        }

        if(horBottomRaycastHit.collider != null)
        {
            characterState |= BOT_HOR_RAY;
        }

        #region Old Desicion tree
        /*
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
                */
        #endregion
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

    private void PrintCharacterState()
    {
        int temp = characterState;
        string prnt = "";

        if (temp == 0)
            prnt = "0";
        else
        {
            prnt = GetBinaryFormOfCharacterState();
        }

        Debug.Log(prnt);
    }

    private string GetBinaryFormOfCharacterState()
    {
        int temp = characterState;
        string binary = "";
        while (temp != 0)
        {
            binary = (temp % 2) + binary;
            temp /= 2;
        }
        return binary;
    }
}
