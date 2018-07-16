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

        if (angledRaycastHit.collider != null)
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
