using UnityEngine;
using Dreamteck.Splines;

public class UnitMovement : MonoBehaviour
{
    public SplineComputer spline;
    public float moveSpeed = 5f;
    public LayerMask collisionLayers;

    private int currentPointIndex = 0;
    private bool isMoving = true;

    void Start()
    {
        if (spline == null)
        {
            Debug.LogError("Spline is not assigned to " + gameObject.name);
            return;
        }

        transform.position = spline.GetPointPosition(0);
    }

    void Update()
    {
        if (!isMoving) return;

        MoveAlongSpline();
    }

    void MoveAlongSpline()
    {
        if (currentPointIndex >= spline.pointCount - 1) return;

        Vector3 startPosition = spline.GetPointPosition(currentPointIndex);
        Vector3 endPosition = spline.GetPointPosition(currentPointIndex + 1);

        float distanceToMove = moveSpeed * Time.deltaTime;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, endPosition, distanceToMove);

        if (CheckCollision(newPosition))
        {
            isMoving = false;
            return;
        }

        transform.position = newPosition;

        if (Vector3.Distance(transform.position, endPosition) < 0.01f)
        {
            currentPointIndex++;
        }
    }

    bool CheckCollision(Vector3 newPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition, 0.1f, collisionLayers);
        return colliders.Length > 1; // 자기 자신을 제외한 다른 콜라이더가 있는지 확인
    }

    public void ResumeMovement()
    {
        isMoving = true;
    }
}