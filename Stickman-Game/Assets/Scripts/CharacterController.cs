using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidBody;
    public DrawLine2D drawLine;

    public float speed;
    bool triggered;

    


    private void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // Run
        animator.SetFloat("Speed", 1f);
    }
	
    private void Update ()
    {
        CheckLine();
        CheckRays();
    }

    private void FixedUpdate()
    {
        // Update character's position
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Wall")
            return;

        // TODO: Check which part of the body is damaged and save it.
        triggered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggered = false;
    }

    // TODO: We can find better names for these methods.
    private void CheckLine()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Line is in the character, stop character
            if (triggered == true)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX |
                                RigidbodyConstraints2D.FreezeRotation;

                // Don't enable collider. Because it has side effects like throwing character.
                drawLine.StopDrawing(false);
            }
            // Line is not in the character
            else
            {
                // Enable collider because line is not in the character.
                drawLine.StopDrawing(true);
            }
        }
    }
}
