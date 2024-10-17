using UnityEngine;
using Dreamteck.Splines;

public class UnitMovement : MonoBehaviour
{
    public SplineComputer spline;
    public float moveSpeed = 2f;
    public LayerMask collisionLayers;

    private int currentPointIndex = 0;
    public bool isMoving = true;

    private EnemyStats enemyStats;
    private float collisionCheckInterval = 0.5f;
    private float lastCollisionCheckTime = 0f;

    void Start()
    {
        if (spline == null)
        {
            return;
        }
        transform.position = spline.GetPointPosition(0);
        enemyStats = gameObject.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            moveSpeed = enemyStats.moveSpeed;
        }
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Time.time - lastCollisionCheckTime >= collisionCheckInterval)
            {
                CheckAndResumeMovement();
                lastCollisionCheckTime = Time.time;
            }
            return;
        }
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
            lastCollisionCheckTime = Time.time;
            return;
        }
        transform.position = newPosition;
        if (Vector3.Distance(transform.position, endPosition) < 0.01f)
        {
            currentPointIndex++;
        }
    }

    bool CheckCollision(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f, collisionLayers);
        return colliders.Length > 1;
    }

    void CheckAndResumeMovement()
    {
        if (!CheckCollision(transform.position))
        {
            ResumeMovement();
        }
    }

    public void ResumeMovement()
    {
        isMoving = true;
    }
}