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

    // 충돌 관련 변수들
    private float collisionCheckInterval = 0.2f; // 더 자주 체크하도록 수정
    private float lastCollisionCheckTime = 0f;
    private float stuckTimeout = 2f; // 최대 멈춤 시간
    private float stuckTimer = 0f;
    private Vector3 lastPosition;
    private float positionCheckThreshold = 0.01f;
    private float lastPositionCheckTime = 0f;
    private float positionCheckInterval = 0.5f;

    void Start()
    {
        if (spline == null)
        {
            Debug.LogError("Spline is not assigned!");
            return;
        }

        transform.position = spline.GetPointPosition(0);
        lastPosition = transform.position;

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
            HandleStuckState();
            return;
        }

        CheckForStuckState();
        MoveAlongSpline();
    }

    void HandleStuckState()
    {
        if (Time.time - lastCollisionCheckTime >= collisionCheckInterval)
        {
            CheckAndResumeMovement();
            lastCollisionCheckTime = Time.time;
        }

        // 일정 시간이 지나면 강제로 이동 재개
        stuckTimer += Time.deltaTime;
        if (stuckTimer >= stuckTimeout)
        {
            ForceResumeMovement();
        }
    }

    void CheckForStuckState()
    {
        if (Time.time - lastPositionCheckTime >= positionCheckInterval)
        {
            if (Vector3.Distance(transform.position, lastPosition) < positionCheckThreshold)
            {
                // 위치가 거의 변하지 않았다면 강제로 이동 재개
                ForceResumeMovement();
            }

            lastPosition = transform.position;
            lastPositionCheckTime = Time.time;
        }
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
            stuckTimer = 0f;
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

    void ForceResumeMovement()
    {
        // 현재 위치에서 약간 앞으로 이동시켜 충돌에서 벗어나게 함
        if (currentPointIndex < spline.pointCount - 1)
        {
            Vector3 nextPoint = spline.GetPointPosition(currentPointIndex + 1);
            Vector3 direction = (nextPoint - transform.position).normalized;
            transform.position += direction * 0.2f;
        }

        stuckTimer = 0f;
        ResumeMovement();
    }

    public void ResumeMovement()
    {
        isMoving = true;
    }
}