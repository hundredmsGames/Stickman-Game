using System;
using UnityEngine;

public partial class CharacterController : MonoBehaviour
{
    //Components
    Rigidbody2D rigidbody2D;

    //vars
    Vector2 force;
    float xForce = 25f;
    float yForce = 0f;
    float maxSpeed = 5f;

    private void Start()
    {
        force = new Vector2(xForce, yForce);
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (rigidbody2D.velocity.x < maxSpeed)
        {
            rigidbody2D.AddForce(force, ForceMode2D.Force);
        }
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), rigidbody2D.velocity.x + "");
    }
}
