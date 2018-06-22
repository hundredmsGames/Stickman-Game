using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 3f;

    private Rigidbody2D rigidbody;

    bool mouseUp;

	// Use this for initialization
	void Start ()
    {
        rigidbody = GetComponent<Rigidbody2D>();
      
    }
	
	// Update is called once per frame
	void Update ()
    {
        rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);

        if (Input.GetMouseButtonUp(0))
            mouseUp = true;
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("hi");
        //if (mouseUp)
        //{
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX |
                        RigidbodyConstraints2D.FreezeRotation;

            mouseUp = false;
        //}
    }
}
