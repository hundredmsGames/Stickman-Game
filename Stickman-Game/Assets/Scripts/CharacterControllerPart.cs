using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterController : MonoBehaviour
{

    void CheckRaysRENAME()
    {
        RaycastHit2D raycastHit2D;
        //for debug
        Debug.DrawLine(topAngledRayPoint.transform.position, RotateRay(topAngledRayPoint, 90, rayLength), Color.red);
        Debug.DrawLine(topAngledRayPoint.transform.position, RotateRay(topAngledRayPoint, -45f, rayLength), Color.blue);
        Debug.DrawLine(horizontalTop.transform.position, RotateRay(horizontalTop, 0, rayLength), Color.green);
        Debug.DrawLine(horizontalMiddle.transform.position, RotateRay(horizontalMiddle, 0, rayLength), Color.white);
        Debug.DrawLine(horizontalBottom.transform.position, RotateRay(horizontalBottom, 0, rayLength), Color.yellow);

        //up line cast
        raycastHit2D = Physics2D.Linecast(topAngledRayPoint.transform.position, RotateRay(topAngledRayPoint, 90f, rayLength));
        if (/*charachter is crouching or crawling and*/ raycastHit2D.collider != null)
        {
            //keep crawling or crouching -- keep doing what you're doing
        }

        //angled lineCast for empty spaces WE ARE GONNA JUMP
        raycastHit2D = Physics2D.Linecast(topAngledRayPoint.transform.position, RotateRay(topAngledRayPoint, -45f, rayLength));
        //we have empty space decide what to do
        //after we decide player can change his mind
        if (raycastHit2D.collider != null)
        {
            //there ise a gap ready to jump over
            Debug.Log("jump over gap");
        }


        //we have 3 line casts for checking either we have somethin on our way like obstacles that we can jump over 
        //that we can climb or we can crouch or crawl
        raycastHit2D = Physics2D.Linecast(horizontalTop.transform.position, RotateRay(horizontalTop, 0f, rayLength));
        //if there is something on top so we can crouch maybe
        if (raycastHit2D.collider != null)
        {
            raycastHit2D = Physics2D.Linecast(horizontalMiddle.transform.position, RotateRay(horizontalMiddle, 0f, rayLength));
            //if there  is something in the middle so we can crawl maybe
            if (raycastHit2D.collider != null)
            {
                raycastHit2D = Physics2D.Linecast(horizontalBottom.transform.position, RotateRay(horizontalBottom, 0f, rayLength));
                //if there is something in bottom too so we can try to climb
                if (raycastHit2D.collider != null)
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
                raycastHit2D = Physics2D.Linecast(horizontalBottom.transform.position, RotateRay(horizontalBottom, 0f, rayLength));
                //if there is something in bottom too so we can try to climb
                if (raycastHit2D.collider != null && raycastHit2D.collider.tag != "Character")
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
            raycastHit2D = Physics2D.Linecast(horizontalMiddle.transform.position, RotateRay(horizontalMiddle, 0f, rayLength));
            //if there  is something in the middle so we can jump and climb
            if (raycastHit2D.collider != null)
            {
                float randomNumber = Random.Range(0f, 1f);

                raycastHit2D = Physics2D.Linecast(horizontalBottom.transform.position, RotateRay(horizontalBottom, 0f, rayLength));
                //if there is something in bottom too so we can try to climb
                if (raycastHit2D.collider != null)
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
                raycastHit2D = Physics2D.Linecast(horizontalBottom.transform.position, RotateRay(horizontalBottom, 0f, rayLength));
                //if there is something in bottom too so we can try to climb
                if (raycastHit2D.collider != null)
                {
                    //jump over bottom
                    Debug.Log("jump over bottom");
                }
                else
                {
                    //walk trough
                }
            }
        }






    }
}
