using System;
using UnityEngine;

public partial class CharacterController : MonoBehaviour
{
    private GameController gameController;

    //Components
    Rigidbody2D rigidbody2D;
    Vector2 startPos;

    //vars
    Vector2 force;
    float xForce = 25f;
    float yForce = 0f;
    float maxSpeed = 7f;

    private void Start()
    {
        force = new Vector2(xForce, yForce);
        rigidbody2D = GetComponent<Rigidbody2D>();
        startPos = transform.position;

        gameController = GameController.Instance;
    }

    private void Update()
    {
        if (transform.position.y < -20f || transform.position.x > 180f)
            gameController.Failed();

    }

    private void FixedUpdate()
    {
        if (rigidbody2D.velocity.x < maxSpeed)
        {
            rigidbody2D.AddForce(force, ForceMode2D.Force);
        }
    }

    public void ResetCharacter()
    {
        transform.position = startPos;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Trap")
            gameController.Failed();
        else if (col.gameObject.tag == "Water")
            gameController.Failed();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Finish")
            gameController.FinishedGame();
        else if (col.tag == "Trap")
            gameController.Failed();
    }
}
