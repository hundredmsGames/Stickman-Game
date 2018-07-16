using UnityEngine;

public partial class CharacterController
{
    public Transform verticalRayPoint;
    public Transform horRayPoint_1;
    public Transform horRayPoint_2;
    public Transform horRayPoint_3;

    private RaycastHit2D verRaycastHit;
    private RaycastHit2D horRaycastHit_1;
    private RaycastHit2D horRaycastHit_2;
    private RaycastHit2D horRaycastHit_3;
    private RaycastHit2D angledRaycastHit;

    private float verHitLen;
    private float horHitLen_1;
    private float horHitLen_2;
    private float horHitLen_3;
    private float angledHitLen;

    public float rayLength;
    private float angledRayLength = 30;
    public float rayAngle;

    private void UpdateCharacterState()
    {
        CheckRays();
        CheckGrounded();
        CheckJumping();
        CheckFalling();
        CheckJumpOver();
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

    private void CheckJumpOver()
    {
        if(horHitLen_1 < 1f)
        {
            jumpOver = true;    
        }
        else
        {
            jumpOver = false;
        }
    }

    private void CheckJumping()
    {
        if(grounded == true && angledRaycastHit.collider == null)
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
        if (grounded != true && rigidBody.velocity.y < 0f)
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
        horRaycastHit_3 = Physics2D.Raycast(horRayPoint_3.position, Vector2.right, rayLength);
        Debug.DrawRay(horRayPoint_3.position, Vector2.right * rayLength, Color.green);

        // Horizontal-Middle ray
        horRaycastHit_2 = Physics2D.Raycast(horRayPoint_2.position, Vector2.right, rayLength);
        Debug.DrawRay(horRayPoint_2.position, Vector2.right * rayLength, Color.white);

        // Horizontal-Bottom ray
        horRaycastHit_1 = Physics2D.Raycast(horRayPoint_1.position, Vector2.right, rayLength);
        Debug.DrawRay(horRayPoint_1.position, Vector2.right * rayLength, Color.yellow);

        // Angled Ray (for empty spaces)
        Vector2 rayEndPoint = RotateRay(verticalRayPoint, rayAngle, angledRayLength);
        angledRaycastHit = Physics2D.Linecast(verticalRayPoint.position, rayEndPoint);
        Debug.DrawLine(verticalRayPoint.position, rayEndPoint, Color.blue);

        verHitLen = float.PositiveInfinity;
        horHitLen_1 = float.PositiveInfinity;
        horHitLen_2 = float.PositiveInfinity;
        horHitLen_3 = float.PositiveInfinity;
        angledHitLen = float.PositiveInfinity;


        if (verRaycastHit.collider != null)
        {
            verHitLen = verRaycastHit.distance;
        }

        if(angledRaycastHit.collider != null)
        {
            angledHitLen = angledRaycastHit.distance;
        }

        if (horRaycastHit_1.collider != null)
        {
            horHitLen_1 = horRaycastHit_1.distance;
        }

        if (horRaycastHit_2.collider != null)
        {
            horHitLen_2 = horRaycastHit_2.distance;
        }

        if (horRaycastHit_3.collider != null)
        {
            horHitLen_3 = horRaycastHit_3.distance;
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
}
