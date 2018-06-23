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

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Run
        animator.SetFloat("Speed", 1f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

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
